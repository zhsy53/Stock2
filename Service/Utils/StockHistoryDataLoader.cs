using Service.Config;
using Service.Module;
using Service.Parser;

namespace Service.Utils;

public static class StockHistoryDataLoader
{
    /**
     * 读取目录下所有*.day文件(deep不限)
     */
    public static Dictionary<string, StockHistoryData> Load()
    {
        return Directory.GetFiles(StockConstant.HistoryPath, "*" + StockConstant.FileSuffix, SearchOption.AllDirectories)
            .Where(o => StockConstant.AllowCodePrefix.Contains(o[^12..^7]))
            .AsParallel()
            // .WithDegreeOfParallelism(64)
            .ToDictionary(ExtractStockCodeFromFilename, ParserFromFile);
    }

    private static StockHistoryData ParserFromFile(string file)
    {
        return StockElementParser.ParseToStockData(File.ReadAllBytes(file));
    }

    /**
     * sh|sz:2
     * code:6
     * .day:4
     */
    private static string ExtractStockCodeFromFilename(string file)
    {
        return file[^12..^4];
    }
}