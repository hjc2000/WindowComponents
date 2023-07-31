using JSLib;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WindowComponents;
public partial class PCLayoutComponent
{
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await ValueTask.CompletedTask;
		if (!firstRender) return;
		_jsop = new JSOp(_jsrt);
		_jsm = new JSModule(_jsrt, "./_content/WindowComponents/PCLayoutComponent.razor.js");
		_initTcs.SetResult();
	}

	private JSOp _jsop = default!;
	private JSModule _jsm = default!;
	private TaskCompletionSource _initTcs = new();

	/// <summary>
	/// 获取侧边栏的宽度
	/// </summary>
	/// <returns></returns>
	public async Task<int> Get_left_menu_width()
	{
		string width_string = await Get_left_menu_width_string();
		width_string = width_string[..(^2)];
		return int.Parse(width_string);
	}

	/// <summary>
	/// 设置侧边栏的宽度
	/// </summary>
	/// <returns></returns>
	public async Task Set_left_menu_width(int width)
	{
		await _initTcs.Task;
		await _jsm.InvokeVoidAsync("set_element_width_string", _left_menu_element, $"{width}px");
	}

	private async Task<string> Get_left_menu_width_string()
	{
		await _initTcs.Task;
		return await _jsm.InvokeAsync<string>("get_element_width_string", _left_menu_element);
	}

	private ElementReference _left_menu_element = default!;

	[Parameter]
	public RenderFragment? LeftMenu { get; set; }

	[Parameter]
	public RenderFragment? TopBar { get; set; }

	[Parameter]
	public RenderFragment? Content { get; set; }

	[Parameter]
	public RenderFragment? StateBar { get; set; }
}
