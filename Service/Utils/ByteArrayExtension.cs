namespace Service.Utils;

public static class BatchGroup
{
    public static IEnumerable<byte[]> Group(this IEnumerable<byte> bs, int size)
    {
        var i = 0;
        return from b in bs
            let num = i++
            group b by num / size
            into g
            select g.ToArray();
    }
}