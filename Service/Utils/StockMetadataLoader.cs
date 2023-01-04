using NPOI.SS.UserModel;
using Service.Module;
using CFG = Service.Config.StockConstant;

namespace Service.Utils;

public static class StockMetadataLoader
{
    public static IEnumerable<StockMetadata> Load()
    {
        return LoadShangHai().Concat(LoadShenZhen());
    }

    private static IEnumerable<StockMetadata> LoadShangHai()
    {
        return LoadFromExcel(
            CFG.ShangHaiMetadataFilename,
            row => new StockMetadata(CFG.ShangHaiPrefix + row.GetCell(0).StringCellValue, row.GetCell(3).StringCellValue)
        );
    }

    private static IEnumerable<StockMetadata> LoadShenZhen()
    {
        return LoadFromExcel(
            CFG.ShenZhenMetadataFilename,
            row => new StockMetadata(CFG.ShenZhenPrefix + row.GetCell(4).StringCellValue, row.GetCell(5).StringCellValue)
        );
    }

    private static IEnumerable<StockMetadata> LoadFromExcel(string file, Func<IRow, StockMetadata> extractor)
    {
        using var fs = new FileStream(CFG.MetadataExcelPath + file, FileMode.Open, FileAccess.Read);

        using var workbook = WorkbookFactory.Create(fs);

        var sheet = workbook.GetSheetAt(0);

        for (var i = 1; i < sheet.LastRowNum; i++)
        {
            var row = sheet.GetRow(i);
            yield return extractor.Invoke(row);
        }
    }
}