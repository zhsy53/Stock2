using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Text;
using NLog;
using Service.Module;

namespace Web.Utils;

public static class HttpServer
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static async Task Run(HttpListener listener, StockCallback callback)
    {
        var run = true;

        while (run)
        {
            var ctx = await listener.GetContextAsync();

            var req = ctx.Request;
            var resp = ctx.Response;

            Log.Info("url: {}, method: {}, hostname: {}, agent: {}", req.Url, req.HttpMethod, req.UserHostName, req.UserAgent);

            if (req.Url?.AbsolutePath == "/shutdown")
            {
                Log.Warn("shutdown");
                run = false;
            }

            if (!run) return;

            var parameters = ParseParameters(req.QueryString);
            Log.Info("parameters: {0}", parameters);

            var result = callback(parameters);

            resp.ContentType = "application/text";
            resp.ContentEncoding = Encoding.UTF8;

            await resp.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(SerializeStockDataList(result)));
            resp.Close();
        }
    }

    private static MeanStrategyParameters ParseParameters(NameValueCollection collection)
    {
        var o = new MeanStrategyParameters();

        foreach (string key in collection)
        {
            // var pi = o.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
            // if (pi != null) pi.SetValue(o, collection[key], null);
            var values = collection.GetValues(key);
            if (values == null || values.Length == 0) continue;

            switch (key.ToLower())
            {
                case "path":
                    o.Path = values[0];
                    break;
                case "count":
                    o.Count = int.Parse(values[0]);
                    break;
                case "min":
                    o.Min = int.Parse(values[0]);
                    break;
                case "max":
                    o.Max = int.Parse(values[0]);
                    break;
                case "percentage":
                    o.Percentage = int.Parse(values[0]);
                    break;
            }
        }

        return o;
    }

    private static string SerializeStockData(StockData o)
    {
        return $"{o.Metadata.Code.PadRight(8)[..8]}\t" +
               $"{o.Metadata.Name.PadRight(10)[..10]}\t" +
               $"{IntToDecimal(o.HistoryData.Select(e => e.ClosingPrice).Min())}\t" +
               $"{IntToDecimal(o.HistoryData.Select(e => e.ClosingPrice).Max())}\t" +
               $"{IntToDecimal(o.HistoryData[^1].ClosingPrice)}";
    }

    private static string SerializeStockDataList(IEnumerable<StockData> list)
    {
        return string.Join("\n", list.Select(SerializeStockData).ToList());
    }

    private static string IntToDecimal(int price)
    {
        return Math.Round(Convert.ToDecimal(price / 100.0), 2).ToString(CultureInfo.CurrentCulture).PadRight(10)[..10];
    }
}

public delegate List<StockData> StockCallback(MeanStrategyParameters parameters);