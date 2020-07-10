using NLog;
using NLog.Config;
using NLog.Targets;
using System.Windows.Forms;

namespace LogTest
{
    public partial class Form1 : Form
    {
        Logger logger1 = CreateCustomLogger("CustomLog1");
        Logger logger2 = CreateCustomLogger("CustomLog2");

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            logger1.Debug("App Log 1");
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            logger1.Info("App Log 2");
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            logger2.Debug("Driver Log 1");
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            logger2.Info("Driver Log 2");
        }


        /// <summary>
        /// Create Custom Logger using parameters passed.
        /// </summary>
        /// <param name="name">Name of file.</param>
        /// <param name="LogEntryLayout">Give "" if you want just message. If omited will switch to full log paramaters.</param>
        /// <param name="logFileLayout">Filename only. No extension or file paths accepted.</param>
        /// <param name="absoluteFilePath">If you want to save the log file to different path thatn application default log path, specify the path here.</param>
        /// <returns>New instance of NLog logger completly isolated from default instance if any</returns>
        public static Logger CreateCustomLogger(string name = "CustomLog",
            //string LogEntryLayout = "${ date:format=dd.MM.yyyy HH\\:mm\\:ss.fff} thread[${threadid}] ${logger} (${level:uppercase=true}): ${message}. ${exception:format=ToString}",
            string LogEntryLayout = "${ date:format=dd.MM.yyyy HH\\:mm\\:ss.fff}${level:uppercase=true}: ${message}. ${exception:format=ToString}",
            string logFileLayout = "logs/{0}.${{shortdate}}.log",
            string absoluteFilePath = "")
        {
            var factory = new LogFactory();
            var target = new FileTarget();
            target.Name = name;
            target.ReplaceFileContentsOnEachWrite = false;
            if (absoluteFilePath == "")
                target.FileName = string.Format(logFileLayout, name);
            else
                target.FileName = string.Format(absoluteFilePath + "//" + logFileLayout, name);
            if (LogEntryLayout == "") //if user specifes "" then use default layout.
                target.Layout = "${message}. ${exception:format=ToString}";
            else
                target.Layout = LogEntryLayout;
            //var defaultconfig = LogManager.Configuration;
            var config = new LoggingConfiguration();
            config.AddTarget(name, target);

            var ruleInfo = new LoggingRule("*", LogLevel.Trace, target);

            config.LoggingRules.Add(ruleInfo);
            factory.Configuration = config;

            return factory.GetCurrentClassLogger();
        }

        ///// <summary>
        ///// Create Custom Logger using a seperate configuration file.
        ///// </summary>
        ///// <param name="name">Name of file.</param>
        ///// <returns>New instance of NLog logger completly isolated from default instance if any</returns>
        //public static Logger CreateCustomLoggerFromConfig(string configname)
        //{
        //    var factory = new LogFactory(new XmlLoggingConfiguration(configname));
        //    return factory.GetCurrentClassLogger();
        //}
    }
}
