// ------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace GitHubContentUtility.Common
{
    /// <summary>
    /// Specifies the common property values required to connect to a specific GitHub repository.
    /// </summary>
    public class ApplicationConfig
    {
        /// <summary>
        /// The name of the GitHub app
        /// </summary>
        public string GitHubAppName { get; set; }

        /// <summary>
        /// The owner of the GitHub repository
        /// </summary>
        public string GitHubOrganization { get; set; }

        /// <summary>
        /// The name of the GitHub repository
        /// </summary>
        public string GitHubRepoName { get; set; }

        /// <summary>
        /// The GitHub App Id
        /// </summary>
        public int GitHubAppId { get; set; }

        /// <summary>
        /// The remote branch where commits are made into
        /// </summary>
        public string WorkingBranch { get; set; }

        /// <summary>
        /// The remote branch which is the base reference of the <see cref="WorkingBranch"/>
        /// </summary>
        public string ReferenceBranch { get; set; }
        
        /// <summary>
        /// The paths of file contents in a repository branch
        /// </summary>
        public Dictionary<string, string> FileContentPaths { get; set; }
        
        /// <summary>
        /// The string contents of files in a respository branch
        /// </summary>
        public Dictionary<string, string> FileContents { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The commit message
        /// </summary>
        public string CommitMessage { get; set; }

        /// <summary>
        /// Pull request title
        /// </summary>
        public string PullRequestTitle { get; set; }

        /// <summary>
        /// Pull request message
        /// </summary>
        public string PullRequestBody { get; set; }

        /// <summary>
        /// List of pull request reviewers
        /// </summary>
        public List<string> Reviewers { get; set; }

        /// <summary>
        /// Pull request assignee
        /// </summary>
        public List<string> PullRequestAssignees { get; set; }

        /// <summary>
        /// Pull request label
        /// </summary>
        public List<string> PullRequestLabels { get; set; }

        /// <summary>
        /// The mode of the file being written to the repository
        /// </summary>
        public Enums.TreeItemMode TreeItemMode { get; set; } = Enums.TreeItemMode.Blob;
    }
}
