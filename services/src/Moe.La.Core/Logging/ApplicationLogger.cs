using Microsoft.Extensions.Logging;

namespace Moe.La.Core.Logging
{
    /// <summary>
    /// Used to add extra logging capabilities to the application for the static types.
    /// </summary>
    public static class ApplicationLogger
    {
        private static ILogger _logger;

        /// <summary>
        /// Configure the <see cref="ApplicationLogger"/> <see cref="ILogger"/> static instance.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public static void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(ApplicationLogger));
        }

        /// <summary>
        /// Get logger.
        /// </summary>
        public static ILogger Logger => _logger;
    }
}
