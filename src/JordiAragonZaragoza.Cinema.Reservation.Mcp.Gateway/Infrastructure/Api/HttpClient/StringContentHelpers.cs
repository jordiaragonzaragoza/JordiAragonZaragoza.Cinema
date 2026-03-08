namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.HttpClient
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