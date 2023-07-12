export class DialogWrapper
{
	/**
	 * 
	 * @param {any} dialogElement
	 * @param {object} dotnetHelper
	 * @returns
	 */
	static create(dialogElement, oncloseCallbackHelper)
	{
		return new DialogWrapper(dialogElement, oncloseCallbackHelper);
	}

	/**
	 * 
	 * @param {HTMLDialogElement} dialogElement
	 */
	constructor(dialogElement, oncloseCallbackHelper)
	{
		this.dialogElement = dialogElement;
		this.oncloseCallbackHelper = oncloseCallbackHelper;
		this.dialogElement.onclose = () =>
		{
			this.__onclose();
		}
	}

	showModal()
	{
		this.dialogElement.showModal();
	}

	close()
	{
		this.dialogElement.close();
	}

	isOpened()
	{
		return this.dialogElement.open;
	}

	__onclose()
	{
		this.oncloseCallbackHelper.invokeMethodAsync("Invoke");
	}
}