using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFantaMentos14.CoreLibrary.Admins;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class FuelStationContext()
{
    private PriceList PriceList { get; } = new();
    private ManualResetEventSlim RunSignal { get; } = new(true);
    private CustomerGenerator CustomerGenerator { get; } = new();
    private CancellationTokenSource StopSource { get; set; } = new();
    public Task DayTask { get; private set; } = Task.CompletedTask;
    public TimeSpan CurrentTime { get; private set; } = new(1, 9, 0, 0);
    public int TimeSpeed
    {
        get => field;
        set
        {
            if(value < 1 || value > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "TimeSpeed must be in range [1..1000]");
            }
            field = value;
        }
    }

    public event Action? OnDayEnd;
    public event Action? OnDayStart;
    public event Action? OnDayUpdate;

    public FuelStation AddFuelStation(string name, IStationAdmin admin)
    {
        FuelStation fuel_station = new(name, PriceList, admin);
        CustomerGenerator.FuelStations.Add(fuel_station);
        return fuel_station;
    }
    public bool RemoveFuelStation(FuelStation fuel_station)
    {
        return CustomerGenerator.FuelStations.Remove(fuel_station);
    }
    public void StartDay()
    {
        DayTask = Task.Run(async delegate
        {
            OnDayStart?.Invoke();
            while(CurrentTime.Hours < 21)
            {
                RunSignal.Wait();
                StopSource.Token.ThrowIfCancellationRequested();
                await Task.Delay(1000 / TimeSpeed);
                CustomerGenerator.Update(CurrentTime);
                OnDayUpdate?.Invoke();
                CurrentTime += TimeSpan.FromMinutes(1);
                if(CurrentTime.Minutes is 59)
                {
                    PriceList.RandomChange();
                }
            }
            OnDayEnd?.Invoke();
        }, StopSource.Token);
    }
    public void PauseDay()
    {
        RunSignal.Reset();
    }
    public void ResumeDay()
    {
        RunSignal.Set();
    }
    public void Reset()
    {
        StopSource.Cancel();
        RunSignal.Set();

        TimeSpeed = 1;
        CurrentTime = new(1, 9, 0, 0);
        CustomerGenerator.FuelStations.Clear();

        StopSource.Dispose();
        StopSource = new CancellationTokenSource();
    }
}