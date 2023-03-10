using System.Diagnostics;
using NLog;

namespace Web.Utils;

public static class ProcessHelper
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static void Execute(string fileName, string arguments, bool show = true)
    {
        using var p = new Process
        {
            StartInfo =
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = fileName,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        if (!string.IsNullOrEmpty(arguments)) p.StartInfo.Arguments = arguments;

        var directoryName = Path.GetDirectoryName(fileName);
        if (!string.IsNullOrEmpty(directoryName)) p.StartInfo.WorkingDirectory = directoryName;

        if (show)
        {
            p.OutputDataReceived += (_, eventArgs) => Log.Info(eventArgs.Data);
            p.ErrorDataReceived += (_, eventArgs) => Log.Warn(eventArgs.Data);
        }

        p.Start();

        if (show)
        {
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
        }

        // log.Info("output: {0}", p.StandardOutput.ReadToEnd());
        // log.Info("error: {0}", p.StandardError.ReadToEnd());

        p.WaitForExit();
    }
}