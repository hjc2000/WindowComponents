export class HiddenFunction
{
	static create(callbackHelper)
	{
		return new HiddenFunction(callbackHelper);
	}

	constructor(callbackHelper)
	{
		this.callbackHelper = callbackHelper;
		window.hf = () =>
		{
			this.callbackHelper.invokeMethodAsync("Invoke");
			return true;
		};
	}
}

