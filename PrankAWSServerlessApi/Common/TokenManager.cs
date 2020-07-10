using Microsoft.IdentityModel.Tokens;
using Prank.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrankAWSServerlessApi.Common
{
    public class TokenManager
    {
        public static string GenerateToken(string randomString, string secretKey, string issuer, string audience)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, randomString),
                    new Claim(MemberPropertyEnum.MemberEmail.ToString(), "Prank@prank.com"),
                }),
                Expires = DateTime.Now.AddDays(7),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            token.Payload["favouriteFood"] = "cheese";

            return handler.WriteToken(token);
        }
        public static MemberModel GetMemberInfoByToken(string token, string secretKey)
        {
            var model = new MemberModel();
            ClaimsPrincipal principal = GetPrincipal(token, secretKey);

            if (principal == null)
                return null;

            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            var firstNameClaim = identity.FindFirst(MemberPropertyEnum.MemberFirstName.ToString());
            model.FirstName = firstNameClaim != null ? firstNameClaim.Value : string.Empty;

            var middleNameClaim = identity.FindFirst(MemberPropertyEnum.MemberMiddleName.ToString());
            model.MiddleName = middleNameClaim != null ? middleNameClaim.Value : string.Empty;

            var lastNameClaim = identity.FindFirst(MemberPropertyEnum.MemberLastName.ToString());
            model.LastName = lastNameClaim != null ? lastNameClaim.Value : string.Empty;

            var emailClaim = identity.FindFirst(MemberPropertyEnum.MemberEmail.ToString());
            model.EmailAddress = emailClaim != null ? emailClaim.Value : string.Empty;



            var memberGuidClaim = identity.FindFirst(MemberPropertyEnum.MemberGuid.ToString());
            model.MemberGuid = memberGuidClaim != null ? memberGuidClaim.Value : string.Empty;

            return model;
        }
        public static string ValidateToken(string token, string secretKey)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token, secretKey);

            if (principal == null)
                return null;

            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;

            return username;
        }
        public static ClaimsPrincipal GetPrincipal(string token, string secretKey)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                byte[] key = Convert.FromBase64String(secretKey);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
