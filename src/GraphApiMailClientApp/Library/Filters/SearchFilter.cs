namespace GraphApiMailClientApp.Library.Filters
{
    public abstract class SearchFilter
    {
        #region Const

        protected const string EqComparer = "eq";
        protected const string LtComparer = "lt";
        protected const string StartsWithComparer = "startsWith({0}, '{1}')";

        #endregion

        #region Abstract properties

        /// <summary>
        /// Gets the property selector for the filter.
        /// </summary>
        protected abstract string PropertySelector { get; }

        /// <summary>
        /// Gets the comparer used in the filter condition.
        /// </summary>
        protected abstract string Comparer { get; }

        /// <summary>
        /// Gets the value associated with the filter condition.
        /// </summary>
        protected abstract string Value { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Determines whether the comparer string contains placeholder indices for property substitution.
        /// </summary>
        /// <remarks>
        /// This property checks if the comparer string contains placeholder indices such as "{0}" or "{1}".
        /// </remarks>
        private bool FormatComparer => Comparer.Contains("{0}") && Comparer.Contains("{1}");

        #endregion

        #region Methods

        /// <summary>
        /// Gets the filter condition string based on the specified property, comparer, and value.
        /// </summary>
        /// <returns>The filter condition as a string.</returns>
        protected virtual string GetCondition()
        {
            if (string.IsNullOrEmpty(Value))
                return null;

            return FormatComparer
                ? string.Format(StartsWithComparer, PropertySelector, Value)
                : $"{PropertySelector} {Comparer} {Value}";
        }

        /// <summary>
        /// Gets combined filter conditions for multiple filters in a collection.
        /// </summary>
        /// <typeparam name="T">The type of the search filters.</typeparam>
        /// <param name="filters">A collection of search filters.</param>
        /// <returns>The combined filter conditions as a string.</returns>
        public static string GetConditions<T>(IEnumerable<T> filters) where T : SearchFilter
        {
            return string.Join(" and ",
                filters.Where(f => !string.IsNullOrEmpty(f.Value))
                    .Select(f => f.GetCondition())
                    .Where(c => c != null)
                );
        }

        #endregion
    }
}
