using JSLib;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WindowComponents;

public partial class Dialog
{
	#region 生命周期
	protected override async Task OnInitializedAsync()
	{
		_jsmodule = await JSModule.CreateAsync(_js, "./_content/WindowComponents/Dialog.razor.js");
		_jsop = await JSOp.CreateAsync(_js);
		_initTcs.SetResult();
	}
	#endregion

	private JSModule _jsmodule = default!;
	private JSOp _jsop = default!;
	private TaskCompletionSource _initTcs = new();
	private ElementReference _dialogElement;

	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	/// <summary>
	/// 显示对话框
	/// </summary>
	/// <returns></returns>
	public async Task ShowModal()
	{
		await _initTcs.Task;
		await _jsmodule.InvokeVoidAsync("showModal", _dialogElement);
	}

	/// <summary>
	/// 关闭对话框
	/// </summary>
	/// <returns></returns>
	public async Task Close()
	{
		await _initTcs.Task;
		await _jsmodule.InvokeVoidAsync("close", _dialogElement);
	}
}
