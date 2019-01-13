using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ForecastMonitor.Test.UI.TestUtils
{
    public static class ProcessManager
    {
        private class ProcessWrapper
        {
            public int Pid { get; set; }
            public int Port { get; set; }
            public string Protocol { get; set; }
        }


        public static void ExecuteCommand(params string[] args)
        {
            var arguments = new StringBuilder();
            var argumentSeparator = " && ";
            for (var i = 0; i < args.Length; i++)
            {
                arguments.Append(args[i]);

                if (i < args.Length - 1)
                {
                    arguments.Append(argumentSeparator);
                }
            }

            var process = new Process
            {
                StartInfo =
                {
                    FileName = @"cmd.exe",
                    Arguments = $@"{arguments}"
                }
            };

            process.Start();
        }

        /// <summary>
        /// Kills process listening to a given port
        /// </summary>
        /// <param name="port"></param>
        public static void KillProcessByPort(int port)
        {
            var processes = GetAllProcesses();
            if (processes.Any(p => p.Port == port))
                try
                {
                    Process.GetProcessById(processes.First(p => p.Port == port).Pid).Kill();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            else
            {
                Console.WriteLine("No process to kill!");
            }
        }

        private static List<ProcessWrapper> GetAllProcesses()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "netstat.exe",
                    Arguments = "-a -n -o",
                    WindowStyle = ProcessWindowStyle.Maximized,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            process.Start();

            var output = process.StandardOutput.ReadToEnd();

            if (process.ExitCode != 0)
            {
                throw new Exception("Unexpected error!");
            }

            var result = new List<ProcessWrapper>();

            var lines = Regex.Split(output, "\r\n");
            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("Proto"))
                    continue;

                var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var len = parts.Length;
                if (len > 2)
                    result.Add(new ProcessWrapper
                    {
                        Protocol = parts[0],
                        Port = int.Parse(parts[1].Split(':').Last()),
                        Pid = int.Parse(parts[len - 1])
                    });


            }
            return result;
        }
    }
}
