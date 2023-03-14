namespace MyDailyDashboard.Data;

public static class DataSizeFormatterService
{

    static readonly string[] units =
        { "Bytes", "KB", "MB", "GB", "TB", "PB" };
    
    public static string Format(Int64 bytes)
    {
        int counter = 0;
        decimal number = (decimal)bytes;
        while (Math.Round(number / 1024) >= 1)
        {
            number = number / 1024;
            counter++;
        }
        return string.Format("{0:n1} {1}", number, units[counter]);
    }
}