using Service.Module;

namespace Service.Utils;

public static class StockDataLoader
{
    public static IEnumerable<StockData> Load()
    {
        var history = StockHistoryDataLoader.Load();
        Console.WriteLine($"统计历史数据的股票数: {history.Count}");
        var metadata = StockMetadataLoader.Load().ToDictionary(o => o.Code, o => o.Name);
        Console.WriteLine($"元数据包含的股票数: {metadata.Count}");

        foreach (var kv in history.Where(kv => metadata.ContainsKey(kv.Key)))
        {
            yield return new StockData(new StockMetadata(kv.Key, metadata[kv.Key]), kv.Value.List);
        }
    }
}