// ------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------------------------------------------------------------------------------

// Partially ported from: https://github.com/microsoftgraph/msgraph-samples-dashboard/blob/dev/SamplesDashboard/SamplesDashboard/Helper/JWTHelper.cs

using System.Collections.Generic;
using Jose;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Security;

namespace GitHubContentUtility.Helpers
{
    /// <summary>
    /// Provides methods for extracting a private key and using it to encode a Jwt token.
    /// </summary>
    internal class JwtHelper
    {
        /// <summary>
        /// Gets private key and uses it to decode the jwt token.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="payload"></param>
        /// <returns>jwt token</returns>
        internal static string CreateEncodedJwtToken(string key, Dictionary<string, object> payload)
        {
            // Generate JWT
            using var rsa = new RSACryptoServiceProvider();
            var rsaParams = DotNetUtilities.ToRSAParameters(GetPrivateKeyParameters(key));
            rsa.ImportParameters(rsaParams);
            return JWT.Encode(payload, rsa, JwsAlgorithm.RS256);
        }

        private static RsaPrivateCrtKeyParameters GetPrivateKeyParameters(string privateKey)
        {
            using var privateKeyReader = new StringReader(privateKey);
            var pemReader = new PemReader(privateKeyReader);
            var keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            return (RsaPrivateCrtKeyParameters)keyPair.Private;
        }
    }
}
