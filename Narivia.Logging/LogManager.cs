using System;
using System.Diagnostics;
using System.IO;

namespace Narivia.Logging
{
    /// <summary>
    /// Log Manager.
    /// </summary>
    public class LogManager
    {
        static volatile LogManager instance;
        static object syncRoot = new object();

        StreamWriter writer;
        TimeSpan elapsedGameTime;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static LogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new LogManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the verbosity level.
        /// </summary>
        /// <value>The verbosity level.</value>
        public int VerbosityLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether logging is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the format of the timestamp.
        /// </summary>
        /// <value>The format of the timestamp.</value>
        public string TimestampFormat { get; set; }

        /// <summary>
        /// Gets or sets the logs directory path.
        /// </summary>
        /// <value>The path to the logs directory.</value>
        public string LogsDirectory { get; set; }

        /// <summary>
        /// Gets the name of the log file.
        /// </summary>
        /// <value>The log file name.</value>
        public string LogName => $"Log.{DateTime.Now.ToString("yyy-MM-dd")}.log";

        /// <summary>
        /// Gets the path to the log file.
        /// </summary>
        /// <value>The log file path.</value>
        public string LogPath => Path.Combine(LogsDirectory, LogName);

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            // TODO: What if the game keeps running into the next day?
            writer = new StreamWriter(LogPath, true);
            writer.AutoFlush = true;

            VerbosityLevel = 1;
            TimestampFormat = "yyyy/MM/dd HH:mm:ss.ffffzzz";

            elapsedGameTime = new TimeSpan();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="elapsedGameTime">Elapsed game time.</param>
        public void Update(TimeSpan elapsedGameTime)
        {
            this.elapsedGameTime = elapsedGameTime;
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="message">Text.</param>
        public void Error(string message)
        {
            WriteLine($"ERROR|{message}");
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="message">Text.</param>
        /// <param name="ex">Exception.</param>
        public void Error(string message, Exception ex)
        {
            WriteLine($"ERROR|{message}", ex);
        }

        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="message">Text.</param>
        public void Info(string message)
        {
            WriteLine($"INFO|{message}");
        }

        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="message">Text.</param>
        /// <param name="ex">Exception.</param>
        public void Info(string message, Exception ex)
        {
            WriteLine($"INFO|{message}", ex);
        }

        /// <summary>
        /// Writes the information.
        /// </summary>
        /// <param name="message">Text.</param>
        public void Warn(string message)
        {
            WriteLine($"Info|{message}");
        }

        /// <summary>
        /// Writes the information.
        /// </summary>
        /// <param name="message">Text.</param>
        /// <param name="ex">Exception.</param>
        public void Warn(string message, Exception ex)
        {
            WriteLine($"Info|{message}", ex);
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="message">Text.</param>
        void WriteLine(string message)
        {
            string logEntry = $"{DateTime.Now.ToString(TimestampFormat)}|{elapsedGameTime.TotalMilliseconds}|{message}|";

            writer.WriteLine(logEntry);

            Debug.WriteLine(logEntry);
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="message">Text.</param>
        /// <param name="ex">Exception.</param>
        void WriteLine(string message, Exception ex)
        {
            string processedStackTrace = ex.StackTrace.Replace(Environment.NewLine, "$");
            string logEntry = $"{DateTime.Now.ToString(TimestampFormat)}|{elapsedGameTime.TotalMilliseconds}|{message}|{processedStackTrace}";

            writer.WriteLine(logEntry);

            Debug.WriteLine(logEntry);
        }
    }
}
