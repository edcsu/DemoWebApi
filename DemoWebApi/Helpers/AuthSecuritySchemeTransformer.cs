using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace DemoWebApi.Helpers;

public sealed class AuthSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        var requirements = new Dictionary<string, OpenApiSecurityScheme>();

        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            requirements.Add("Bearer",  new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                BearerFormat = "Json Web Token"
            });
        }
        document.Components ??= new OpenApiComponents();
        document.Info.Contact = new OpenApiContact 
        {
            Name = "Ssewannonda Keith Edwin",
            Email = "skeith@696.gmail",
        };
        document.Components.SecuritySchemes = requirements;
    }
}