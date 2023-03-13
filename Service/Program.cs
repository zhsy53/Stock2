using System.Globalization;
using NLog;
using Service.Config;
using Service.Module;
using Service.Strategy;

var log = LogManager.GetCurrentClassLogger();

static string IntToDecimal(int price)
{
    return Math.Round(Convert.ToDecimal(price / 100.0), 2)
        .ToString(CultureInfo.CurrentCulture)
        .PadRight(10)[..10];
}

//解析参数
var parameters = new MeanStrategyParameters();

log.Info("请将所有历史数据存放在指定目录,默认目录为: {0}", StockConstant.HistoryPath);

var path = args.Length >= 1 ? args[0] : StockConstant.HistoryPath;

if (args.Length >= 2) parameters.Count = int.Parse(args[1]);

if (args.Length >= 3) parameters.Min = int.Parse(args[2]);

if (args.Length >= 4) parameters.Max = int.Parse(args[3]);

if (args.Length >= 5) parameters.Percentage = int.Parse(args[4]);

log.Info("当前指定的数据目录为: {0}", path);
log.Info("当前统计的交易日为: [{}]天", parameters.Count);
log.Info("当前过滤的价格区间(最近收盘价)为: [{},{}]", parameters.Min, parameters.Max);
log.Info("过滤当前价格不超出最低价的: [{}%]", parameters.Percentage);

var list = MeanStrategyExecutor.Execute(path, false, parameters);

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
    log.Info(
        "{0} | {1} | {2} | {3} | {4}",
        o.Metadata.Code.PadRight(8)[..8],
        o.Metadata.Name.PadRight(10)[..10],
        IntToDecimal(o.HistoryData.Select(e => e.ClosingPrice).Min()),
        IntToDecimal(o.HistoryData.Select(e => e.ClosingPrice).Max()),
        IntToDecimal(o.HistoryData[^1].ClosingPrice)
    );
// Console.WriteLine($"{o.Metadata.Code,-8} | {o.Metadata.Name,-10} | {Math.Round(Convert.ToDecimal(o.MovingAverage.Avg / 100.0), 2),-10} | {string.Join(",", o.MovingAverage.Curve)}");