using MyFinanceMCP.App.Models;

namespace MyFinanceMCP.App.Interfaces;

/// <summary>
/// Provides operations for creating, reading, updating, and deleting <see cref="Models.Transfer"/> entities.
/// </summary>
public interface ITransferService
{
    /// <summary>
    /// Adds a new transfer.
    /// </summary>
    /// <param name="value">The transfer amount as an absolute value.</param>
    /// <param name="date">The local date and time of the transfer.</param>
    /// <param name="description">An optional short note about the transfer.</param>
    /// <param name="type">The transfer type.</param>
    /// <returns>The created <see cref="Models.Transfer"/>.</returns>
    Task<Transfer> AddTransferAsync(decimal value, DateTime date, string description, TransferType type);

    /// <summary>
    /// Updates an existing transfer.
    /// </summary>
    /// <param name="id">The identifier of the transfer to update.</param>
    /// <param name="value">The updated transfer amount as an absolute value.</param>
    /// <param name="date">The updated local date and time of the transfer.</param>
    /// <param name="description">The updated optional short note about the transfer.</param>
    /// <param name="type">The updated transfer type.</param>
    /// <returns>The updated <see cref="Models.Transfer"/>.</returns>
    Task<Transfer> UpdateTransferAsync(Guid id, decimal value, DateTime date, string description, TransferType type);

    /// <summary>
    /// Deletes a transfer by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the transfer to delete.</param>
    Task DeleteTransferAsync(Guid id);

    /// <summary>
    /// Retrieves transfers within the specified inclusive date range.
    /// </summary>
    /// <param name="from">The start date (inclusive).</param>
    /// <param name="to">The end date (inclusive).</param>
    /// <returns>A collection of transfers that match the date range.</returns>
    Task<IEnumerable<Transfer>> GetTransfersFromDateRangeAsync(DateTime from, DateTime to);
}
