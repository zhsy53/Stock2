namespace Service.Module;

public readonly record struct StockElement(
    // 日期
    int Date,
    // 开盘价
    int OpeningPrice,
    // 收盘价
    int ClosingPrice,
    // 最高价
    int HighestPrice,
    // 最低价
    int LowestPrice,
    // 成交额
    float Turnover,
    // 成交量
    int Volume
)
{
    public override string ToString()
    {
        return $"日期:{Date}\t开盘价:{OpeningPrice}\t收盘价:{ClosingPrice}\t最高价:{HighestPrice}\t最低价:{LowestPrice}";
    }
}

public readonly record struct StockHistoryData(List<StockElement> List);

public readonly record struct StockData(StockMetadata Metadata, List<StockElement> HistoryData);