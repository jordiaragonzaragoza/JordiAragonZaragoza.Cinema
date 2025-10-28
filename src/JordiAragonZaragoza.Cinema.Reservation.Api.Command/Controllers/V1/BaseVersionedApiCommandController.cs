namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V1
{
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using Asp.Versioning;

    [ApiVersion("1.0", Deprecated = false)]
    public abstract class BaseVersionedApiCommandController : BaseApiCommandController
    {
    }
}