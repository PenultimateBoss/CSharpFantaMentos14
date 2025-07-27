using System;

namespace CSharpFantaMentos14.CoreLibrary.Transactions;

public sealed record ChangePriceTransaction(TimeSpan TransactionTime, string Name, double Price) : ITransaction
{
    #region Base
    public override string ToString()
    {
        return $"ChangePrice({TransactionTime}): Name = {Name}; Price = {Price}";
    }
    #endregion
}