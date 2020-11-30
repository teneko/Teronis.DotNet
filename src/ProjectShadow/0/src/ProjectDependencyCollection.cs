using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;

namespace Teronis.ProjectShadow
{
    public class ProjectDependencyCollection : ProjectDependency, ICollection<ProjectDependency>, IDisposable
    {
        public int Count => ((ICollection<ProjectDependency>)projectDependencies).Count;
        public bool IsReadOnly => ((ICollection<ProjectDependency>)projectDependencies).IsReadOnly;

        public ProjectCollection ProjectCollection { get; }

        private readonly List<ProjectDependency> projectDependencies;
        public bool Loaded { get; private set; }

        public ProjectDependencyCollection(string absoluteProjectPath)
        : base(absoluteProjectPath)
        {
            ProjectCollection = new ProjectCollection();
            projectDependencies = new List<ProjectDependency>();
        }

        public void Add(ProjectDependency item) => ((ICollection<ProjectDependency>)projectDependencies).Add(item);
        public void Clear() => ((ICollection<ProjectDependency>)projectDependencies).Clear();
        public bool Contains(ProjectDependency item) => ((ICollection<ProjectDependency>)projectDependencies).Contains(item);
        public void CopyTo(ProjectDependency[] array, int arrayIndex) => ((ICollection<ProjectDependency>)projectDependencies).CopyTo(array, arrayIndex);
        public IEnumerator<ProjectDependency> GetEnumerator() => ((IEnumerable<ProjectDependency>)projectDependencies).GetEnumerator();
        public bool Remove(ProjectDependency item) => ((ICollection<ProjectDependency>)projectDependencies).Remove(item);
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)projectDependencies).GetEnumerator();

        public void LoadProjectDependencies()
        {
            if (Loaded) {
                throw new InvalidOperationException("The project dependencies have been already loaded. Unload them first.");
            }

            IEnumerable<ProjectItem> getProjectReferencesFromProject(Project project) =>
                project.GetItems("ProjectReference");

            IEnumerable<ProjectItem> getProjectReferences(string absoluteProjectPath)
            {
                var project = ProjectCollection.LoadProject(absoluteProjectPath);
                return getProjectReferencesFromProject(project);
            }

            var initialProject = ProjectCollection.LoadProject(AbsoluteProjectPath);
            Project = initialProject;
            var initialProjectReferences = getProjectReferencesFromProject(initialProject);
            var unevaluatedProjectDepencies = new List<ProjectItem>(initialProjectReferences);

            while (unevaluatedProjectDepencies.Count > 0) {
                //var nextProjectReference = unevaluatedProjectDepencies.First
                //var nextProjectReferences = 
            }
        }

        public void Dispose() => 
            ProjectCollection.Dispose();
    }
}
