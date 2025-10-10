using Microsoft.EntityFrameworkCore;
using MyFinanceMCP.App.Data;
using MyFinanceMCP.App.Interfaces;
using MyFinanceMCP.App.Models;

namespace MyFinanceMCP.App.Services;

/// <summary>
/// Implements <see cref="ITransferService"/> using Entity Framework Core for persistence.
/// </summary>
public class TransferService(MyFinanceDbContext myFinanceDbContext) : ITransferService
{
    private readonly MyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    /// <summary>
    /// Adds a new transfer to the database.
    /// </summary>
    /// <param name="value">The transfer amount as an absolute value.</param>
    /// <param name="date">The local date and time of the transfer.</param>
    /// <param name="description">An optional short note about the transfer.</param>
    /// <param name="type">The transfer type.</param>
    /// <returns>The created <see cref="Models.Transfer"/>.</returns>
    public async Task<Transfer> AddTransferAsync(decimal value, DateTime date, string description, TransferType type)
    {
        var transfer = new Transfer(value, date, description, type);

        _myFinanceDbContext.Transfers.Add(transfer);
        await _myFinanceDbContext.SaveChangesAsync();

        return transfer;
    }

    /// <summary>
    /// Deletes the transfer with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the transfer to delete.</param>
    /// <exception cref="KeyNotFoundException">Thrown when a transfer with the specified <paramref name="id"/> does not exist.</exception>
    public async Task DeleteTransferAsync(Guid id)
    {
        var transfer = _myFinanceDbContext.Transfers.FirstOrDefault(t => t.Id == id) ?? 
            throw new KeyNotFoundException($"Transfer with id {id} not found.");

        _myFinanceDbContext.Transfers.Remove(transfer);

        await _myFinanceDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves transfers that occurred within the specified inclusive date range.
    /// </summary>
    /// <param name="from">The start date (inclusive).</param>
    /// <param name="to">The end date (inclusive).</param>
    /// <returns>A collection of matching <see cref="Models.Transfer"/> instances.</returns>
    public async Task<IEnumerable<Transfer>> GetTransfersFromDateRangeAsync(DateTime from, DateTime to)
    {
        var transfers = await _myFinanceDbContext.Transfers
            .AsNoTracking()
            .Where(t => t.Date >= from && t.Date <= to)
            .ToListAsync();

        return transfers;
    }

    /// <summary>
    /// Updates an existing transfer with new values.
    /// </summary>
    /// <param name="id">The identifier of the transfer to update.</param>
    /// <param name="value">The updated transfer amount as an absolute value.</param>
    /// <param name="date">The updated local date and time of the transfer.</param>
    /// <param name="description">The updated optional short note about the transfer.</param>
    /// <param name="type">The updated transfer type.</param>
    /// <returns>The updated <see cref="Models.Transfer"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when a transfer with the specified <paramref name="id"/> does not exist.</exception>
    public async Task<Transfer> UpdateTransferAsync(Guid id, decimal value, DateTime date, string description, TransferType type)
    {
        var transfer = _myFinanceDbContext.Transfers.FirstOrDefault(t => t.Id == id) ??
            throw new KeyNotFoundException($"Transfer with id {id} not found.");

        transfer.Update(value, date, description, type);
        _myFinanceDbContext.Transfers.Update(transfer);
        await _myFinanceDbContext.SaveChangesAsync();

        return transfer;
    }
}
