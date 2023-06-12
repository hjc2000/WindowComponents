using Microsoft.AspNetCore.Components;

namespace WindowComponents;
public partial class PCLayoutComponent
{
	[Parameter]
	public RenderFragment? LeftMenu { get; set; }

	[Parameter]
	public RenderFragment? TopBar { get; set; }

	[Parameter]
	public RenderFragment? Content { get; set; }
}
