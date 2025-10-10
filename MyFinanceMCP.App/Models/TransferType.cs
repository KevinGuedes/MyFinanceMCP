namespace MyFinanceMCP.App.Models;

/// <summary>
/// Specifies the type of a <see cref="Transfer"/>.
/// </summary>
public enum TransferType
{
    /// <summary>
    /// Indicates a transfer that reduces the account balance.
    /// </summary>
    Expense,

    /// <summary>
    /// Indicates a transfer that increases the account balance.
    /// </summary>
    Income,
}
