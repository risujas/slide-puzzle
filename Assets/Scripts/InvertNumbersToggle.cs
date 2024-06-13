using TMPro;
using UnityEngine;

public class InvertNumbersToggle : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private PlayerPrefsIntChanger changer;

	private bool invertNumbers = false;

	public void Toggle()
	{
		invertNumbers = !invertNumbers;
		text.text = invertNumbers ? "On" : "Off";

		int value = invertNumbers ? 1 : 0;
		changer.Set(value);
	}
}
