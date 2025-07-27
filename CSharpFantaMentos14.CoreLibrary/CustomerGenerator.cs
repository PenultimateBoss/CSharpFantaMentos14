using System;
using System.Linq;
using System.Collections.Generic;

namespace CSharpFantaMentos14.CoreLibrary;

public sealed class CustomerGenerator()
{
    public HashSet<FuelStation> FuelStations { get; } = [];
    public TimeSpan GenerationPeriod { get; } = TimeSpan.FromMinutes(5);
    public TimeSpan TimeLeft { get; private set; } = TimeSpan.FromMinutes(5);

    public void Update(TimeSpan current_time)
    {
        TimeLeft -= TimeSpan.FromMinutes(1);
        if(TimeLeft <= TimeSpan.Zero)
        {
            TimeLeft = GenerationPeriod;
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
            GetRandomFuelStation()?.CustomerQueue.Enqueue(customer);

            string GetRandomFuelName()
            {
                string[] fuel_names = FuelStations.SelectMany(fuel_station => fuel_station.FuelPumps).Select(fuel_pump => fuel_pump.Name).Distinct().ToArray();
                return fuel_names[Random.Shared.Next(fuel_names.Length)];
            }
            FuelStation? GetRandomFuelStation()
            {
                FuelStation[] fuel_stations = [..FuelStations.Where(fuel_station =>
                {
                    return fuel_station.FuelPumps.Any(fuel_pump =>
                    {
                        return fuel_pump.Name == fuel_name && fuel_pump.LiterCount >= liter_count;
                    }) && fuel_station.CoffeeMachine.CupCount >= cup_count;
                })];
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
                        }
                        return fuel_stations[^1];

                        double GetTotalPrice(FuelStation fuel_station)
                        {
                            FuelPump fuel_pump = fuel_station.FuelPumps.First(fuel_pump => fuel_pump.Name == fuel_name);
                            return fuel_pump.PricePerLiter * liter_count + fuel_station.CoffeeMachine.PricePerCup * cup_count;
                        }
                    }
                }
            }
        }
        foreach(FuelStation fuel_station in FuelStations)
        {
            fuel_station.Update(current_time);
        }
    }
}