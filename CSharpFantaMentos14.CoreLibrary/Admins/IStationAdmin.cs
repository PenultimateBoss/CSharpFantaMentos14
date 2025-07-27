using System;

namespace CSharpFantaMentos14.CoreLibrary.Admins;

public interface IStationAdmin
{
    public abstract void Update(TimeSpan current_time, FuelStation fuel_station);
}