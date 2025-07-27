using System;

namespace CSharpFantaMentos14.CoreLibrary.Transactions;

public sealed record BuyCoffeeTransaction(TimeSpan TransactionTime, uint CupCount, double Price) : ITransaction
{
    #region Base
    public override string ToString()
    {
        return $"BuyCoffee({TransactionTime}): CupCount = {CupCount}; Price = {Price}";
    }
    #endregion
}