namespace MyFinanceMCP.App.Models;

/// <summary>
/// Represents a financial transfer record such as an income or an expense.
/// </summary>
public class Transfer(
    decimal value,
    DateTime date,
    string description,
    TransferType type)
{
    /// <summary>
    /// Gets the unique identifier of the transfer.
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Gets the monetary value of the transfer. The value is stored as an absolute amount.
    /// </summary>
    public decimal Value { get; private set; } = value;

    /// <summary>
    /// Gets the local date and time when the transfer occurred.
    /// </summary>
    public DateTime Date { get; private set; } = date;

    /// <summary>
    /// Gets the optional short description of the transfer (for example, <c>"Rent"</c>).
    /// </summary>
    public string Description { get; private set; } = description;

    /// <summary>
    /// Gets the type of the transfer indicating whether it is an income or an expense.
    /// </summary>
    public TransferType Type { get; private set; } = type;

    /// <summary>
    /// Updates the transfer properties with new values.
    /// </summary>
    /// <param name="value">The updated transfer amount as an absolute value.</param>
    /// <param name="date">The updated local date and time for the transfer.</param>
    /// <param name="description">The updated optional short description.</param>
    /// <param name="type">The updated transfer type.</param>
    public void Update(decimal value, DateTime date, string description, TransferType type)
    {
        Value = value;
        Date = date;
        Description = description;
        Type = type;
    }
}
