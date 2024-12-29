using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Sienar.Infrastructure;

namespace Sienar.Layouts;

public class NavBase : ComponentBase, IDisposable
{
	private bool _disposed;

	protected List<List<MenuLink>> Menus = [];

	[Parameter]
	public required IEnumerable<string> MenuNames { get; set; }

	[Inject]
	protected IMenuGenerator MenuGenerator { get; set; } = null!;

	[Inject]
	protected AuthenticationStateProvider AuthState { get; set; } = null!;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		base.OnInitialized();

		AuthState.AuthenticationStateChanged += UpdateMenuAndRender;
		UpdateMenuAndRender(AuthState.GetAuthenticationStateAsync());
	}

	private async void UpdateMenuAndRender(Task<AuthenticationState> s)
	{
		await s;
		Menus.Clear();
		foreach (var name in MenuNames)
		{
			Menus.Add(await MenuGenerator.Create(name));
		}
		StateHasChanged();
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (_disposed || !disposing) return;

		AuthState.AuthenticationStateChanged -= UpdateMenuAndRender;
		_disposed = true;
	}
}