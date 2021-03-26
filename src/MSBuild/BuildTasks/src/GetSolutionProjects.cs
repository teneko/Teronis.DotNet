// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//$Id$

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace Teronis.MSBuild.BuildTasks
{
    /// <summary>
    /// Retrieves the list of Projects contained within a Visual Studio Solution (.sln) file .
    /// </summary>
    /// <example>
    /// Returns project name, GUID, and path information from test solution.
    /// <code><![CDATA[
    ///   <Target Name="Test">
    ///       <GetSolutionProjects Solution="TestSolution.sln">
    ///           <Output ItemName="ProjectFiles" TaskParameter="Output"/>
    ///       </GetSolutionProjects>
    /// 
    ///     <Message Text="Project names:" />
    ///     <Message Text="%(ProjectFiles.ProjectName)" />
    ///     <Message Text="Relative project paths:" />
    ///     <Message Text="%(ProjectFiles.ProjectPath)" />
    ///     <Message Text="Project GUIDs:" />
    ///     <Message Text="%(ProjectFiles.ProjectGUID)" />
    ///     <Message Text="Full paths to project files:" />
    ///     <Message Text="%(ProjectFiles.FullPath)" />
    ///     <Message Text="Project Type GUIDs:" />
    ///     <Message Text="%(ProjectFiles.ProjectTypeGUID)" />
    ///   </Target>
    /// ]]></code>
    /// </example>
    public class GetSolutionProjects : Task
    {
        private const string SolutionFolderProjectType = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";
        private const string ExtractProjectsFromSolutionRegex = @"Project\(""(?<ProjectTypeGUID>.+?)""\)\s*=\s*""(?<ProjectName>.+?)""\s*,\s*""(?<ProjectFile>.+?)""\s*,\s*""(?<ProjectGUID>.+?)""";
        private string solutionFile = "";
        private ITaskItem[]? output = null;

        /// <summary>
        /// A list of the project files found in <see cref="Solution" />.
        /// </summary>
        /// <remarks>
        /// The name of the project can be retrieved by reading metadata tag <c>ProjectName</c>.
        /// <para>
        /// The path to the project as it is is stored in the solution file retrieved by reading metadata tag <c>ProjectPath</c>.
        /// </para>
        /// <para>
        /// The project's GUID can be retrieved by reading metadata tag <c>ProjectGUID</c>.
        /// </para>
        /// </remarks>
        [Output]
        public ITaskItem[]? Output {
            get { return output; }
            set { output = value; }
        }

        /// <summary>
        /// Name of Solution to get the projects from.
        /// </summary>
        [Required]
        public string Solution {
            get { return solutionFile; }
            set { solutionFile = value; }
        }

        /// <summary>
        /// Performs the task.
        /// </summary>
        /// <returns>true on success</returns>
        public override bool Execute()
        {
            if (solutionFile == null || !File.Exists(solutionFile)) {
                Log.LogError("Solution \"{0}\" does not exist.", solutionFile);
                return false;
            }

            string solutionFolder = Path.GetDirectoryName(solutionFile)!;

            string solutionText = File.ReadAllText(solutionFile);
            MatchCollection matches = Regex.Matches(solutionText, ExtractProjectsFromSolutionRegex);
            List<ITaskItem> taskItems = new List<ITaskItem>();

            for (int i = 0; i < matches.Count; i++) {
                string projectPathRelativeToSolution = matches[i].Groups["ProjectFile"].Value;
                string projectPathOnDisk = Path.Combine(solutionFolder, projectPathRelativeToSolution);
                string projectFile = projectPathRelativeToSolution;
                string projectName = matches[i].Groups["ProjectName"].Value;
                string projectGUID = matches[i].Groups["ProjectGUID"].Value;
                string projectTypeGUID = matches[i].Groups["ProjectTypeGUID"].Value;

                // do not include Solution Folders in output
                if (projectTypeGUID.Equals(SolutionFolderProjectType, StringComparison.OrdinalIgnoreCase)) {
                    continue;
                }

                ITaskItem project = new TaskItem(projectPathOnDisk);
                project.SetMetadata("ProjectPath", projectFile);
                project.SetMetadata("ProjectName", projectName);
                project.SetMetadata("ProjectGUID", projectGUID);
                project.SetMetadata("ProjectTypeGUID", projectTypeGUID);
                taskItems.Add(project);
            }

            output = taskItems.ToArray();
            return true;
        }
    }
}
