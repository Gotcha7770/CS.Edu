using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests;

[TestFixture]
public class ProcessExecutionTests
{
    public record ProcessResult(int ExitCode, IList<string> Output, IList<string> Error);

    [Test]
    public void Simple()
    {
        Process process = new Process
        {
            StartInfo =
            {
                FileName = "cmd.exe",
                Arguments = "/c dir",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            }
        };
        process.Start();
        process.WaitForExit();

        string result = process.StandardOutput.ReadToEnd();

        Assert.IsNotNull(result);
        Assert.IsNotEmpty(result);
    }

    [Test]
    public void ExecuteProcess()
    {
        var result = ExecuteProcess("cmd.exe", "/c dir");

        Assert.IsNotNull(result);
        Assert.IsNotEmpty(result.Output);
    }

    public static  ProcessResult ExecuteProcess(string name, string arguments)
    {
        return ExecuteProcess1(new ProcessStartInfo
        {
            FileName = name,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        });
    }

    public static ProcessResult ExecuteProcess1(ProcessStartInfo startInfo)
    {
        var process = Process.Start(startInfo);

        var output = new List<string>();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is not null)
            {
                output.Add(e.Data);
            }
        };

        var error = new List<string>();

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is not null)
            {
                error.Add(e.Data);
            }
        };

        //Reads the output stream first and then waits because deadlocks are possible
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();

        return new ProcessResult(process.ExitCode, output, error);
    }

    public static ProcessResult ExecuteProcess2(ProcessStartInfo startInfo)
    {
        var process = Process.Start(startInfo);

        string[] output = ConsumeReader(process.StandardOutput).ToArray();
        string[] error = ConsumeReader(process.StandardError).ToArray();

        process.WaitForExit();

        return new ProcessResult(process.ExitCode, output, error);
    }

    private static IEnumerable<string> ConsumeReader(StreamReader reader)
    {
        string text;
        while ((text = reader.ReadLine()) != null)
        {
            yield return text;
        }
    }
}