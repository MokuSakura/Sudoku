using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace SudokuWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
#if DEBUG
            AttachToParentConsole();
#endif
        }
#if DEBUG
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        ///     Redirects the console output of the current process to the parent process.
        /// </summary>
        /// <remarks>
        ///     Must be called before calls to <see cref="Console.WriteLine()" />.
        /// </remarks>
        public static void AttachToParentConsole()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
        }
#endif
    }
}