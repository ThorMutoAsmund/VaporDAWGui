using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VaporDAWGui
{
    public static class Env
    {
        public static Config Conf { get; private set; } = new Config();
        public static Project Project { get; private set; } = new Project();
        public static Watchers Watchers { get; private set; } = new Watchers();
        public static Composer Composer { get; set; } 
        public static MainWindow MainWindow { get; set; }

        public static void StartUp()
        {
            Conf.StartUp();
            Project.StartUp();
            Watchers.StartUp();
        }

        private static void WriteTrace(string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            Trace.WriteLine("member name: " + memberName);
            Trace.WriteLine("source file path: " + sourceFilePath);
            Trace.WriteLine("source line number: " + sourceLineNumber);
        }

        public static void LogError(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.WriteLine("ERROR message: " + message);
            WriteTrace(memberName, sourceFilePath, sourceLineNumber);
        }
        public static void LogException(Exception ex, 
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace.WriteLine("EXCEPTION message: " + ex.Message);
            WriteTrace(memberName, sourceFilePath, sourceLineNumber);
        }
    }
}
