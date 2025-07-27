using System;

namespace CSharpFantaMentos14.CoreLibrary.Transactions;

public interface ITransaction
{
    public abstract TimeSpan TransactionTime { get; }

    public abstract string ToString();
}