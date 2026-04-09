using EShop.Domain.Enums;

namespace EShop.Domain.ValueObject;

public record Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Of(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        if (!Enum.IsDefined(typeof(Currency), currency))
            throw new ArgumentException("Currency is required.", nameof(currency));

        return new Money(amount, currency);
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot add different currencies.");
        return Of(left.Amount + right.Amount, left.Currency);
    }

    public static Money Zero(Currency currency)
    {
        return new Money(0, currency);
    }

    public static Money Vnd(decimal amount)
    {
        return Of(amount, Currency.VND);
    }
    public static Money Usd(decimal amount)
    {
        return Of(amount, Currency.USD);
    }
}
