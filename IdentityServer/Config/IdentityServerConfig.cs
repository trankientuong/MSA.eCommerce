using Duende.IdentityServer.Models;

namespace IdentityServer.Config;

public static class IdentityServerConfig
{
    public static IEnumerable<IdentityResource> IdentityResources
    {
        get 
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
    }

    public static IEnumerable<ApiScope> ApiScopes
    {
        get 
        {
            return
            [
                new ApiScope("productapi.read"),
                new ApiScope("productapi.write")
            ];
        }
    }

    public static IEnumerable<ApiResource> ApiResources
    {
        get
        {
            return new List<ApiResource>()
            {
                new ApiResource("productapi","Product API") {
                    Scopes = new List<string> {"productapi.read", "productapi.write"},
                    ApiSecrets = new List<Secret> { new Secret("Scopesecret".Sha256())},
                    UserClaims = new List<string> {"role"}
                }
            };
        }
    }

    public static IEnumerable<Client> Clients
    {
        get
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                        AllowedScopes = {
                            "productapi.read",
                            "productapi.write"
                        }
                },
                new Client()
                {
                    ClientId = "angular-client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,  // Sử dụng PKCE để bảo mật hơn cho Angular SPA
                    RequireClientSecret = false,  // SPA không yêu cầu client secret

                    RedirectUris = { "http://localhost:4200/auth-callback" },  // URL callback của Angular app sau khi đăng nhập
                    PostLogoutRedirectUris = { "http://localhost:4200/" },  // URL khi đăng xuất
                    AllowedCorsOrigins = { "http://localhost:4200" },  // Cho phép CORS từ Angular
                    AllowedScopes = { "openid", "profile", "productapi.read", "productapi.write" }  // Các scope mà Angular yêu cầu
                },
                // product-swagger client using code flow + pkce
                new Client
                {
                    ClientId = "product-swagger",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "https://localhost:5002/swagger/oauth2-redirect.html" },
                    AllowedCorsOrigins = { "https://localhost:5002" },
                    AllowedScopes = { "openid", "profile", "productapi.read", "productapi.write" },

                    AllowOfflineAccess = true
                }
            };
        }
    }
}