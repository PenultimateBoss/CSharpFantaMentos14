using System;
using System.Collections.Generic;
using CSharpFantaMentos14.CoreLibrary.Admins;
using CSharpFantaMentos14.CoreLibrary.Transactions;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class FuelStation
{
    internal IStationAdmin Admin { get; }
    internal PriceList PriceList { get; }
    internal CoffeeMachine CoffeeMachine { get; }
    internal HashSet<FuelPump> FuelPumps { get; }
    internal Queue<Customer> CustomerQueue { get; }
    internal List<ITransaction> Transactions { get; }
    public string Name { get; }
    public double Balance { get; internal set; }

    public FuelStation(string name, PriceList price_list, IStationAdmin admin)
    {
        Name = name;
        Admin = admin;
        FuelPumps = [];
        Balance = 1000.0;
        Transactions = [];
        CustomerQueue = [];
        PriceList = price_list;
        CoffeeMachine = new CoffeeMachine(this, 50, 1.0);
        PriceList.AddPrice("Coffee", 0.9);
    }

    public FuelPump AddFuelPump(string name)
    {
        FuelPump fuel_pump = new(this, name, 1000, 2.5);
        FuelPumps.Add(fuel_pump);
        PriceList.AddPrice(name, 2.4);
        return fuel_pump;
    }
    public bool RemoveFuelPump(FuelPump fuel_pump)
    {
        return FuelPumps.Remove(fuel_pump);
    }
    public void Update(TimeSpan current_time)
    {
        Admin.Update(current_time, this);
    }
}