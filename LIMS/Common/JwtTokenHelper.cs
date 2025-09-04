using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace LIMS.Common
{
    public class JwtTokenHelper
    {
        private static readonly IConfiguration configuration;

        static JwtTokenHelper()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }



        public static string GenerateJwtToken(int userId, string username)
        {
            var key = configuration["JwtSettings:Key"];
            var issuer = configuration["JwtSettings:Issuer"];
            var audience = configuration["JwtSettings:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("user_id", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Validate Token via Authorization Header (Bearer)
        public static List<string> CheckValidToken(string authorizationHeader)
        {
            List<string> lstOutput = new List<string>();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                lstOutput.Add("false");
                lstOutput.Add("Authorization header is missing.");
                return lstOutput;
            }

            try
            {
                if (!AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue) ||
                    string.IsNullOrWhiteSpace(headerValue.Parameter))
                {
                    lstOutput.Add("false");
                    lstOutput.Add("Invalid Authorization header format.");
                    return lstOutput;
                }

                var token = headerValue.Parameter;

                bool isValid = ValidateCurrentToken(token);

                lstOutput.Add(isValid ? "true" : "false");
                lstOutput.Add(isValid ? "" : "Invalid or expired token.");
            }
            catch (Exception ex)
            {
                lstOutput.Add("false");
                lstOutput.Add("Error while validating token: " + ex.Message);
            }

            return lstOutput;
        }

        // Validate JWT Token
        public static bool ValidateCurrentToken(string token)
        {
            try
            {
                var key = configuration["JwtSettings:Key"];
                var issuer = configuration["JwtSettings:Issuer"];
                var audience = configuration["JwtSettings:Audience"];

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        

    }
}
