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
    internal static class GitHubAuthService
    {
        /// <summary>
        /// Gets a GitHub access token.
        /// </summary>
        /// <param name="appConfig">The application configuration object which contains values
        /// for connecting to the specified GitHub repository.</param>
        /// <param name="privateKey">The RSA private key of a registered GitHub app installed in the specified repository.</param>
        /// <returns>Access token.</returns>
        internal static async Task<string> GetGithubAppTokenAsync(ApplicationConfig appConfig, string privateKey)
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
                var dtoUtc = DateTimeOffset.UtcNow;
                var payload = new Dictionary<string, object>
                {
                    {"iat", dtoUtc.ToUnixTimeSeconds()},
                    {"exp", dtoUtc.AddMinutes(10).ToUnixTimeSeconds()}, // 10 minutes is the maximum time allowed
                    {"iss", appConfig.GitHubAppId} // The GitHub App Id
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
            catch
            {
                throw;
            }
        }
    }
}
