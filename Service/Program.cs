using System.Diagnostics;
using Service.Module;
using Service.Strategy;
using Service.Utils;

var stopwatch = new Stopwatch();
stopwatch.Start();

var r = StockDataLoader.Load()
    .ToList()
    .AsParallel()
    .Where(o => o.HistoryData[^1].ClosingPrice is <= 50 * 100 and >= 10 * 100) //价格区间[10,50]
    .Where(o => o.HistoryData.Count >= 500) //最少数据量:500天
    .Select(o => new StockData(o.Metadata, o.HistoryData.TakeLast(500).ToList()))
    .Select(o => (o.Metadata, MovingAverage: MovingAverage.GenerateFromList(o.HistoryData.Select(e => e.ClosingPrice).ToList()))) //均值曲线
    .Where(o => o.MovingAverage.Curve.Min() > -70) //最长低谷期(均值下方):70天
    .Where(o => o.MovingAverage.Curve[^1] < -10) //已经在均值下方10天以上
    .ToList();

stopwatch.Stop();

var timeTaken = stopwatch.Elapsed;
Console.WriteLine("费时: " + timeTaken.ToString(@"m\:ss\.fff"));

Console.WriteLine("编    码 | 名      称 | 单    价 | 曲    线");

foreach (var o in r)
{
    Console.WriteLine($"{o.Metadata.Code,-8} | {o.Metadata.Name,-10} | {Math.Round(Convert.ToDecimal(o.MovingAverage.Avg / 100.0), 2),-10} | {string.Join(",", o.MovingAverage.Curve)}");
}