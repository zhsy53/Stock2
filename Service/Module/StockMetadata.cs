namespace Service.Module;

/**
 * 上海:http://www.sse.com.cn/assortment/stock/list/share/
 * 深圳:http://www.szse.cn/market/product/stock/list/
 */
public readonly record struct StockMetadata(string Code, string Name)
{
    public override string ToString()
    {
        return $"{Code}:{Name}";
    }
}