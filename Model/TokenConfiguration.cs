using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Model
{
    public class TokenConfiguration
    {
        private SymmetricSecurityKey _securityKey;

        public const string ConfigurationSectionName = "TokenConfigurations";

        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInSeconds { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            if(_securityKey == null)
                _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            return _securityKey;
        }
    }
}
