using Application.Common.Helper;

namespace Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.GetUserIdFromToken() ?? throw new Exception("Unauthorize!");
        }

        public Guid GetSessionId()
        {
            return _httpContextAccessor.HttpContext?.User.GetSessionIdFromToken() ?? throw new Exception("Unauthorize!");
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }

    }
}
