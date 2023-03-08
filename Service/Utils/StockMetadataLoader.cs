using Service.Module;

namespace Service.Utils;

public interface StockMetadataLoader
{
    IEnumerable<StockMetadata> Load();
}