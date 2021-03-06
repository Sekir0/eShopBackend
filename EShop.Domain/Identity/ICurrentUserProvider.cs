using Microsoft.AspNetCore.Http;

namespace EShop.Domain.Identity
{
    public interface ICurrentUserProvider
    {
        public string UserId { get; }
    }
    
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserProvider(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        
        public string UserId => _httpContext.HttpContext?.User.FindFirst(s => s.Type == "id")?.Value;
    }
}