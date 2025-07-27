using System;

namespace CSharpFantaMentos14.CoreLibrary.Transactions;

public sealed record RefillFuelTransaction(TimeSpan TransactionTime, string FuelName, uint LiterCount, double Price) : ITransaction
{
    #region Base
    public override string ToString()
    {
        return $"RefillFuel({TransactionTime}): FuelName = {FuelName}; LiterCount = {LiterCount}; Price = {Price}";
    }
    #endregion
}