namespace Service.Config;

public static class StockConstant
{
    public const string FileSuffix = ".day";

    public const string ShangHaiPrefix = "sh";
    public const string ShenZhenPrefix = "sz";

    public const string ShangHaiMetadataFilename = @"sh.xls";
    public const string ShenZhenMetadataFilename = @"sz.xlsx";

    public readonly static string[] AllowCodePrefix =
    {
        // 沪A
        "sh600", "sh601", "sh603", "sh605",
        // 深A
        "sz000", "sz001", "sz002", "sz003",
        // 科创 (20%)
        "sh688",
        // 创业 (20%)
        "sz300"
    };

    public readonly static string MetadataExcelPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\metadata\";

    public readonly static string HistoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\history\";
}