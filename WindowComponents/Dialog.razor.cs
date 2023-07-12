using JSLib;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WindowComponents;

public partial class Dialog
{
	public Dialog()
	{
		_oncloseCallbackHelper.Action = () =>
		{
			Console.WriteLine("弹窗关闭");
			DialogClosed?.Invoke();
		};
	}

	#region 生命周期
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_jsm = await JSModule.CreateAsync(_js, "./_content/WindowComponents/Dialog.razor.js");
			_dialogWrapper = await _jsm.InvokeAsync<IJSObjectReference>("DialogWrapper.create", _dialogElement, _oncloseCallbackHelper.DotNetHelper);
			_jsop = await JSOp.CreateAsync(_js);
			_initTcs.SetResult();
		}
	}
	#endregion

	private JSModule _jsm = default!;
	private JSOp _jsop = default!;
	private IJSObjectReference _dialogWrapper = default!;
	private TaskCompletionSource _initTcs = new();
	private ElementReference _dialogElement;
	private CallbackHelper _oncloseCallbackHelper = new();

	public event Action? DialogClosed;

	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	/// <summary>
	/// 显示对话框
	/// </summary>
	/// <returns></returns>
	public async Task ShowModal()
	{
		await _initTcs.Task;
		await _dialogWrapper.InvokeVoidAsync("showModal", _dialogElement);
	}

	/// <summary>
	/// 关闭对话框
	/// </summary>
	/// <returns></returns>
	public async Task Close()
	{
		await _initTcs.Task;
		await _dialogWrapper.InvokeVoidAsync("close", _dialogElement);
	}

	public async Task<bool> IsOpen()
	{
		await _initTcs.Task;
		return await _dialogWrapper.InvokeAsync<bool>("isOpened");
	}
}
