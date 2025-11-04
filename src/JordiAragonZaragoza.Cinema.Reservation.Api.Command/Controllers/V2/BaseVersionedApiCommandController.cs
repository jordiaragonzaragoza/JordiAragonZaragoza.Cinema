namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2
{
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [ApiVersion("2.0", Deprecated = false)]
    public abstract class BaseVersionedApiCommandController : BaseApiCommandController
    {
    }
}