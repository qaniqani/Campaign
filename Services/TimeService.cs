namespace Campaign.Services;

public static class TimeService
{
    private static DateTime _currentTime;

    static TimeService()
    {
        _currentTime = new(1900, 1, 1, 0, 0, 0);
    }

    public static DateTime GetCurrentTime()
    {
        return _currentTime;
    }

    public static void IncreaseTime(int hours)
    {
        _currentTime = _currentTime.AddHours(hours);
        Console.WriteLine($"Time is {_currentTime:HH:mm}");
    }
}