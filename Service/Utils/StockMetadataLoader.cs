using Service.Module;

namespace Service.Utils;

public interface IStockMetadataLoader
{
    IEnumerable<StockMetadata> Load();
}