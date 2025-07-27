using System;

namespace CSharpFantaMentos14.CoreLibrary.Transactions;

public sealed record RefillCoffeeTransaction(TimeSpan TransactionTime, uint CupCount, double Price) : ITransaction
{
    #region Base
    public override string ToString()
    {
        return $"RefillCoffee({TransactionTime}): CupCount = {CupCount}; Price = {Price}";
    }
    #endregion
}