namespace Service.Config;

public static class StockConstant
{
    public const string FileSuffix = ".day";

    public const string ShangHaiPrefix = "sh";
    public const string ShenZhenPrefix = "sz";

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

    private static readonly char Sep = Path.DirectorySeparatorChar;

    public static readonly string HistoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + $"{Sep}Downloads{Sep}history{Sep}";
}