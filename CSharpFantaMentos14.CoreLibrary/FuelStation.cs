using System.Collections.Generic;
using CSharpFantaMentos14.CoreLibrary.Admins;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class FuelStation
{
    internal Queue<Customer> customer_queue = [];
    internal List<Transaction> transactions = [];
    internal Dictionary<string, FuelPump> pump_dictionary = [];

    public string Name { get; }
    public IAdmin Admin { get; }
    public FuelStationModel Model { get; }
    public CoffeeMachine CoffeeMachine { get; }
    public IReadOnlyCollection<Customer> Customers
    {
        get => customer_queue;
    }
    public IReadOnlyList<Transaction> Transactions
    {
        get => transactions;
    }
    public IReadOnlyDictionary<string, FuelPump> PumpDictionary
    {
        get => pump_dictionary;
    }
    public double Balance { get; internal set; } = 500.0;

    public FuelStation(string name, IAdmin admin, FuelStationModel model)
    {
        Name = name;
        Admin = admin;
        Model = model;
        CoffeeMachine = new CoffeeMachine(this);
    }

    internal void Update()
    {
        Admin.Update(this);
    }
    public bool AddFuelPump(string name)
    {
        bool added = pump_dictionary.TryAdd(name, new FuelPump(name, this));
        if(added is true)
        {
            Model.GlobalPriceState.AddFuel(name);
        }
        return added;
    } 
    public bool RemoveFuelPump(string name)
    {
        return pump_dictionary.Remove(name);
    }
}