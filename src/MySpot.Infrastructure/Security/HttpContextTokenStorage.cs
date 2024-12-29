using Microsoft.AspNetCore.Http;
using MySpot.Application.DTO;
using MySpot.Application.Security;

namespace MySpot.Infrastructure.Security;

internal sealed class HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor): ITokenStorage
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private const string TokenKey = "jwt";
    public void Set(JwtDto jwt) => _httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwt);

    public JwtDto Get()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return null;
        }

        if (_httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwt))
        {
            return jwt as JwtDto;
        }

        return null;
    }
}