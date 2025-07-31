using System;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class FuelPump(string name, FuelStation fuel_station)
{
    public string Name { get; } = name;
    public FuelStation FuelStation { get; } = fuel_station;
    public uint LiterCount { get; private set; } = 500;
    public double PricePerLiter
    {
        get => field;
        set
        {
            if(value < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PricePerLiter cannot be negative");
            }
            field = value;
        }
    } = 2.5;

    public bool Buy(uint liter_count)
    {
        if(liter_count is 0)
        {
            return true;
        }
        if(liter_count > LiterCount)
        {
            return false;
        }
        BuyFuelTransaction transaction = new(FuelStation.Model.Game.CurrentTime, Name, liter_count, PricePerLiter);
        FuelStation.transactions.Add(transaction);
        FuelStation.Balance += liter_count * PricePerLiter;
        LiterCount -= liter_count;
        return true;
    }
    public bool Refill(uint liter_count)
    {
        if(liter_count is 0)
        {
            return true;
        }
        double global_price = FuelStation.Model.GlobalPriceState.FuelPriceDictionary[Name];
        double total_price = global_price * liter_count;
        if(total_price > FuelStation.Balance)
        {
            return false;
        }
        RefillFuelTransaction transaction = new(FuelStation.Model.Game.CurrentTime, Name, liter_count, global_price);
        FuelStation.transactions.Add(transaction);
        FuelStation.Balance -= total_price;
        LiterCount += liter_count;
        return true;
    }
}