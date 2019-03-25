using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MoviesProject
{
    class Program
    {

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        static void Main(string[] args)
        {

            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3

            var app = new App("movies.json");
            app.Run();
        }
    }
}

