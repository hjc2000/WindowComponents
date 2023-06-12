using Microsoft.JSInterop;

namespace WindowComponents;

public partial class DisableZoom
{
	private IJSObjectReference moudel = default!;
	protected override async Task OnInitializedAsync()
	{
		moudel = await JS.InvokeAsync<IJSObjectReference>("import",
		"./_content/WindowComponents/DisableZoom.razor.js");
		await moudel.InvokeVoidAsync("DisableZoom");
	}
}
