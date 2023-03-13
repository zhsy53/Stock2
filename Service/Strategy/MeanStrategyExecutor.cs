using System.Diagnostics;
using NLog;
using Service.Module;
using Service.Utils;

namespace Service.Strategy;

public static class MeanStrategyExecutor
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private static readonly Dictionary<DateOnly, IEnumerable<StockData>> DataCaches = new();

    private static IEnumerable<StockData> LoadData(string path, bool cache = false)
    {
        if (!cache) return StockDataLoader.Load(path).ToList();

        var date = DateOnly.FromDateTime(DateTime.Now);

        if (DataCaches.TryGetValue(date, out var value)) return value.ToList();

        DataCaches.Clear();
        return DataCaches[date] = StockDataLoader.Load(path).ToList();
    }

    public static List<StockData> Execute(
        string path,
        bool cache,
        MeanStrategyParameters parameters
    )
    {
        var watch = new Stopwatch();

        watch.Start();

        Log.Info("数据加载中...");

        var list = LoadData(path, cache);

        watch.Stop();

        Log.Info("数据加载完毕,耗时: [{}] 秒", watch.Elapsed.Seconds);

        watch.Restart();

        var result = list.ToList()
            .AsParallel()
            .Where(o => o.HistoryData.Count >= parameters.Count)
            .Select(o => o with { HistoryData = o.HistoryData.TakeLast(parameters.Count).ToList() })
            .Where(o => o.HistoryData[^1].ClosingPrice >= parameters.Min * 100)
            .Where(o => o.HistoryData[^1].ClosingPrice <= parameters.Max * 100)
            .Where(o => o.HistoryData[^1].ClosingPrice <= o.HistoryData.Select(e => e.ClosingPrice).Min() * (1 + parameters.Percentage / 100.0))
            // .Select(o => (o.Metadata, MovingAverage: MovingAverage.GenerateFromList(o.HistoryData.Select(e => e.ClosingPrice).ToList()))) //均值曲线
            // .Where(o => o.MovingAverage.Curve.Min() > -70) //最长低谷期(均值下方):70天
            // .Where(o => o.MovingAverage.Curve[^1] < -10) //已经在均值下方10天以上
            .ToList();

        watch.Stop();

        Log.Info("分析完毕,耗时: [{}] 秒,共有待选结果: [{}] 条", watch.Elapsed.Seconds, result.Count);

        return result;
    }
}