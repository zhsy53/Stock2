using System.Diagnostics;
using NLog;
using Service.Config;
using Service.Module;
using Service.Utils;

var log = LogManager.GetCurrentClassLogger();
var watch = new Stopwatch();

//path:历史数据目录
//count:统计天数(最近交易日)
//min:最低价(当前收盘)
//max:最高价(当前收盘)
//percentage:最多超出统计区间内最低价的百分比
List<StockData> Execute(string path, int count, int min, int max, int percentage)
{
    watch.Start();

    log.Info("正在加载数据...");

    var list = StockDataLoader.Load(path).ToList();

    watch.Stop();

    log.Info("数据加载完毕,耗时: {}", watch.Elapsed.ToString(@"m\:ss\.fff"));

    watch.Restart();

    var result = list.AsParallel()
        .Where(o => o.HistoryData.Count >= count)
        .Select(o => new StockData(o.Metadata, o.HistoryData.TakeLast(count).ToList()))
        .Where(o => o.HistoryData[^1].ClosingPrice >= min * 100)
        .Where(o => o.HistoryData[^1].ClosingPrice <= max * 100)
        .Where(o => o.HistoryData[^1].ClosingPrice <= o.HistoryData.Select(e => e.ClosingPrice).Min() * (1 + percentage / 100.0))
        // .Select(o => (o.Metadata, MovingAverage: MovingAverage.GenerateFromList(o.HistoryData.Select(e => e.ClosingPrice).ToList()))) //均值曲线
        // .Where(o => o.MovingAverage.Curve.Min() > -70) //最长低谷期(均值下方):70天
        // .Where(o => o.MovingAverage.Curve[^1] < -10) //已经在均值下方10天以上
        .ToList();

    watch.Stop();

    log.Info("分析完毕,耗时: {},共有待选结果: [{}]", watch.Elapsed.ToString(@"m\:ss\.fff"), result.Count);

    return result;
}

static string IntToDecimal(int price)
{
    return Math.Round(Convert.ToDecimal(price / 100.0), 2).ToString().PadRight(10)[..10];
}

//解析参数
var path = StockConstant.HistoryPath;
var min = 10;
var max = 100;
var count = 500;
var percentage = 50;
log.Info("请将所有历史数据存放在指定目录,默认目录为: {0}", path);
if (args.Length >= 1)
{
    path = args[0];
}

if (args.Length >= 2)
{
    count = int.Parse(args[1]);
}

if (args.Length >= 3)
{
    min = int.Parse(args[2]);
}

if (args.Length >= 4)
{
    max = int.Parse(args[3]);
}

if (args.Length >= 5)
{
    percentage = int.Parse(args[4]);
}

log.Info("当前指定的数据目录为: {0}", path);
log.Info("当前统计的交易日为: [{}]天", count);
log.Info("当前过滤的价格区间(最近收盘价)为: [{},{}]", min, max);
log.Info("过滤当前价格不超出最低价的: [{}%]", percentage);

var list = Execute(path, count, min, max, percentage);

log.Info(new string('=', 100));
log.Info(new string('=', 100));
log.Info(new string('=', 100));

log.Info(
    "{0} | {1} | {2} | {3} | {4}", //TODO 避免nlog string 引号?
    "code".PadRight(8)[..8],
    "name".PadRight(10)[..10],
    "min".PadRight(10)[..10],
    "max".PadRight(10)[..10],
    "current".PadRight(10)[..10]
);
foreach (var o in list)
{
    log.Info(
        "{0} | {1} | {2} | {3} | {4}",
        o.Metadata.Code.PadRight(8)[..8],
        o.Metadata.Name.PadRight(10)[..10],
        IntToDecimal(o.HistoryData.Select(e => e.ClosingPrice).Min()),
        IntToDecimal(o.HistoryData.Select(e => e.ClosingPrice).Max()),
        IntToDecimal(o.HistoryData[^1].ClosingPrice)
    );
    // Console.WriteLine($"{o.Metadata.Code,-8} | {o.Metadata.Name,-10} | {Math.Round(Convert.ToDecimal(o.MovingAverage.Avg / 100.0), 2),-10} | {string.Join(",", o.MovingAverage.Curve)}");
}