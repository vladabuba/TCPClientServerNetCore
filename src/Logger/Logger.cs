using System;
using System.Diagnostics;
using System.IO;

namespace Logger
{
    public class LoggerToFile : ILogger
    {
        private string logfile;
        private static bool LogToFile = true;

        private long MAX_LOG_SIZE = 10000000;

        private const string logname = "app.log";
        private const string logdirectorypath = @"logs\";
        
        public LoggerToFile() : this(logname)
        {
        }

        public LoggerToFile(string logfilename)
        {
            logfile = FixPathToLogFile(logfilename);
        }

        public string GetPathToLogFile()
        {
            return logfile;
        }

        private string FixPathToLogFile(string pathLogFile)
        {
            if (!Path.IsPathRooted(pathLogFile))
            {
                string pathApp = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                if (Debugger.IsAttached)
                {
                    pathApp = @"C:\app\";
                }

                if(!pathApp.EndsWith(@"\"))
                {
                    pathApp += @"\";
                }
                
                if (pathLogFile.StartsWith("logs") || pathLogFile.StartsWith(@"\logs") || pathLogFile.StartsWith(@"\\logs"))
                {
                    return pathApp + pathLogFile;
                }

                if(!System.IO.Directory.Exists(pathApp + logdirectorypath))
                {
                    System.IO.Directory.CreateDirectory(pathApp + logdirectorypath);
                }

                return pathApp + logdirectorypath + pathLogFile;
            }

            return pathLogFile;
        }

        public void AddToLog(string Type, string Entry)
        {
            Add2Log(Type, Entry);
        }

        public void Add2Log(string type, string entry)
        {
            string text = DateTime.Now.ToString() + "\t<" + type + ">: \t" + entry + "\n";
            if (LogToFile)
            {
                CheckLogFileSize(logfile);
                SaveTextToFile(logfile, text);
            }
        }

        private void CheckLogFileSize(string logfile)
        {
            if (!File.Exists(logfile))
            {
                return;
            }

            FileInfo f = new FileInfo(logfile);
            if (f.Exists)
            {
                long s1 = f.Length;
                if (s1 > MAX_LOG_SIZE)
                {
                    string backupcopy = (logfile + ".old");
                    File.Copy(logfile, backupcopy, true);
                    FileInfo f1 = new FileInfo(logfile);
                    FileInfo f2 = new FileInfo(backupcopy);
                    long fl1 = f1.Length;
                    long fl2 = f2.Length;

                    if (fl1 == fl2)
                    {
                        File.Delete(logfile);
                    }
                }
            }
        }

        private bool SaveTextToFile(String filename, String text)
        {

            try
            {
                text += "\r\n";
                File.AppendAllText(filename, text);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be saved:");
                Console.WriteLine(e.Message);
            }

            return false;
        }

    }
}
