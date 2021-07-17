public static class Utilities
{
    public static string ToPercentage(this float number)
    {
        return (number * 100).ToString("F0") + "%";
    }
}