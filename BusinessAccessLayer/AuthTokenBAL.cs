using DataAccessLayer.Log;
using DataObject;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer
{
    public class AuthTokenBAL
    {
        private static readonly IConfigurationRoot configuration;
        private static readonly string jwtKey;
        private static readonly int jwtExpireMinutes;


        static AuthTokenBAL()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
            jwtKey = configuration["JwtSettings:Key"];
            jwtExpireMinutes = int.TryParse(configuration["JwtSettings:ExpireMinutes"], out int minutes) ? minutes : 60;
        }

        public static string GenerateJwtToken(List<UserLoginData> UserData)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                int UserId = UserData[0].UserId;
                string UserName = UserData[0].Username;

                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, UserName),
            new Claim("user_id", UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(jwtExpireMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("AuthTokenBAL", "GenerateJwtToken", "Error generating JWT token.", ex.StackTrace, ex.Message, 0);

                return null;
            }
        }

        public static List<string> CheckValidToken(string authorizationHeader)
        {
            List<string> lstOutput = new List<string>();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                lstOutput.Add("false");
                lstOutput.Add("Authorization header is missing.");
                LoggerDAL.FnStoreErrorLog("AuthTokenBAL", "CheckValidToken", "Error Occured during CheckValidToken.", "", "", 0);

                return lstOutput;
            }

            try
            {
                if (!AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue) ||
                    string.IsNullOrWhiteSpace(headerValue.Parameter))
                {
                    lstOutput.Add("false");
                    lstOutput.Add("Invalid Authorization header format.");
                    LoggerDAL.FnStoreErrorLog("AuthTokenBAL", "CheckValidToken", "Invalid Authorization header format.", "", "", 0);

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
                LoggerDAL.FnStoreErrorLog("AuthTokenBAL", "CheckValidToken", "Error while validating token.", ex.StackTrace, ex.Message, 0);

            }

            return lstOutput;
        }

        public static bool ValidateCurrentToken(string token)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch(Exception ex) 
            {
                LoggerDAL.FnStoreErrorLog("AuthTokenBAL", "ValidateCurrentToken", "Error while ValidateCurrentToken.", ex.StackTrace, ex.Message, 0);

                return false;
            }
        }
        // To Extarct UserId Rohit R 22-07-2025
        public static int? GetUserIdFromToken(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                LoggerDAL.FnStoreErrorLog("AuthTokenBAL", "GetUserIdFromToken", "authorizationHeader cannot be null", "", "", 0);

                return null;
            }
            if (!AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
            {
                LoggerDAL.FnStoreErrorLog("AuthTokenBAL", "GetUserIdFromToken", "Error while AuthenticationHeaderValue.", "", "", 0);

                return null;
            }
            var token = headerValue.Parameter;
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "user_id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            catch (Exception ex)
            {
                LoggerDAL.FnStoreErrorLog("AuthTokenBAL", "GetUserIdFromToken", "Error while GetUserIdFromToken.", ex.StackTrace, ex.Message, 0);

                return null;
            }

            return null;
        }


    }
}
