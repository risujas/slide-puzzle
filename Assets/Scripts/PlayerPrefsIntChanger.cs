using UnityEngine;

public class PlayerPrefsIntChanger : MonoBehaviour
{
	[SerializeField] private string key;
	[SerializeField] private int defaultValue;

	public void Set(int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	public int Get()
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}
}
