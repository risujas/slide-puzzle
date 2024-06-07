using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private bool isMouseOver = false;

	public bool WasClicked { get; private set; }

	public void OnPointerEnter(PointerEventData eventData)
	{
		isMouseOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isMouseOver = false;
	}

	private void Update()
	{
		WasClicked = false;

		if (isMouseOver)
		{
			if (Input.GetMouseButtonUp(0))
			{
				WasClicked = true;
			}
		}
	}
}
