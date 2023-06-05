namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V1
{
    using JordiAragon.SharedKernel.Presentation.WebApi.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous] // This API does not use authentication.
    [ApiVersion("1.0", Deprecated = false)]
    public abstract class BaseVersionedApiController : BaseApiController
    {
    }
}