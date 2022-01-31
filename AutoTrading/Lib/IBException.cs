using System;

namespace AutoTrading.Lib
{

    /// <summary>
    /// Severity level of Exception
    /// </summary>
    public enum SeverityLevel
    {
        Fatal,
        Critical,
        Information
    }
    /// <summary>
    /// Log level of Exception
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Event
    }




    /// <summary>
    /// Summary description for CustomException ApplicationException
    /// </summary>
    public class IBException : Exception
    {
 
        // private members
        // defines the severity level of the Exception
        private SeverityLevel severityLevelOfException;
        // defines the logLevel of the Exception
        private LogLevel logLevelOfException;
        // System Exception that is thrown
        private Exception innerException;
        // Custom Message
        private string customMessage;
        /// <summary>
        /// Public accessor of customMessage
        /// </summary>
        public string CustomMessage
        {
            get { return this.customMessage; }
            set { this.customMessage = value; }
        }
        /// <summary>
        /// Standard default Constructor
        /// </summary>
        public IBException()
        { }
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="severityLevel"></param>
        /// <param name="logLevel"></param>
        /// <param name="exception"></param>
        /// <param name="customMessage"></param>
        public IBException(SeverityLevel severityLevel, LogLevel logLevel, Exception exception, string customMessage)
        {
            this.severityLevelOfException = severityLevel;
            this.logLevelOfException = logLevel;
            this.innerException = exception;
            this.customMessage = customMessage;
        }
    }
}