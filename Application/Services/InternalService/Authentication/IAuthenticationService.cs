using Domain.Models.Common;

namespace Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Guid GetUserId();
        Guid GetSessionId();
        bool IsAuthenticated();
    }
}