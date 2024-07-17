using System.Net.Http.Headers;
using System.Threading.Tasks;
using Light.GuardClauses;
using Microsoft.AspNetCore.Authentication;
using Yarp.ReverseProxy.Transforms;

namespace Gateway.Transformation;

public static class AuthenticationRequestTransform
{
    public static async ValueTask ApplyAsync(RequestTransformContext context)
    {
        var authenticateResult = context.HttpContext.Features.Get<IAuthenticateResultFeature>()?.AuthenticateResult;
        if (authenticateResult?.Ticket is null)
        {
            return;
        }

        switch (authenticateResult.Ticket.AuthenticationScheme)
        {
            case "Bearer":
                var accessToken = authenticateResult.Properties?.GetTokenValue("access_token");
                if (!accessToken.IsNullOrWhiteSpace())
                {
                    context.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                break;

            case "Cookie":
                var userTokens = await context.HttpContext.GetUserAccessTokenAsync(
                    cancellationToken: context.HttpContext.RequestAborted
                );
                context.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userTokens.AccessToken);
                break;
        }
    }
}