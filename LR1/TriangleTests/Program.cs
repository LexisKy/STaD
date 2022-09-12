using System;
using System.Diagnostics;
using System.IO;

namespace TriangleTest
{
    public class Program
    {
        private static readonly string pathToExecutableFile = @"D:\Учёба\PS_3\PS_3_1\ТиОПО\LR1\Triangle\bin\Debug\net6.0\";
        private static readonly string pathToCheckResults = @"..\..\..\ResultOfCheck.txt";
        private static bool CheckProgramResponse(string inputValues, string example)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo(pathToExecutableFile + "Triangle.exe");
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.Arguments = inputValues;

            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            string result = proc.StandardOutput.ReadToEnd().Trim();
            return result == example;
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            try
            {
                string input = args[0];
                using (StreamReader streamInput = new StreamReader(input))
                {
                    using (StreamWriter streamOutput = new StreamWriter(pathToCheckResults))
                    {
                        for (int count = 1; !streamInput.EndOfStream; count++)
                        {
                            string inputValues = streamInput.ReadLine();
                            string example = streamInput.ReadLine();
                            if (CheckProgramResponse(inputValues, example))
                            {
                                streamOutput.WriteLine($"{count} success");
                            }
                            else
                            {
                                streamOutput.WriteLine($"{count} error");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}