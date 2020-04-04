using System;
using System.IO;
using RE.Common;
using static SimpleExec.Command;

namespace RE.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var exePath = Path.Combine(Environment.CurrentDirectory, "redis", "redis-server.exe");

            Run(exePath, $"--port {AppConfig.Port}");
        }
    }
}
