// ------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------------------------------------------------------------------------------

using Octokit;
using System;
using GitHubContentUtility.Common;

namespace GitHubContentUtility.Services
{
    /// <summary>
    /// Provides utility method that returns an authenticated GitHub client.
    /// </summary>
    internal static class GitHubClientFactory
    {
        /// <summary>
        /// Gets an authenticated GitHub client.
        /// </summary>
        /// <param name="appConfig">The application configuration object which contains values
        /// for connecting to the specified GitHub repository.</param>
        /// <param name="privateKey">The RSA private key of a registered GitHub app installed in the specified repository.</param>
        /// <returns>An authenticated GitHub client.</returns>
        internal static GitHubClient GetGitHubClient(ApplicationConfig appConfig, string privateKey)
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
                string token = GitHubAuthService.GetGithubAppTokenAsync(appConfig, privateKey)
                                .GetAwaiter().GetResult();

                return new GitHubClient(new ProductHeaderValue(appConfig.GitHubAppName))
                {
                    Credentials = new Credentials(token, AuthenticationType.Bearer)
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
