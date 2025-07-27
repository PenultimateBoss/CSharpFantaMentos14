using System;
using System.Linq;
using System.Collections.Concurrent;

namespace CSharpFantaMentos14.CoreLibrary.Admins;

public sealed class ManualAdmin() : IStationAdmin
{
    private ConcurrentQueue<Action<ManualAdmin, TimeSpan, FuelStation>> Actions { get; } = [];

    public void RefillFuel(FuelPump pump, uint liter_count)
    {
        Actions.Enqueue(delegate(ManualAdmin admin, TimeSpan current_time, FuelStation fuel_station)
        {
            double price = fuel_station.PriceList.Prices.First(pair => pair.name == pump.Name).price * liter_count;
            if(fuel_station.Balance < price)
            {
                return;
            }
            pump.Refill(liter_count);
            fuel_station.Balance -= price;
        });
    }
    public void RefillCoffee(uint cup_count)
    {
        Actions.Enqueue(delegate(ManualAdmin admin, TimeSpan current_time, FuelStation fuel_station)
        {
            double price = fuel_station.PriceList.Prices.First(pair => pair.name is "Coffee").price * cup_count;
            if(fuel_station.Balance < price)
            {
                return;
            }
            fuel_station.CoffeeMachine.Refill(cup_count);
            fuel_station.Balance -= price;
        });
    }
    public void ChangeFuelPrice(FuelPump pump, double price)
    {
        Actions.Enqueue(delegate(ManualAdmin admin, TimeSpan current_time, FuelStation fuel_station)
        {
            pump.PricePerLiter = price;
        });
    }
    public void Update(TimeSpan current_time, FuelStation fuel_station)
    {
        foreach(Action<ManualAdmin, TimeSpan, FuelStation> action in Actions)
        {
            action(this, current_time, fuel_station);
        }
        while(fuel_station.CustomerQueue.Count > 0)
        {
            Customer customer = fuel_station.CustomerQueue.Dequeue();
            FuelPump pump = fuel_station.FuelPumps.First(pump => pump.Name == customer.FuelName);
            if(pump.LiterCount >= customer.LiterCount)
            {
                fuel_station.Balance += pump.Buy(customer.LiterCount);
            }
            if(fuel_station.CoffeeMachine.CupCount >= customer.CupCount)
            {
                fuel_station.Balance += fuel_station.CoffeeMachine.Buy(customer.CupCount);
            }
        }
    }
}