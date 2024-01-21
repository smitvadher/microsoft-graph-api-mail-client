namespace GraphApiMailClientApp.Library.Exceptions
{
    /// <summary>
    /// Exception thrown for configuration-related issues.
    /// </summary>
    [Serializable]
    public class ConfigException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigException"/> class.
        /// </summary>
        /// <param name="configErrorMessages">The collection of configuration error messages.</param>
        public ConfigException(IReadOnlyCollection<string> configErrorMessages) : base(BuildErrorMessage(configErrorMessages))
        {
            ConfigErrorMessages = configErrorMessages;
        }

        /// <summary>
        /// Gets the collection of configuration error messages.
        /// </summary>
        public IReadOnlyCollection<string> ConfigErrorMessages { get; }

        private static string BuildErrorMessage(IReadOnlyCollection<string> configErrorMessages)
        {
            return string.Join(Environment.NewLine, configErrorMessages ?? Array.Empty<string>());
        }
    }
}
