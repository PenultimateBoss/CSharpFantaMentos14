using System;
using System.Linq;

namespace CSharpFantaMentos14.CoreLibrary.Admins;

public sealed class AutoAdmin() : IStationAdmin
{
    public TimeSpan ChangePricesTimeLeft { get; private set; } = TimeSpan.FromMinutes(60);

    public void Update(TimeSpan current_time, FuelStation fuel_station)
    {
        foreach(FuelPump pump in fuel_station.FuelPumps)
        {
            if(pump.LiterCount < 100)
            {
                double price = fuel_station.PriceList.Prices.First(pair => pair.name == pump.Name).price * 500;
                if(fuel_station.Balance < price)
                {
                    continue;
                }
                pump.Refill(500);
                fuel_station.Balance -= price;
            }
        }
        if(fuel_station.CoffeeMachine.CupCount < 10)
        {
            double price = fuel_station.PriceList.Prices.First(pair => pair.name is "Coffee").price * 50;
            if(fuel_station.Balance >= price)
            {
                fuel_station.CoffeeMachine.Refill(50);
                fuel_station.Balance -= price;
            }
        }
        if(ChangePricesTimeLeft <= TimeSpan.Zero)
        {
            foreach(FuelPump pump in fuel_station.FuelPumps)
            {
                pump.PricePerLiter += pump.PricePerLiter * 0.1 * Random.Shared.Next(-1, 2) * Random.Shared.NextDouble();
            }
            fuel_station.CoffeeMachine.PricePerCup += fuel_station.CoffeeMachine.PricePerCup * 0.1 * Random.Shared.Next(-1, 2) * Random.Shared.NextDouble();
            ChangePricesTimeLeft = TimeSpan.FromMinutes(60);
        }
        ChangePricesTimeLeft -= TimeSpan.FromMinutes(1);
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