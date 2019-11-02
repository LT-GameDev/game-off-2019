using UnityEngine;
using System.Collections;

namespace Game.Utility
{
    public static class LoggerExtensions
    {
        public static Logger Log(this object context) => new Logger(context);
    }

    public class Logger
    {
        private object loggerFor;

        public Logger(object loggerFor)
        {
            this.loggerFor = loggerFor;
        }

        public void Debug(string message, string prefix = "")
        {
#if LOG_LEVEL_DEBUG
            UnityEngine.Debug.Log(CreateLogString(message, prefix));
#endif
        }

        public void Debug(UnityEngine.Object context, string message, string prefix = "")
        {
#if LOG_LEVEL_DEBUG
            UnityEngine.Debug.Log(CreateLogString(message, prefix), context);
#endif
        }

        public void Warning(string message, string prefix = "")
        {
#if LOG_LEVEL_DEBUG || LOG_LEVEL_WARNING
            UnityEngine.Debug.LogWarning(CreateLogString(message, prefix));
#endif
        }

        public void Warning(UnityEngine.Object context, string message, string prefix = "")
        {
#if LOG_LEVEL_DEBUG || LOG_LEVEL_WARNING
            UnityEngine.Debug.LogWarning(CreateLogString(message, prefix), context);
#endif
        }

        public void Error(string message, string prefix = "")
        {
#if LOG_LEVEL_DEBUG || LOG_LEVEL_WARNING || LOG_LEVEL_ERROR
            UnityEngine.Debug.LogError(CreateLogString(message, prefix));
#endif
        }

        public void Error(UnityEngine.Object context, string message, string prefix = "")
        {
#if LOG_LEVEL_DEBUG || LOG_LEVEL_WARNING || LOG_LEVEL_ERROR
            UnityEngine.Debug.LogError(CreateLogString(message, prefix), context);
#endif
        }

        public void Exception(System.Exception exception)
        {
#if LOG_LEVEL_DEBUG || LOG_LEVEL_WARNING || LOG_LEVEL_ERROR || LOG_LEVEL_CRITICAL
            UnityEngine.Debug.LogException(exception);
#endif
        }

        public void Exception(UnityEngine.Object context, System.Exception exception)
        {
#if LOG_LEVEL_DEBUG || LOG_LEVEL_WARNING || LOG_LEVEL_ERROR || LOG_LEVEL_CRITICAL
            UnityEngine.Debug.LogException(exception, context);
#endif
        }

        private string CreateLogString(string message, string prefix) => $"[{loggerFor.GetType().Name}]{(!string.IsNullOrWhiteSpace(prefix) ? $" [{prefix}]" : "")} {message}";
    }
}