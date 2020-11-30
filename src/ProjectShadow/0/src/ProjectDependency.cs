using Microsoft.Build.Evaluation;

namespace Teronis.ProjectShadow
{
    public class ProjectDependency
    {
        public string AbsoluteProjectPath { get; private set; }
        public Project Project { get; internal set; } = null!;

        internal ProjectDependency(string absoluteProjectPath)
        {
            AbsoluteProjectPath = absoluteProjectPath ?? throw new System.ArgumentNullException(nameof(absoluteProjectPath));
        }

        public ProjectDependency(string absoluteProjectPath, Project project)
            : this(absoluteProjectPath)
        {
            Project = project ?? throw new System.ArgumentNullException(nameof(project));
        }
    }
}
