// ------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------------------------------------------------------------------------------

using Octokit;
using System;
using System.Threading.Tasks;
using GitHubContentUtility.Common;
using GitHubContentUtility.Services;

namespace GitHubContentUtility.Operations
{
    /// <summary>
    /// Provides method for writing to a blob in a GitHub repository.
    /// </summary>
    public class PullRequestCreator
    {
        /// <summary>
        /// Creates a Pull Request.
        /// </summary>
        /// <param name="appConfig">The application configuration object which contains values
        /// for connecting to the specified GitHub repository.</param>
        /// <param name="privateKey">The RSA private key of a registered GitHub app installed in the specified repository.</param>
        /// <returns>A task.</returns>
        public async Task CreatePullRequestAsync(ApplicationConfig appConfig, string privateKey)
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
                var gitHubClient = GitHubClientFactory.GetGitHubClient(appConfig, privateKey);

                // Create a PR
                // NB: If the PR already exists, this call will just throw an exception
                var pullRequest =
                    await gitHubClient.Repository.PullRequest.Create(appConfig.GitHubOrganization,
                        appConfig.GitHubRepoName,
                        new NewPullRequest(appConfig.PullRequestTitle,
                            appConfig.WorkingBranch,
                            appConfig.ReferenceBranch)
                        { Body = appConfig.PullRequestBody });

                // Add PR reviewers
                if (appConfig.Reviewers != null)
                {
                    var reviewersResult = await gitHubClient.Repository.PullRequest.ReviewRequest.Create(appConfig.GitHubOrganization,
                    appConfig.GitHubRepoName,
                    pullRequest.Number,
                    new PullRequestReviewRequest(appConfig.Reviewers.AsReadOnly(), null));
                }

                var issueUpdate = new IssueUpdate();

                // Add PR assignee
                appConfig.PullRequestAssignees?.ForEach(assignee => issueUpdate.AddAssignee(assignee));

                // Add PR label
                appConfig.PullRequestLabels?.ForEach(label => issueUpdate.AddLabel(label));

                // Update the PR with the relevant info.
            if ((issueUpdate.Assignees != null && issueUpdate.Assignees.Any()) ||
                issueUpdate.Labels != null && issueUpdate.Labels.Any())
                {
                    await gitHubClient.Issue.Update(appConfig.GitHubOrganization,
                        appConfig.GitHubRepoName,
                        pullRequest.Number,
                        issueUpdate);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
