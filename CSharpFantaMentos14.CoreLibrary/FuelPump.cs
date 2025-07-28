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
        double price = FuelStation.Model.GlobalPriceState.FuelPriceDictionary[Name];
        if(price * liter_count > FuelStation.Balance)
        {
            return false;
        }
        RefillFuelTransaction transaction = new(FuelStation.Model.Game.CurrentTime, Name, liter_count, price);
        FuelStation.transactions.Add(transaction);
        FuelStation.Balance -= price;
        LiterCount += liter_count;
        return true;
    }
}