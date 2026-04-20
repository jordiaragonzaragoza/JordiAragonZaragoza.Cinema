namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Services
{
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public class AuthorizationService : IAuthorizationService
    {
        // TODO: Add CacheService and implement caching of the authorization results or cache repository.
        public Task<Result> ValidateScopeAsync(ExecutionContext executionContext)
        {
            // TODO: Complete.
            return Task.FromResult(Result.Success());
        }
    }
}