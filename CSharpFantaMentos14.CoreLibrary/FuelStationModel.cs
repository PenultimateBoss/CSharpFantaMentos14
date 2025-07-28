using System.Collections.Generic;
using CSharpFantaMentos14.CoreLibrary.Admins;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class FuelStationModel
{
    private readonly Dictionary<string, FuelStation> station_dictionary = [];

    public Game Game { get; }
    public GlobalPriceState GlobalPriceState { get; } = new();
    public CustomerGenerator CustomerGenerator { get; }
    public IReadOnlyDictionary<string, FuelStation> StationDictionary
    {
        get => station_dictionary;
    }

    public FuelStationModel(Game game_model)
    {
        Game = game_model;
        CustomerGenerator = new CustomerGenerator(this);
    }

    internal void Update()
    {
        GlobalPriceState.Update();
        CustomerGenerator.Update();
        foreach(KeyValuePair<string, FuelStation> pair in StationDictionary)
        {
            pair.Value.Update();
        }
    }
    public AutoAdmin? AddAutoStation(string name)
    {
        if(StationDictionary.ContainsKey(name) is true)
        {
            return null;
        }
        AutoAdmin admin = new();
        station_dictionary.Add(name, new FuelStation(name, admin, this));
        return admin;
    }
    public ManualAdmin? AddManualStation(string name)
    {
        if(StationDictionary.ContainsKey(name) is true)
        {
            return null;
        }
        ManualAdmin admin = new();
        station_dictionary.Add(name, new FuelStation(name, admin, this));
        return admin;
    }
    public bool RemoveStation(string name)
    {
        return station_dictionary.Remove(name);
    }
}