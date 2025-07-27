using System;
using System.Collections.Generic;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class PriceList()
{
    private readonly List<(string name, double price)> prices = [];
    public IReadOnlyCollection<(string name, double price)> Prices
    {
        get => prices;
    }

    internal bool AddPrice(string name, double price)
    {
        if(price <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero");
        }
        if(prices.Exists(p => p.name == name) is false)
        {
            prices.Add((name, price));
            return true;
        }
        return false;
    }
    internal bool RemovePrice(string name)
    {
        int index = prices.FindIndex(pair => pair.name == name);
        if(index is not -1)
        {
            prices.RemoveAt(index);
            return true;
        }
        return false;
    }
    internal void RandomChange()
    {
        for(int index = 0; index < prices.Count; index++)
        {
            (string name, double price) = prices[index];
            prices[index] = (name, price += price * 0.1 * Random.Shared.Next(-1, 2) * Random.Shared.NextDouble());
        }
    }
}