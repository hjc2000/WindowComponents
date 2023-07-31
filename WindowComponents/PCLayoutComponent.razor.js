/**
 * 设置指定的 HTML 元素的宽度
 * @param {HTMLElement} element
 */
export function set_element_width_string(element, width)
{
	element.style.width = width;
}

/**
 * 获取指定的 HTML 元素的宽度。返回的是字符串
 * @param {HTMLElement} element
 */
export function get_element_width_string(element)
{
	return window.getComputedStyle(element).width;
}