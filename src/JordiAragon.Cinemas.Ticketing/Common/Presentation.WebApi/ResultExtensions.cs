namespace JordiAragon.Cinemas.Ticketing.Vertical.Common.Presentation.WebApi
{
    using System.Threading.Tasks;
    using Ardalis.Result;
    using FastEndpoints;

    // TODO: Move to SharedKernel.
    public static class ResultExtensions
    {
        public static async Task SendResponseAsync(this IEndpoint endpoint, IResult result)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok:
                    await endpoint.HttpContext.Response.SendAsync(result.GetValue());
                    break;

                case ResultStatus.Invalid:
                    result.ValidationErrors.ForEach(e =>
                        endpoint.ValidationFailures.Add(new(e.Identifier, e.ErrorMessage)));

                    await endpoint.HttpContext.Response.SendErrorsAsync(endpoint.ValidationFailures);
                    break;
            }
        }
    }
}