using JSLib;
using Microsoft.JSInterop;

namespace WindowComponents;
public partial class HiddenFunction : IAsyncDisposable
{
	public HiddenFunction()
	{
		_showHiddenFunctionCallback.CallbackAction = () =>
		{
			ShouldShowHiddenFunction = true;
		};
	}

	protected override async Task OnInitializedAsync()
	{
		Console.WriteLine("hf 初始化");
		_jsm = new(_jrt, "./_content/WindowComponents/HiddenFunction.razor.js");
		_js_HiddenFunction = await _jsm.InvokeAsync<IJSObjectReference>("HiddenFunction.create", _showHiddenFunctionCallback.DotNetHelper);
	}

	private bool _disposed = false;
	public async ValueTask DisposeAsync()
	{
		if (_disposed) { return; }

		_disposed = true;
		GC.SuppressFinalize(this);

		if (_jsm != null)
		{
			await _jsm.DisposeAsync();
		}

		if (_js_HiddenFunction != null)
		{
			await _js_HiddenFunction.DisposeAsync();
		}
	}

	#region 隐藏功能
	private JSModule _jsm = default!;

	/// <summary>
	/// js 的 HiddenFunction 类的对象的引用
	/// </summary>
	private IJSObjectReference _js_HiddenFunction = default!;
	private CallbackHelper _showHiddenFunctionCallback = new();

	private static bool _hiddenFunction = false;
	public static bool ShouldShowHiddenFunction
	{
		get
		{
			return _hiddenFunction;
		}

		set
		{
			_hiddenFunction = value;
			Console.WriteLine("显示隐藏功能");
			ShouldShowHiddenFunctionChanged?.Invoke();
		}
	}

	/// <summary>
	/// HiddenFunction 属性改变事件
	/// </summary>
	public static event Action? ShouldShowHiddenFunctionChanged;
	#endregion
}
