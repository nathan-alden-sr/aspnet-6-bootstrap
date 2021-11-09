using System.Text.RegularExpressions;

namespace Company.Product.WebApi.Common;

public sealed record EmailAddress
{
    public static readonly Regex EmailAddressRegex = new(
        @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");

    private EmailAddress()
    {
    }

    public EmailAddress(string value)
    {
        ThrowIfNull(value, nameof(value));

        if (!IsValid(value))
        {
            ThrowArgumentException("Value is not a valid email address.", nameof(value));
        }

        Value = value.ToLowerInvariant();
    }

    public string Value { get; private init; } = default!;

    public static EmailAddress Parse(string value) =>
        new(value);

    public static bool TryParse(string? value, out EmailAddress emailAddress)
    {
        if (value is null || !IsValid(value))
        {
            emailAddress = default!;

            return false;
        }

        emailAddress = new EmailAddress { Value = value };

        return true;
    }

    public static bool IsValid(string emailAddress)
    {
        ThrowIfNull(emailAddress, nameof(emailAddress));

        return EmailAddressRegex.IsMatch(emailAddress);
    }

    public override string ToString() => Value;
}
