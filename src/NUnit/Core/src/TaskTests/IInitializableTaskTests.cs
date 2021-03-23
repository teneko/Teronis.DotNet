namespace Teronis.NUnit.TaskTests
{
    public interface IInitializableTaskTests
    {
        /// <summary>
        /// Initializes the instance. (Compare <see cref="TaskTests{TDerived}"/>).
        /// Used only in <see cref="TaskTestsAnnotatedClasses"/> when assigning instances.
        /// </summary>
        public void Initialize();
    }
}
