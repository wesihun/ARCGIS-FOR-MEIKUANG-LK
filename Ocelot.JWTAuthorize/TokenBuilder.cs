﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Ocelot.JwtAuthorize
{
    /// <summary>
    /// TokenBuilder
    /// </summary>
    public class TokenBuilder : ITokenBuilder
    {
        /// <summary>
        /// JwtAuthorizationRequirement
        /// </summary>
        readonly JwtAuthorizationRequirement _jwtAuthorizationRequirement;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="jwtAuthorizationRequirement"></param>
        public TokenBuilder(JwtAuthorizationRequirement jwtAuthorizationRequirement)
        {
            _jwtAuthorizationRequirement = jwtAuthorizationRequirement;
        }
        /// <summary>
        /// 获取jwt令牌
        /// </summary>
        /// <param name="claims">claim array</param>
        /// <param name="expires">expires</param>
        /// <returns></returns>
        public Token BuildJwtToken(Claim[] claims, DateTime? expires = null)
        {
            var claimList = new List<Claim>(claims);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _jwtAuthorizationRequirement.Issuer,
                audience: _jwtAuthorizationRequirement.Audience,
                claims: claimList.ToArray(),
                notBefore: now,
                expires: expires,
                signingCredentials: _jwtAuthorizationRequirement.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new Token
            {
                TokenValue = encodedJwt,
                Expires = expires,
                TokenType = "Bearer"
            };
            return responseJson;
        }
        /// <summary>
        /// 获取jwt令牌
        /// </summary>
        /// <param name="claims">claim array</param>
        /// <param name="notBefore">not Before time</param>
        /// <param name="expires">expires</param>
        /// <returns></returns>
        public Token BuildJwtToken(Claim[] claims, DateTime notBefore, DateTime? expires = null)
        {
            var claimList = new List<Claim>(claims);
            var jwt = new JwtSecurityToken(
                issuer: _jwtAuthorizationRequirement.Issuer,
                audience: _jwtAuthorizationRequirement.Audience,
                claims: claimList.ToArray(),
                notBefore: notBefore,
                expires: expires,
                signingCredentials: _jwtAuthorizationRequirement.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new Token
            {
                TokenValue = encodedJwt,
                Expires = expires,
                TokenType = "Bearer"
            };
            return responseJson;
        }

        /// <summary>
        /// 获取jwt令牌
        /// </summary>
        /// <param name="claims">claim array</param>
        /// <param name="ip">ip</param>
        /// <param name="notBefore">not Before time</param>
        /// <param name="expires">expires</param>
        /// <returns></returns>
        public Token BuildJwtToken(Claim[] claims, string ip, DateTime? notBefore = null, DateTime? expires = null)
        {
           
            var claimList = new List<Claim>(claims);
            claimList.Add(new Claim("ip", ip));
            var now = notBefore.HasValue ? notBefore.Value : DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _jwtAuthorizationRequirement.Issuer,
                audience: _jwtAuthorizationRequirement.Audience,
                claims: claimList.ToArray(),
                notBefore: notBefore,
                expires: expires,
                signingCredentials: _jwtAuthorizationRequirement.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new Token
            {
                TokenValue = encodedJwt,
                Expires = expires,
                TokenType = "Bearer"
            };
            return responseJson;
        }
        /// <summary>
        /// back token
        /// </summary>
        public class Token
        {
            /// <summary>
            /// Token Value
            /// </summary>
            public string TokenValue
            { get; set; }
            /// <summary>
            /// Expires (unit second)
            /// </summary>
            public DateTime? Expires
            { get; set; }
            /// <summary>
            /// token type
            /// </summary>
            public string TokenType
            { get; set; }
        }
    }
}
