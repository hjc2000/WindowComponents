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
		async Task init_width_style()
		{
			/* 初始化的时候先获取当前的计算样式的宽度，然后设置一遍，将计算样式应用到内联样式中
			 * 如果不这么做，宽度变化的过渡效果不会生效。想要生效必须初始时有明确设置一个固定宽度。
			 */
			_init_left_menu_width = await Get_left_menu_width();
			await Set_left_menu_width(_init_left_menu_width);
		}

		await init_width_style();
	}

	private JSOp _jsop = default!;
	private JSModule _jsm = default!;
	private TaskCompletionSource _initTcs = new();

	/// <summary>
	/// 左侧边栏初始的宽度
	/// </summary>
	private int _init_left_menu_width = 0;

	private async Task On_hide_left_menu_button_click()
	{
		int width = await Get_left_menu_width();
		if (width > 0)
		{
			await Set_left_menu_width(0);
		}
		else
		{
			await Set_left_menu_width(_init_left_menu_width);
		}
	}

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

	/// <summary>
	/// 用于隐藏左侧边栏的按钮。如果设置了本参数，则内置的按钮将被禁用，转而使用这个
	/// </summary>
	[Parameter]
	public RenderFragment? Hide_left_menu_button { get; set; }
}
