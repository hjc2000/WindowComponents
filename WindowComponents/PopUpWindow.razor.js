export function AddWindowEventListener(event, dotnetObj, methodName)
{
	window.addEventListener(event, (e) =>
	{
		e.preventDefault();
		dotnetObj.invokeMethodAsync(methodName, e.pageX, e.pageY);
	});
}