﻿using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using RocketStoreApi.Controllers;

namespace RocketStoreApi.Tests
{
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Defines a test fixture used to test the <see cref="CustomersController"/>.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public sealed partial class CustomersFixture : IDisposable
    {
        // Ignore Spelling: json

        private bool disposed;

        private TestServer Server
        {
            get;
        }

        private HttpClient Client
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersFixture"/> class.
        /// </summary>
        public CustomersFixture()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = configurationBuilder.Build();

            IWebHostBuilder webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseConfiguration(configuration)
                .UseStartup<Startup>();

            this.Server = new TestServer(webHostBuilder);

            this.Client = this.Server.CreateClient();
        }
        
        /// <summary>
        /// Send a post request.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="endpointPath">The endpoint path.</param>
        /// <param name="model">The model instance.</param>
        /// <returns>
        /// The <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The <see cref="HttpResponseMessage"/> instance.
        /// </returns>
        public async Task<HttpResponseMessage> PostAsync<T>(string endpointPath, T model)
        {
            string json = JsonSerializer.Serialize(model);

            using StringContent content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            return await this.Client.PostAsync(
                new Uri($"{this.Server.BaseAddress}{endpointPath}"),
                content)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Send a Get request.
        /// </summary>
        /// <param name="endpointPath">Endpoint path.</param>
        /// <param name="model">model.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>HttpResponseMessage.</returns>
        public Task<HttpResponseMessage> GetAsync<T>(string endpointPath, T model)
        {
            var uriBuilder = new UriBuilder($"{this.Server.BaseAddress}{endpointPath}");
            uriBuilder.Query = GetQueryString(model);

            return this.Client.GetAsync(
                uriBuilder.Uri);
        }

        /// <summary>
        /// Send a Get request.
        /// </summary>
        /// <param name="endpoint">Endpoint path.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>HttpResponseMessage.</returns>
        public Task<HttpResponseMessage> GetAsync(Uri endpoint)
        {
            return this.Client.GetAsync(endpoint);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.Client != null)
                    {
                        this.Client.Dispose();
                    }

                    if (this.Server != null)
                    {
                        this.Server.Dispose();
                    }
                }

                this.disposed = true;
            }
        }

        private static string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                where p.GetValue(obj, null) != null
                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }
    }
}
