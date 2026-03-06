namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.Command
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CommandServiceOptions
    {
        public const string Section = "CommandService";

        [Required]
        public Uri Url { get; init; } = default!;
    }
}