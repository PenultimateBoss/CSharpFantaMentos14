using System;
using System.Linq;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed record class Customer(string FuelName, uint LiterCount, uint CupCount);
public sealed class CustomerGenerator(FuelStationModel fuel_station_model)
{
    public FuelStationModel FuelStationModel
    {
        get => fuel_station_model;
    }
    public int CycleLeft { get; private set; } = 5;

    internal void Update()
    {
        if(--CycleLeft > 0)
        {
            return;
        }
        string fuel_name = GetRandomFuelName();
        uint liter_count = (uint)Random.Shared.Next(5, 31);
        uint cup_count = Random.Shared.NextDouble() switch
        {
            >= 0.00 and < 0.33 => 0,
            >= 0.33 and < 0.66 => 1,
            >= 0.66 and < 0.85 => 2,
            >= 0.85 and < 0.93 => 3,
            >= 0.93 and < 0.99 => 4,
            _ => 5
        };
        Customer customer = new(fuel_name, liter_count, cup_count);
        GetRandomFuelStation()?.customer_queue.Enqueue(customer);
        CycleLeft = 5;

        string GetRandomFuelName()
        {
            string[] fuel_names = FuelStationModel.StationDictionary.SelectMany(pair => pair.Value.PumpDictionary).Select(pair => pair.Key).Distinct().ToArray();
            return fuel_names[Random.Shared.Next(fuel_names.Length)];
        }
        FuelStation? GetRandomFuelStation()
        {
            FuelStation[] fuel_stations = [..FuelStationModel.StationDictionary.Where(pair =>
            {
                return pair.Value.PumpDictionary.Any(pair =>
                {
                    return pair.Key == fuel_name && pair.Value.LiterCount >= liter_count;
                }) && pair.Value.CoffeeMachine.CupCount >= cup_count;
            }).Select(pair => pair.Value)];
            switch(fuel_stations.Length)
            {
                case 0:
                {
                    return null;
                }
                case 1:
                {
                    return fuel_stations[0];
                }
                default:
                {
                    fuel_stations = [..fuel_stations.OrderBy(GetTotalPrice)];
                    double least_price = GetTotalPrice(fuel_stations[0]);
                    double[] propabilities = [..Enumerable.Repeat(1.00 / fuel_stations.Length, fuel_stations.Length)];
                    for(int index = 1; index < fuel_stations.Length; index++)
                    {
                        double price = GetTotalPrice(fuel_stations[index]);
                        double scale = least_price / price;
                        double difference = propabilities[index] - (propabilities[index] * scale);
                        propabilities[index] -= difference;
                        difference /= index;
                        for(int index2 = index - 1; index2 >= 0; index2--)
                        {
                            propabilities[index2] += difference;
                        }
                    }
                    double propapility = Random.Shared.NextDouble();
                    for(int index = 0; index < propabilities.Length; index++)
                    {
                        if(propapility < propabilities[index])
                        {
                            return fuel_stations[index];
                        }
                        propapility -= propabilities[index];
                    }
                    return fuel_stations[^1];

                    double GetTotalPrice(FuelStation fuel_station)
                    {
                        return fuel_station.PumpDictionary[fuel_name].PricePerLiter * liter_count + fuel_station.CoffeeMachine.PricePerCup * cup_count;
                    }
                }
            }
        }
    }
}