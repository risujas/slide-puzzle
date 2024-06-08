using UnityEngine;

public class ToggleEnable : MonoBehaviour
{
	public void Toggle()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}
}
