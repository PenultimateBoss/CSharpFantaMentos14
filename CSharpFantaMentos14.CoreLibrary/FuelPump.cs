using System;
using CSharpFantaMentos14.CoreLibrary.Transactions;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class FuelPump(FuelStation fuel_station, string name, uint liter_count, double price_per_liter)
{
    public string Name { get; } = name;
    public FuelStation FuelStation { get; } = fuel_station;
    public uint LiterCount { get; private set; } = liter_count;
    public double PricePerLiter
    {
        get => field;
        private set
        {
            if(value < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PricePerLiter cannot be negative");
            }
            field = value;
        }
    } = price_per_liter;

    public double Buy(TimeSpan current_time, uint liter_count)
    {
        if(liter_count > LiterCount)
        {
            throw new ArgumentOutOfRangeException(nameof(liter_count), "Not enough fuel available");
        }
        LiterCount -= liter_count;
        double price = liter_count * PricePerLiter;
        BuyFuelTransaction transaction = new(current_time, Name, liter_count, price);
        FuelStation.Transactions.Add(transaction);
        return price;
    }
    public void Refill(TimeSpan current_time, uint liter_count, double price)
    {
        RefillFuelTransaction transaction = new(current_time, Name, liter_count, price);
        FuelStation.Transactions.Add(transaction);
        LiterCount += liter_count;
    }
    public void SetPrice(TimeSpan current_time, double price)
    {
        PricePerLiter = price;
        ChangePriceTransaction transaction = new(current_time, Name, price);
        FuelStation.Transactions.Add(transaction);
    }
}