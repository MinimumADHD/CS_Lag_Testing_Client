using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int key);

    static void Main()
    {
        bool releaseRenewFlag = false;

        Console.WriteLine("Press F8 to toggle ipconfig /release and /renew.", Console.ForegroundColor = ConsoleColor.DarkBlue);
        Console.WriteLine("\n", Console.ForegroundColor = ConsoleColor.White);

        while (true)
        {
            // Check if F8 key is pressed
            if (GetAsyncKeyState((int)ConsoleKey.F8) < 0)
            {
                // Toggle the flag
                releaseRenewFlag = !releaseRenewFlag;

                // Execute ipconfig /release or /renew based on the flag
                if (releaseRenewFlag)
                {
                    ExecuteCommand("ipconfig /release");
                    Console.WriteLine("ipconfig /release executed.", Console.ForegroundColor = ConsoleColor.Magenta);
                }
                else
                {
                    ExecuteCommand("ipconfig /renew");
                    Console.WriteLine("ipconfig /renew executed.", Console.ForegroundColor = ConsoleColor.Magenta);
                }
                Console.WriteLine("\n", Console.ForegroundColor = ConsoleColor.White);

                // Wait for the key release to avoid multiple executions
                while (GetAsyncKeyState((int)ConsoleKey.F8) < 0) { }
            }
        }
    }

    static void ExecuteCommand(string command)
    {
        // this code hides (by default) the terminal logs, which is NOT what I need
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            RedirectStandardInput = true,
            RedirectStandardOutput = false,
            CreateNoWindow = false,
            UseShellExecute = false,
        };

        Process process = new Process { StartInfo = processStartInfo };
        process.Start();

        using (var streamWriter = process.StandardInput)
        {
            if (streamWriter.BaseStream.CanWrite)
                streamWriter.WriteLine(command);
        }

        process.WaitForExit();
    }
}
