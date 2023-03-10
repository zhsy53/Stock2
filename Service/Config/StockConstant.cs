namespace Service.Config;

public static class StockConstant
{
    public const string FileSuffix = ".day";

    public const string ShangHaiPrefix = "sh";
    public const string ShenZhenPrefix = "sz";

    // public const string ShangHaiMetadataFilename = @"sh.xls";
    // public const string ShenZhenMetadataFilename = @"sz.xlsx";

    public static readonly string[] AllowCodePrefix =
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

    private static readonly string Home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private static readonly char Sep = Path.DirectorySeparatorChar;

    // public static readonly string MetadataExcelPath = HOME + $"{SEP}Downloads{SEP}metadata{SEP}";

    public static readonly string HistoryPath = Home + $"{Sep}Downloads{Sep}history{Sep}";
}