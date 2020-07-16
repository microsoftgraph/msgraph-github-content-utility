// ------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------------------------------------------------------------------------------

using GitHubContentUtility.Common;
using GitHubContentUtility.Helpers;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubContentUtility.Services
{
    /// <summary>
    /// Provides methods for authenticating a GitHub app with a GitHub repository to
    /// obtain a Jwt token for making calls to the respective repository.
    /// </summary>
    public static class GitHubAuthService
    {
        private static readonly long TicksSince197011 = new DateTime(1970, 1, 1).Ticks;

        public static async Task<string> GetGithubAppTokenAsync(ApplicationConfig appConfig, string privateKey)
        {
            if (appConfig == null)
            {
                throw new ArgumentNullException(nameof(appConfig), "Parameter cannot be null");
            }
            if (string.IsNullOrEmpty(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey), "Parameter cannot be null or empty");
            }

            try
            {
                var utcNow = DateTime.UtcNow;
                var payload = new Dictionary<string, object>
                {
                    {"iat", ToUtcSeconds(utcNow)},
                    {"exp", ToUtcSeconds(utcNow.AddSeconds(600))}, // 10 minutes is the maximum time allowed
                    {"iss",  appConfig.GitHubAppId} // The GitHub App Id
                };

                var jwtToken = JwtHelper.CreateEncodedJwtToken(privateKey, payload);

                // Pass the JWT as a Bearer token to Octokit.net
                var appClient = new GitHubClient(new ProductHeaderValue(appConfig.GitHubAppName))
                {
                    Credentials = new Credentials(jwtToken, AuthenticationType.Bearer)
                };

                // Get a list of installations for the authenticated GitHub App and installationID for the GitHub Organization
                var installations = await appClient.GitHubApps.GetAllInstallationsForCurrent();
                var id = installations.Where(installation => installation.Account.Login == appConfig.GitHubOrganization)
                    .FirstOrDefault().Id;

                // Create an Installation token for the GitHub Organization installation instance
                var response = await appClient.GitHubApps.CreateInstallationToken(id);

                return response.Token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static long ToUtcSeconds(DateTime dt)
        {
            return (dt.ToUniversalTime().Ticks - TicksSince197011) / TimeSpan.TicksPerSecond;
        }
    }
}
