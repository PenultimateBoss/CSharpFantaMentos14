using System;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class CoffeeMachine(FuelStation fuel_station)
{
    public FuelStation FuelStation { get; } = fuel_station;
    public uint CupCount { get; private set; } = 50;
    public double PricePerCup
    {
        get => field;
        set
        {
            if(value < 0.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PricePerCup cannot be negative");
            }
            field = value;
        }
    } = 1.0;

    public bool Buy(uint cup_count)
    {
        if(cup_count > CupCount)
        {
            return false;
        }
        BuyCoffeeTransaction transaction = new(FuelStation.Model.Game.CurrentTime, cup_count, PricePerCup);
        FuelStation.transactions.Add(transaction);
        FuelStation.Balance += cup_count * PricePerCup;
        CupCount -= cup_count;
        return true;
    }
    public bool Refill(uint cup_count)
    {
        double price = FuelStation.Model.GlobalPriceState.CoffeePrice;
        if(price * cup_count > FuelStation.Balance)
        {
            return false;
        }
        RefillCoffeeTransaction transaction = new(FuelStation.Model.Game.CurrentTime, cup_count, price);
        FuelStation.transactions.Add(transaction);
        FuelStation.Balance -= price;
        CupCount += cup_count;
        return true;
    }
}