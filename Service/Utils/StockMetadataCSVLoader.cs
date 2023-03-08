using System.Globalization;
using System.Text;
using CsvHelper;
using NLog;
using Service.Module;
using CFG = Service.Config.StockConstant;

namespace Service.Utils;

public class StockMetadataCSVLoader : StockMetadataLoader
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    public IEnumerable<StockMetadata> Load()
    {
        return LoadShangHai().Concat(LoadShenZhen());
    }

    private static IEnumerable<StockMetadata> LoadShangHai()
    {
        var path = Path.Combine(System.AppContext.BaseDirectory, $"Resources{Path.DirectorySeparatorChar}sh.csv");
        return LoadFromCsvFile(path, 0, 2, CFG.ShangHaiPrefix);
    }

    private static IEnumerable<StockMetadata> LoadShenZhen()
    {
        var path = Path.Combine(AppContext.BaseDirectory, $"Resources{Path.DirectorySeparatorChar}sz.csv");
        return LoadFromCsvFile(path, 4, 5, CFG.ShenZhenPrefix);
    }

    //TODO
    private static IEnumerable<StockMetadata> LoadFromCsvFile(string path, int codeIndex, int nameIndex, string prefix)
    {
        using var reader = new StreamReader(path, Encoding.UTF8);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Read();
        while (csv.Read())
        {
            yield return new StockMetadata(prefix + csv.GetField<string>(codeIndex), csv.GetField<string>(nameIndex));
        }
    }
}