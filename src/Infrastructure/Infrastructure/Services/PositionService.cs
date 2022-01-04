namespace RocketStore.Infrastructure.Services
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;
    using RocketStore.Application.Common.Interfaces;
    using RocketStore.Domain.Entities;

    public class PositionService : IPositionService
    {
        public async Task<AddressData> GetAddress(string address)
        {
            var uriBuilder = new UriBuilder("http://api.positionstack.com/v1/forward");
            var client = new HttpClient();
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            var queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
            queryString["access_key"] = "0d425df114162e6dd8264cd302f4fe1f";
            queryString["query"] = address;
            uriBuilder.Query = queryString.ToString();

            var response = await client.GetAsync(uriBuilder.Uri);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();

                var forwardResponse = JsonSerializer.Deserialize<ForwardResponse>(content, jsonOptions);

                return forwardResponse?.Data.FirstOrDefault();
            }

            return null;
        }
    }
}