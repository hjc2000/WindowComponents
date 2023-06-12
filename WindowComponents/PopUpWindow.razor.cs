using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Reflection.Metadata;

namespace WindowComponents;

public partial class PopUpWindow : IDisposable
{
	#region 生命周期
	public PopUpWindow()
	{
		_dotnetObj = DotNetObjectReference.Create(this);
	}

	protected override async Task OnInitializedAsync()
	{
		moudel = await JS.InvokeAsync<IJSObjectReference>("import",
			"./_content/WindowComponents/PopUpWindow.razor.js");
		await moudel.InvokeVoidAsync("AddWindowEventListener", "mousemove", _dotnetObj, "OnMouseMove");
		await moudel.InvokeVoidAsync("AddWindowEventListener", "mouseup", _dotnetObj, "OnMouseUp");
	}

	public void Dispose()
	{
		_dotnetObj.Dispose();
	}
	#endregion

	#region 组件参数
	[Parameter]
	public RenderFragment? Content { get; set; }
	[Parameter]
	public RenderFragment? Title { get; set; }

	[Parameter]
	public bool Visible { get; set; }
	[Parameter]
	public EventCallback<bool> VisibleChanged { get; set; }

	[Parameter]
	public string Width { get; set; } = string.Empty;
	[Parameter]
	public string Height { get; set; } = string.Empty;

	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();
	#endregion

	#region js互操作
	private IJSObjectReference moudel = default!;
	private readonly DotNetObjectReference<PopUpWindow> _dotnetObj;

	[JSInvokable]
	public void OnMouseMove(double x, double y)
	{
		if (Visible && _allowMove)
		{
			CurrentMousePosX = x;
			CurrentMousePosY = y;
			StateHasChanged();
		}
	}

	[JSInvokable]
	public void OnMouseUp()
	{
		_allowMove = false;
	}
	private bool _allowMove = false;

	#endregion

	#region 鼠标位置参数
	//鼠标初始位置
	private double _currentMousePosX = 0;

	private double CurrentMousePosX
	{
		set
		{
			double offset = value - _currentMousePosX;
			_currentMousePosX = value;
			TranslateX += offset;
		}
	}

	private double _currentMousePosY = 0;

	private double CurrentMousePosY
	{
		set
		{
			double offset = value - _currentMousePosY;
			_currentMousePosY = value;
			Translate += offset;
		}
	}
	#endregion

	#region 窗口参数
	[Parameter]
	public double TranslateX { get; set; }
	[Parameter]
	public EventCallback<double> TranslateXChanged { get; set; }

	[Parameter]
	public double Translate { get; set; }
	[Parameter]
	public EventCallback<double> TranslateYChanged { get; set; }
	#endregion

	#region CSS
	//累计的偏移量
	private string CssTranslateX => $"{TranslateX}px";

	private string CssTranslateY => $"{Translate}px";
	#endregion

	#region DOM事件处理函数
	private void OnCloseClick()
	{
		if (Visible)
		{
			Visible = false;
		}
	}

	/// <summary>
	/// 记录鼠标按下时的鼠标位置，以后以这点为参考计算偏移
	/// </summary>
	/// <param name="e"></param>
	private void OnTitleMouseDown(MouseEventArgs e)
	{
		if (Visible && e.Buttons == 1)
		{
			_allowMove = true;
			_currentMousePosX = e.PageX;
			_currentMousePosY = e.PageY;
		}
	}

	#endregion
}
