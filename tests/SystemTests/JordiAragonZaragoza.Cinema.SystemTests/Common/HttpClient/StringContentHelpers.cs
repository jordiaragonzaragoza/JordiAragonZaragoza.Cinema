namespace JordiAragonZaragoza.Cinema.SystemTests.Common.HttpClient
{
    using System.Net.Http;
    using System.Text;

    using System.Text.Json;

    public static class StringContentHelpers
    {
        public static StringContent FromModelAsJson(object model)
        {
            return new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        }
    }
}