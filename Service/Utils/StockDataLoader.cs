using NLog;
using Service.Module;

namespace Service.Utils;

public static class StockDataLoader
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static IEnumerable<StockData> Load(string path)
    {
        var history = StockHistoryDataLoader.Load(path);
        Log.Info("历史数据: {}", history.Count);

        var stockMetadataLoader = new StockMetadataCsvLoader();
        var metadata = stockMetadataLoader.Load().ToDictionary(o => o.Code, o => o.Name);
        Log.Info("元数据: {}", metadata.Count);

        foreach (var kv in history.Where(kv => metadata.ContainsKey(kv.Key))) yield return new StockData(new StockMetadata(kv.Key, metadata[kv.Key]), kv.Value.List);
    }
}