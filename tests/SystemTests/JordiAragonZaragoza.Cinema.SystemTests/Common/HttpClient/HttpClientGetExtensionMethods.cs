namespace JordiAragonZaragoza.Cinema.SystemTests.Common.HttpClient
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Xunit.Abstractions;

    public static class HttpClientGetExtensionMethods
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions = new()
        {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static async Task<T?> GetAndDeserializeAsync<T>(this HttpClient client, string requestUri, ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(requestUri);

            var uri = new Uri(requestUri, UriKind.RelativeOrAbsolute);
            return await GetAndDeserializeInternalAsync<T>(client, uri, output);
        }

        public static async Task<T?> GetAndDeserializeAsync<T>(this HttpClient client, Uri requestUri, ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(requestUri);

            return await GetAndDeserializeInternalAsync<T>(client, requestUri, output);
        }

        private static async Task<T?> GetAndDeserializeInternalAsync<T>(HttpClient client, Uri requestUri, ITestOutputHelper? output)
        {
            HttpResponseMessage response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                output?.WriteLine($"Response failed with status code: {response.StatusCode}");

                return default;
            }

            string text = await response.Content.ReadAsStringAsync();
            output?.WriteLine("Response: " + text);
            return JsonSerializer.Deserialize<T>(text, DefaultJsonOptions);
        }
    }
}