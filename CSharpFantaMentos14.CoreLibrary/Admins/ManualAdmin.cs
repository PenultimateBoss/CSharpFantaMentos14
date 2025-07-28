using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CSharpFantaMentos14.CoreLibrary.Admins;

public sealed class ManualAdmin() : IAdmin
{
    private ConcurrentQueue<Action<FuelStation>> Actions { get; } = [];

    public Task<bool> RefillFuelAsync(string name, uint liter_count)
    {
        TaskCompletionSource<bool> task_source = new();
        Actions.Enqueue(void(FuelStation fuel_station) =>
        {
            if(fuel_station.PumpDictionary.TryGetValue(name, out FuelPump? pump) is false)
            {
                task_source.SetException(new ArgumentException($"Fuel pump with name '{name}' not found"));
            }
            else
            {
                task_source.SetResult(pump.Refill(liter_count));
            }
        });
        return task_source.Task;
    }
    public Task<bool> RefillCoffeeAsync(uint cup_count)
    {
        TaskCompletionSource<bool> task_source = new();
        Actions.Enqueue(void(FuelStation fuel_station) =>
        {
            task_source.SetResult(fuel_station.CoffeeMachine.Refill(cup_count));
        });
        return task_source.Task;
    }
    public Task ChangeFuelPriceAsync(string name, double price)
    {
        TaskCompletionSource task_source = new();
        Actions.Enqueue(void(FuelStation fuel_station) =>
        {
            if(fuel_station.PumpDictionary.TryGetValue(name, out FuelPump? pump) is false)
            {
                task_source.SetException(new ArgumentException($"Fuel pump with name '{name}' not found"));
            }
            else
            {
                try
                {
                    pump.PricePerLiter = price;
                    task_source.SetResult();
                }
                catch(Exception exception)
                {
                    task_source.SetException(exception);
                }
            }
        });
        return task_source.Task;
    }
    public Task ChangeCoffeePriceAsync(double price)
    {
        TaskCompletionSource task_source = new();
        Actions.Enqueue(void(FuelStation fuel_station) =>
        {
            try
            {
                fuel_station.CoffeeMachine.PricePerCup = price;
                task_source.SetResult();
            }
            catch(Exception exception)
            {
                task_source.SetException(exception);
            }
        });
        return task_source.Task;
    }
    public void Update(FuelStation fuel_station)
    {
        foreach(Action<FuelStation> action in Actions)
        {
            action(fuel_station);
        }
        while(fuel_station.customer_queue.Count > 0)
        {
            Customer customer = fuel_station.customer_queue.Dequeue();
            fuel_station.PumpDictionary[customer.FuelName].Buy(customer.LiterCount);
            fuel_station.CoffeeMachine.Buy(customer.CupCount);
        }
    }
}