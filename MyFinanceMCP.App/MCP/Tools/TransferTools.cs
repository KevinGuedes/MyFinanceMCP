using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using MyFinanceMCP.App.Interfaces;
using MyFinanceMCP.App.Models;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyFinanceMCP.App.MCP.Tools;

[McpServerToolType]
public sealed class TransferTools(ITransferService transferService, ILogger<TransferTools> logger)
{
    private readonly ITransferService transferService = transferService;
    private readonly ILogger<TransferTools> logger = logger;

    /// <summary>
    /// Adds a new <see cref="Transfer"/> to the data store.
    /// </summary>
    /// <param name="value">The transfer amount as an absolute, positive value.</param>
    /// <param name="date">The local date and time of the transfer.</param>
    /// <param name="description">An optional short note about the transfer (for example, <c>"Rent"</c>).</param>
    /// <param name="type">The transfer type as a string; valid values are <c>"Income"</c> and <c>"Expense"</c> (case-insensitive).</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the created <see cref="Transfer"/> serialized as JSON.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="type"/> is not a valid <see cref="Models.TransferType"/> value.</exception>
    [McpServerTool, Description("Add a new transfer")]
    public async Task<string> AddTransferAsync(
        [Description("Amount (absolute, positive)")] decimal value,
        [Description("Transfer date and time (local)")] DateTime date,
        [Description("Optional short note (e.g., 'Rent')")] string description,
        [Description("Type: 'Income' or 'Expense' (case-insensitive)")] string type)
    {
        logger.LogInformation("AddTransferAsync called with Value={Value}, Date={Date}, Description={Description}, Type={Type}",
            value, date, description, type);

        if (!Enum.TryParse<TransferType>(type, true, out var transferType))
        {
            logger.LogWarning("AddTransferAsync - invalid transfer type: {Type}", type);
            throw new ArgumentException($"Invalid transfer type: {type}. Valid types are 'Income' and 'Expense'.");
        }

        var transfer = await transferService.AddTransferAsync(value, date, description, transferType);
        logger.LogInformation("AddTransferAsync - transfer created with Id={Id}", transfer?.Id);
        return JsonSerializer.Serialize(transfer, MyFinanceContext.Default.Transfer);
    }

    /// <summary>
    /// Updates an existing <see cref="Transfer"/> identified by <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The identifier of the transfer to update.</param>
    /// <param name="value">The updated transfer amount as an absolute, positive value.</param>
    /// <param name="date">The updated local date and time of the transfer.</param>
    /// <param name="description">An updated optional short note about the transfer.</param>
    /// <param name="type">The updated transfer type as a string; valid values are <c>"Income"</c> and <c>"Expense"</c> (case-insensitive).</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the updated <see cref="Transfer"/> serialized as JSON.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="type"/> is not a valid <see cref="Models.TransferType"/> value.</exception>
    [McpServerTool, Description("Update an existing transfer")]
    public async Task<string> UpdateTransferAsync(
        [Description("Transfer id")] Guid id,
        [Description("Amount (absolute, positive)")] decimal value,
        [Description("Transfer date and time (local)")] DateTime date,
        [Description("Optional short note (e.g., 'Rent')")] string description,
        [Description("Type: 'Income' or 'Expense' (case-insensitive)")] string type)
    {
        logger.LogInformation("UpdateTransferAsync called for Id={Id} with Value={Value}, Date={Date}, Description={Description}, Type={Type}",
            id, value, date, description, type);

        if (!Enum.TryParse<TransferType>(type, true, out var transferType))
        {
            logger.LogWarning("UpdateTransferAsync - invalid transfer type: {Type}", type);
            throw new ArgumentException($"Invalid transfer type: {type}. Valid types are 'Income' and 'Expense'.");
        }

        var transfer = await transferService.UpdateTransferAsync(id, value, date, description, transferType);
        logger.LogInformation("UpdateTransferAsync - transfer updated Id={Id}", transfer?.Id);
        return JsonSerializer.Serialize(transfer, MyFinanceContext.Default.Transfer);
    }

    /// <summary>
    /// Deletes the <see cref="Transfer"/> with the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The identifier of the transfer to delete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous delete operation.</returns>
    [McpServerTool, Description("Delete a transfer")]
    public async Task DeleteTransferAsync(
        [Description("Transfer id")] Guid id)
    {
        logger.LogInformation("DeleteTransferAsync called for Id={Id}", id);
        await transferService.DeleteTransferAsync(id);
        logger.LogInformation("DeleteTransferAsync completed for Id={Id}", id);
    }

    /// <summary>
    /// Gets transfers within the inclusive date range specified by <paramref name="from"/> and <paramref name="to"/>.
    /// </summary>
    /// <param name="from">The start date (inclusive).</param>
    /// <param name="to">The end date (inclusive).</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the collection of <see cref="Transfer"/> objects serialized as JSON.</returns>
    [McpServerTool, Description("Get transfers within a date range")]
    public async Task<string> GetTransfersFromDateRangeAsync(
        [Description("Start date (inclusive)")] DateTime from,
        [Description("End date (inclusive)")] DateTime to)
    {
        logger.LogInformation("GetTransfersFromDateRangeAsync called with From={From}, To={To}", from, to);
        var transfers = await transferService.GetTransfersFromDateRangeAsync(from, to);
        var count = transfers?.Count() ?? 0;
        logger.LogInformation("GetTransfersFromDateRangeAsync - retrieved {Count} transfers", count);
        return JsonSerializer.Serialize(transfers, MyFinanceContext.Default.IEnumerableTransfer);
    }
}

[JsonSerializable(typeof(IEnumerable<Transfer>))]
[JsonSerializable(typeof(Transfer))]
internal sealed partial class MyFinanceContext : JsonSerializerContext;