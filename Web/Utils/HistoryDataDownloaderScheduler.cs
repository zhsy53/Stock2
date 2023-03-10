using System.Diagnostics;
using NLog;

namespace Web.Utils;

public static class HistoryDataDownloaderScheduler
{
    private const string ShUrl = "https://www.tdx.com.cn/products/data/data/vipdoc/shlday.zip";
    private const string SzUrl = "https://www.tdx.com.cn/products/data/data/vipdoc/szlday.zip";
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static void Run()
    {
        const string cmd = "aria2c";
        const string shArgs = $"-s16 -x16 --dir /mnt/history {ShUrl}";
        const string szArgs = $"-s16 -x16 --dir /mnt/history {SzUrl}";

        Log.Info("正在下载历史数据");

        var watch = new Stopwatch();
        watch.Start();

        Log.Info("{0} {1}", cmd, shArgs);
        ProcessHelper.Execute(cmd, shArgs);

        Log.Info("{0} {1}", cmd, szArgs);
        ProcessHelper.Execute(cmd, szArgs);

        watch.Stop();
        Log.Info("下载完成,费时: [{0}] 秒", watch.Elapsed.Seconds);
    }
}