using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rogue
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Init();
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            Cef.Shutdown();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Cef.Shutdown();
        }

        static void Init()
        {
            var settings = new CefSettings();
            settings.LogFile = @"F:\Git\CefSharpTester\CefSharpTester\Log\log.log";
            settings.CachePath = @"F:\Git\CefSharpTester\CefSharpTester\Cache";
            Cef.EnableHighDPISupport();
            Cef.Initialize(settings);
        }
    }
}
