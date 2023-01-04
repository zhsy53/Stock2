using Service.Module;
using Service.Utils;

namespace Service.Parser;

public static class StockElementParser
{
    public static StockHistoryData ParseToStockData(IEnumerable<byte> bs)
    {
        return new StockHistoryData(bs.Group(32).Select(ParseToStockElement).ToList());
    }

    private static StockElement ParseToStockElement(IEnumerable<byte> bs)
    {
        var grouped = bs.Group(4).ToList();

        return new StockElement(
            BitConverter.ToInt32(grouped[0]),
            BitConverter.ToInt32(grouped[1]),
            BitConverter.ToInt32(grouped[4]),
            BitConverter.ToInt32(grouped[2]),
            BitConverter.ToInt32(grouped[3]),
            BitConverter.ToSingle(grouped[5]),
            BitConverter.ToInt32(grouped[6])
        );
    }
}