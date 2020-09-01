using System;
using System.IO;
using static SimpleExec.Command;

namespace RE.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var exePath = Path.Combine(Environment.CurrentDirectory, "redis", "redis-server.exe");

            Run(exePath, $"--port 1001");
        }
    }
}
