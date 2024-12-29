using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sienar.Extensions;
using Sienar.Data;
using Sienar.Services;
using Sienar.Ui;

namespace Sienar.Pages;

public class TablePage<TEntity> : ComponentBase
	where TEntity : EntityBase
{
	[Inject]
	protected IEntityReader<TEntity> Reader { get; set; } = null!;

	[Inject]
	protected IEntityDeleter<TEntity> Deleter { get; set; } = null!;

	[Inject]
	protected IDialogService DialogService { get; set; } = null!;

	protected Table<TEntity> Table = null!;

	protected Task DeleteEntity(
		Guid id,
		string? title = null,
		string? question = null)
	{
		var entityName = typeof(TEntity).GetEntityName();
		title ??= $"Delete {entityName}";
		question ??= $"Are you sure you want to delete this {entityName}? This cannot be undone!";

		return ConfirmAction(
			title,
			question,
			() => Deleter.Delete(id),
			mainColor: Color.Error,
			cancelColor: Color.Primary);
	}

	protected async Task ConfirmAction<T>(
		string title,
		string question,
		Func<Task<OperationResult<T>>> action,
		string confirmText = "Yes",
		string cancelText = "No",
		Color mainColor = Color.Primary,
		Color cancelColor = Color.Secondary)
	{
		var shouldAct = await DialogService.Confirm(
			title,
			question,
			confirmText,
			cancelText,
			mainColor,
			cancelColor);

		if (shouldAct && (await action()).Status == OperationStatus.Success)
		{
			await Table.ReloadTable();
		}
	}
}