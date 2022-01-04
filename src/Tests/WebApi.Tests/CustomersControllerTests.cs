namespace RocketStoreApi.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using RocketStore.Application.Customer.Commands.CreateCustomer;
    using RocketStore.Application.Customer.Queries.GetCustomerDetail;
    using RocketStore.Application.Customer.Queries.GetCustomerList;
    using RocketStore.Application.Managers;
    using RocketStoreApi.Controllers;
    using Xunit;

    /// <summary>
    /// Provides integration tests for the <see cref="CustomersController"/> type.
    /// </summary>
    [SuppressMessage("Resharper", "CA1707", Justification = "Test methods can have underline in name of the methods.")]
    public class CustomersControllerTests : TestsBase, IClassFixture<CustomersFixture>
    {
        // Ignore Spelling: api

        private readonly CustomersFixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersControllerTests"/> class.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        public CustomersControllerTests(CustomersFixture fixture)
        {
            this.fixture = fixture;
        }

        /// <summary>
        /// Tests the <see cref="CustomersController.CreateCustomerAsync(CreateCustomerCommand)"/> method
        /// to ensure that it requires name and email.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        [Fact]
        public async Task CreateCustomerAsync_CreateWithoutRequiredFields_ReturnsBadRequestAsync()
        {
            // Arrange

            IDictionary<string, string[]> expectedErrors = new Dictionary<string, string[]>
            {
                { "Name", new string[] { "The Name field is required." } },
                { "EmailAddress", new string[] { "The EmailAddress field is required." } },
                { "Address", new string[] { "The Address field is required." } },
                { "VatNumber", new string[] { "The VatNumber field is required." } },
            };

            CreateCustomerCommand customer2 = new CreateCustomerCommand
            {
            };

            // Act

            HttpResponseMessage httpResponse = await this.fixture.PostAsync("api/customers", customer2).ConfigureAwait(false);

            // Assert

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            ValidationProblemDetails error = await this.GetResponseContentAsync<ValidationProblemDetails>(httpResponse).ConfigureAwait(false);
            error.Should().NotBeNull();
            error.Errors.Should().BeEquivalentTo(expectedErrors);
        }

        /// <summary>
        /// Tests the <see cref="CustomersController.CreateCustomerAsync(CreateCustomerCommand)"/> method
        /// to ensure that it requires a valid email address.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        [Fact]
        public async Task CreateCustomerAsync_CreateRequiresValidEmail_ReturnBadRequestAsync()
        {
            // Arrange

            IDictionary<string, string[]> expectedErrors = new Dictionary<string, string[]>
            {
                { "EmailAddress", new string[] { "The Email field is not a valid e-mail address." } }
            };

            var customer2 = new CreateCustomerCommand()
            {
                Name = "A customer",
                EmailAddress = "An invalid email",
                VatNumber = "123456789",
                Address = "Address"
            };

            // Act

            HttpResponseMessage httpResponse = await this.fixture.PostAsync("api/customers", customer2).ConfigureAwait(false);

            // Assert

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            ValidationProblemDetails error = await this.GetResponseContentAsync<ValidationProblemDetails>(httpResponse).ConfigureAwait(false);
            error.Should().NotBeNull();
            error.Errors.Should().BeEquivalentTo(expectedErrors);
        }

        /// <summary>
        /// Tests the <see cref="CustomersController.CreateCustomerAsync(CreateCustomerCommand)"/> method
        /// to ensure that it requires a unique email address.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        [Fact]
        public async Task CreateCustomerAsync_CreateRequiresUniqueEmail_ReturnCreatedAsync()
        {
            // Arrange
            var customerEmail = $"{GetRandomString()}@server.pt";

            var customer1 = new CreateCustomerCommand()
            {
                Name = "A customer",
                EmailAddress = customerEmail,
                VatNumber = "123456789",
                Address = "Address",
            };

            var customer2 = new CreateCustomerCommand()
            {
                Name = "Another customer",
                EmailAddress = customerEmail,
                VatNumber = "123456789",
                Address = "Address",
            };

            // Act

            HttpResponseMessage httpResponse1 = await this.fixture.PostAsync("api/customers", customer1).ConfigureAwait(false);

            HttpResponseMessage httpResponse2 = await this.fixture.PostAsync("api/customers", customer2).ConfigureAwait(false);

            // Assert

            httpResponse1.StatusCode.Should().Be(HttpStatusCode.Created);

            httpResponse2.StatusCode.Should().Be(HttpStatusCode.Conflict);

            ProblemDetails error = await this.GetResponseContentAsync<ProblemDetails>(httpResponse2).ConfigureAwait(false);
            error.Should().NotBeNull();
            error.Title.Should().Be(ErrorCodes.CustomerAlreadyExists);
        }

        /// <summary>
        /// Tests the <see cref="CustomersController.CreateCustomerAsync(CreateCustomerCommand)"/> method
        /// to ensure that it requires a valid customer.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        [Fact]
        public async Task CreateCustomerAsync_ValidCustomer_ReturnCreatedAsync()
        {
            // Arrange

            var customer2 = new CreateCustomerCommand()
            {
                Name = "My customer",
                EmailAddress = "mycustomer@server.pt",
                VatNumber = "123456789",
                Address = "Address"
            };

            // Act

            HttpResponseMessage httpResponse = await this.fixture.PostAsync("api/customers", customer2).ConfigureAwait(false);

            // Assert

            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            Guid? id = await this.GetResponseContentAsync<Guid?>(httpResponse).ConfigureAwait(false);
            id.Should().NotBeNull();

            httpResponse.Headers.Location.Should().NotBeNull();
        }

        /// <summary>
        /// Test.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetAllCustomerAsync_FilterByName_ReturnOneRecordAsync()
        {
            // Arrange

            var customer = new CreateCustomerCommand()
            {
                Name = "Filter By Name",
                EmailAddress = "filterbyname@server.pt",
                VatNumber = "123456789",
                Address = "Address"
            };

            HttpResponseMessage httpResponseInsert = await this.fixture.PostAsync("api/customers", customer).ConfigureAwait(false);

            // Act

            HttpResponseMessage httpResponse = await this.fixture.GetAsync("api/customers", new { filter = "Name" }).ConfigureAwait(false);

            // Assert

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var customers = await this.GetResponseContentAsync<IList<CustomersDto>>(httpResponse).ConfigureAwait(false);
            customers.Should().NotBeNull();
            customers.Should().ContainSingle();
        }
        
        /// <summary>
        /// Test.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetCustomerByIdAsync_InsertAndGet_ReturnOkAsync()
        {
            // Arrange

            var customer = new CreateCustomerCommand()
            {
                Name = "My customer",
                EmailAddress = "mycustomerbyid@server.pt",
                VatNumber = "123456789",
                Address = "Rua Castelo da Maia Ginasio Clube, 100",
            };

            HttpResponseMessage httpResponseInsert = await this.fixture.PostAsync("api/customers", customer).ConfigureAwait(false);

            // Act

            HttpResponseMessage httpResponse = await this.fixture.GetAsync(httpResponseInsert.Headers.Location).ConfigureAwait(false);

            // Assert

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var customers = await this.GetResponseContentAsync<CustomerDetailDto>(httpResponse).ConfigureAwait(false);
            customers.Should().NotBeNull();
            customers.Address.Should().NotBeNull();
        }
    }
}
