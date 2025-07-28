using System;

namespace CSharpFantaMentos14.CoreLibrary;

public abstract record class Transaction(TimeSpan TransactionTime);
public sealed record class BuyFuelTransaction(TimeSpan TransactionTime, string FuelName, uint LiterCount, double FuelPrice) : Transaction(TransactionTime);
public sealed record class RefillFuelTransaction(TimeSpan TransactionTime, string FuelName, uint LiterCount, double GlobalFuelPrice) : Transaction(TransactionTime);
public sealed record class BuyCoffeeTransaction(TimeSpan TransactionTime, uint CupCount, double CoffeePrice) : Transaction(TransactionTime);
public sealed record class RefillCoffeeTransaction(TimeSpan TransactionTime, uint CupCount, double GlobalCoffeePrice) : Transaction(TransactionTime);