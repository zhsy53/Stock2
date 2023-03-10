using Service.Config;

namespace Service.Module;

public class MeanStrategyParameters
{
    //历史数据目录
    public string Path { get; set; } = StockConstant.HistoryPath;

    //统计天数(最近交易日)
    public int Count { get; set; } = 500;

    //最低价(当前收盘)
    public int Min { get; set; } = 10;

    //最高价(当前收盘)
    public int Max { get; set; } = 50;

    //最多超出统计区间内最低价的百分比
    public int Percentage { get; set; } = 15;

    public override string ToString()
    {
        return $"path: {Path}, count:{Count}, range:[{Min},{Max}], percentage:[{Percentage}%]";
    }
}