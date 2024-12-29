using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class BaseForm : ComponentBase
{
	[Parameter]
	public string? FormErrorMessage { get; set; }

	[Parameter]
	public Color ThemeColor { get; set; } = Color.Primary;

	[Parameter]
	public EventCallback<MouseEventArgs> OnReset { get; set; }

	[Parameter]
	public RenderFragment? MoreActions { get; set; }

	[Parameter]
	public RenderFragment? Information { get; set; }

	[Parameter]
	public string? Title { get; set; }

	[Parameter]
	public required string FormName { get; set; }

	[Parameter]
	public string? SubmitText { get; set; }

	[Parameter]
	public string? ResetText { get; set; } = "Reset";

	[Parameter]
	public string? Icon { get; set; }

	[Parameter]
	public string? IconTitle { get; set; }

	[Parameter]
	public bool ShowReset { get; set; }

	[Parameter]
	public bool HideSubmit { get; set; }

	[Parameter]
	public bool Square { get; set; }

	[Parameter]
	public Func<Task>? OnSubmit { get; set; }

	[Parameter]
	public RenderFragment? Fields { get; set; }

	[Parameter]
	public bool IsLoading { get; set; }

	protected bool Loading;

	protected async Task HandleSubmit()
	{
		Loading = true;
		await OnSubmit!.Invoke();
		Loading = false;
	}
}

public class BaseForm<TModel> : BaseForm
{
	[Parameter]
	public TModel? Model { get; set; }
}