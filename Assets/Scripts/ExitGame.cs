using UnityEngine;

public class ExitGame : MonoBehaviour
{
	public void Exit()
	{
		Debug.Log("Application.Quit");
		Application.Quit();
	}
}
