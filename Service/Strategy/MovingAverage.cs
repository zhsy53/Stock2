namespace Service.Strategy;

public readonly record struct MovingAverage(double Avg, List<int> Curve)
{
    public override string ToString()
    {
        return $"均值:{Avg}\t曲线:{string.Join(",", Curve)}";
    }

    public static MovingAverage GenerateFromList(List<int> list)
    {
        var avg = list.Average();

        var pre = ToInt(list[0] > avg);

        var r = new List<int> { pre };

        foreach (var i in list.Skip(1).Select(o => o > avg).Select(ToInt).ToList())
        {
            if (i == pre) r[^1] = r[^1] + i;
            else r.Add(i);

            pre = i;
        }

        return new MovingAverage(avg, r.Where(o => o is > 10 or < -10).ToList());
    }

    private static int ToInt(bool @bool)
    {
        return @bool ? 1 : -1;
    }
}