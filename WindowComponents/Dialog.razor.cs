using JSLib;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WindowComponents;

public partial class Dialog : IAsyncDisposable
{
	#region 生命周期
	public Dialog()
	{
		_onclose_callback_helper.CallbackAction = async () =>
		{
			Console.WriteLine("弹窗关闭");
			await DialogClosed.InvokeAsync();
		};
	}

	~Dialog()
	{
		_ = DisposeAsync();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_jsm = new JSModule(_jsrt, "./_content/WindowComponents/Dialog.razor.js");
			_dialogWrapper = await _jsm.InvokeAsync<IJSObjectReference>("DialogWrapper.create", _dialogElement, _onclose_callback_helper.DotNetHelper);
			_initTcs.SetResult();
		}
	}

	private bool _disposed = false;
	public async ValueTask DisposeAsync()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		GC.SuppressFinalize(this);

		Console.WriteLine("Dialog 释放");
		await _jsm.DisposeAsync();
		await _dialogWrapper.DisposeAsync();
		_onclose_callback_helper.Dispose();
	}
	#endregion

	private JSModule _jsm = default!;
	private IJSObjectReference _dialogWrapper = default!;
	private TaskCompletionSource _initTcs = new();
	private ElementReference _dialogElement;
	private CallbackHelper _onclose_callback_helper = new();

	[Parameter]
	public EventCallback DialogClosed { get; set; }

	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	/// <summary>
	/// 显示对话框
	/// </summary>
	/// <returns></returns>
	public async Task ShowModalAsync()
	{
		await _initTcs.Task;
		await _dialogWrapper.InvokeVoidAsync("showModal", _dialogElement);
	}

	/// <summary>
	/// 关闭对话框
	/// </summary>
	/// <returns></returns>
	public async Task CloseAsync()
	{
		await _initTcs.Task;
		await _dialogWrapper.InvokeVoidAsync("close", _dialogElement);
	}

	public async Task<bool> IsOpened()
	{
		await _initTcs.Task;
		return await _dialogWrapper.InvokeAsync<bool>("isOpened");
	}
}
