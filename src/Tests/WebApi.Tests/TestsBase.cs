namespace RocketStoreApi.Tests
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the base class for test classes.
    /// </summary>
    public abstract class TestsBase
    {
        private static Random random = new Random();

        #region Public Methods

        /// <summary>
        /// Gets the content from the specified response.
        /// </summary>
        /// <typeparam name="T">The content response.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>
        /// The <see cref="Task{TResult}"/> that represents the asynchronous operation.
        /// The <typeparamref name="T"/> instance.
        /// </returns>
        protected virtual async Task<T> GetResponseContentAsync<T>(HttpResponseMessage response)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        /// <summary>
        /// Get a Random String to be used in test methods.
        /// </summary>
        /// <returns>Random string based in a int number.</returns>
        protected static string GetRandomString()
        {
            const int maxString = 999999999;
            const int minString = 10000;

            return random.Next(minString, maxString).ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
