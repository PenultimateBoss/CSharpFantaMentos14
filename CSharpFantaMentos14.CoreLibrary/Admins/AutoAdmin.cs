using System.Collections.Generic;

namespace CSharpFantaMentos14.CoreLibrary.Admins;

public sealed class AutoAdmin() : IAdmin
{
    public int CycleLeft { get; private set; } = 60;

    public void Update(FuelStation fuel_station)
    {
        foreach(KeyValuePair<string, FuelPump> pair in fuel_station.PumpDictionary)
        {
            if(pair.Value.LiterCount < 100)
            {
                pair.Value.Refill(500);
            }
        }
        if(fuel_station.CoffeeMachine.CupCount < 10)
        {
            fuel_station.CoffeeMachine.Refill(50);
        }
        if(--CycleLeft <= 0)
        {
            foreach(KeyValuePair<string, FuelPump> pair in fuel_station.PumpDictionary)
            {
                pair.Value.PricePerLiter = Game.ChangePrice(pair.Value.PricePerLiter);
            }
            fuel_station.CoffeeMachine.PricePerCup = Game.ChangePrice(fuel_station.CoffeeMachine.PricePerCup);
            CycleLeft = 60;
        }
        while(fuel_station.customer_queue.Count > 0)
        {
            Customer customer = fuel_station.customer_queue.Dequeue();
            fuel_station.PumpDictionary[customer.FuelName].Buy(customer.LiterCount);
            fuel_station.CoffeeMachine.Buy(customer.CupCount);
        }
    }
}