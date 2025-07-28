using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class Game
{
    #region Static
    public static double ChangePrice(double price)
    {
        price += price * 0.1 * Random.Shared.Next(-1, 2) * Random.Shared.NextDouble();
        return double.Max(price, 0.1);
    }
    #endregion

    #region Instance
    private ManualResetEventSlim RunSignal { get; } = new(true);
    public FuelStationModel FuelStationModel { get; }
    public TimeSpan CurrentTime { get; private set; } = new(1, 9, 0, 0);
    public int TimeSpeed
    {
        get => field;
        set
        {
            if(value is < 1 or > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "TimeSpeed valid range == [1..1000]");
            }
            field = value;
        }
    } = 1;
    public bool IsRunning { get; set; }

    public event Action? OnUpdate;
    public event Action? DayEnded;
    public event Action? DayPaused;
    public event Action? DayStarted;
    public event Action? DayResumed;

    public Game()
    {
        FuelStationModel = new FuelStationModel(this);
    }

    public Task StartDay()
    {
        if(IsRunning is true)
        {
            return Task.FromException(new InvalidOperationException("Day not ended yet"));
        }
        IsRunning = true;
        return Task.Run(async ValueTask() =>
        {
            TimeSpeed = 1;
            RunSignal.Set();
            CurrentTime = new(CurrentTime.Days, 9, 0, 0);

            DayStarted?.Invoke();
            while(CurrentTime.Hours < 21)
            {
                RunSignal.Wait();
                await Task.Delay(1000 / TimeSpeed);
                FuelStationModel.Update();
                OnUpdate?.Invoke();
            }
            IsRunning = false;
            DayEnded?.Invoke();
        });
    }
    public void PauseDay()
    {
        RunSignal.Reset();
        DayPaused?.Invoke();
    }
    public void ResumeDay()
    {
        RunSignal.Set();
        DayResumed?.Invoke();
    }
    #endregion
}