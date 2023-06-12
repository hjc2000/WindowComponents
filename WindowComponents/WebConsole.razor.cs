using Microsoft.JSInterop;

namespace WindowComponents;
public partial class WebConsole
{
	protected override async Task OnInitializedAsync()
	{
		module = await JS.InvokeAsync<IJSObjectReference>("import",
			"./_content/WindowComponents/WebConsole.razor.js");
	}

	public async Task LogAsync(string message)
	{
		await module.InvokeVoidAsync("log", message);
	}

	private IJSObjectReference module = default!;
}
