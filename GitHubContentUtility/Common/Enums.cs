// ------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------------------------------------------------------------------------------

namespace GitHubContentUtility.Common
{
    /// <summary>
    /// Provides a definition of commonly used enumerations.
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// Enumeration of the supported file modes.
        /// </summary>
        public enum TreeItemMode
        {
            Blob = 100644,
            Executable_Blob = 100755,
            Subdirectory_Tree = 040000,
            Submodule_Commit = 160000,
            Symlink_Blob = 120000
        }
    }
}
