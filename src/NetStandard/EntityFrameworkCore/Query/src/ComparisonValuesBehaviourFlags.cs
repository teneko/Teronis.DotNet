namespace Teronis.EntityFrameworkCore.Query
{
    /// <summary>
    /// Controls the behaviour of the collection of comparison values.
    /// </summary>
    public enum ComparisonValuesBehaviourFlags
    {
        /// <summary>
        /// If the collection of the comparison values is null or empty, 
        /// the boolean expression branch for each item won't be created.
        /// </summary>
        NullOrEmptyLeadsToSkip = 0,
        /// <summary>
        /// If the collection of the comparison values is null, the boolean 
        /// expression branch for each each item won't be created. Instead 
        /// an expression is used that leads to false.
        /// </summary>
        NullLeadsToFalse = 1,
        /// <summary>
        /// If the collection of the comparison values is empty, the boolean
        /// expression branch for each each item won't be created. Instead 
        /// an expression is used that leads to false.
        /// </summary>
        EmptyLeadsToFalse = 2,
        /// <summary>
        /// If the collection of the comparison values is null or empty,
        /// the boolean expression branch for each each item won't be
        /// created. Instead an expression is used that leads to false.
        /// </summary>
        NullOrEmptyLeadsToFalse = NullLeadsToFalse | EmptyLeadsToFalse
    }
}
