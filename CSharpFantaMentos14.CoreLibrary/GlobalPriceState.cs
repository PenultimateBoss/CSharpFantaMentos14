using System.Linq;
using System.Collections.Generic;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class GlobalPriceState()
{
    private readonly Dictionary<string, double> fuel_price_dictionary = [];

    public IReadOnlyDictionary<string, double> FuelPriceDictionary
    {
        get => fuel_price_dictionary;
    }
    public int CycleCount { get; private set; } = 60;
    public double CoffeePrice { get; private set; } = 0.9;

    internal void Update()
    {
        if(--CycleCount > 0)
        {
            return;
        }
        CoffeePrice = Game.ChangePrice(CoffeePrice);
        for(int index = 0; index < fuel_price_dictionary.Count; index++)
        {
            KeyValuePair<string, double> pair = fuel_price_dictionary.ElementAt(index);
            fuel_price_dictionary[pair.Key] = Game.ChangePrice(pair.Value);
        }
        CycleCount = 60;
    }
    public bool AddFuel(string fuel_name)
    {
        return fuel_price_dictionary.TryAdd(fuel_name, 2.0);
    }
}