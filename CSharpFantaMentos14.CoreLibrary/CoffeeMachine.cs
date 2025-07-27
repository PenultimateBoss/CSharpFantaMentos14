using System;
using CSharpFantaMentos14.CoreLibrary.Transactions;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class CoffeeMachine(FuelStation fuel_station, uint cup_count, double price_per_cup)
{
    public FuelStation FuelStation { get; } = fuel_station;
    public uint CupCount { get; private set; } = cup_count;
    public double PricePerCup
    {
        get => field;
        private set
        {
            if(value < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PricePerCup cannot be negative");
            }
            field = value;
        }
    } = price_per_cup;

    public double Buy(TimeSpan current_time, uint cup_count)
    {
        if(cup_count > CupCount)
        {
            throw new ArgumentOutOfRangeException(nameof(cup_count), "Not enough cups available");
        }
        CupCount -= cup_count;
        double price = PricePerCup * cup_count;
        BuyCoffeeTransaction transaction = new(current_time, cup_count, price);
        FuelStation.Transactions.Add(transaction);
        return price;
    }
    public void Refill(TimeSpan current_time, uint cup_count, double price)
    {
        CupCount += cup_count;
        RefillCoffeeTransaction transaction = new(current_time, cup_count, price);
        FuelStation.Transactions.Add(transaction);
    }
    public void SetPrice(TimeSpan current_time, double price)
    {
        PricePerCup = price;
        ChangePriceTransaction transaction = new(current_time, "Coffee", price);
        FuelStation.Transactions.Add(transaction);
    }
}