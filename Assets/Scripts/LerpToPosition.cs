using System.Collections;
using UnityEngine;

public static class LerpToPosition
{
	public static IEnumerator Lerp(GameObject obj, Vector3 pos, float threshold, float speed)
	{
		float distance = Vector3.Distance(obj.transform.position, pos);
		while (distance > threshold)
		{
			if (obj == null)
			{
				yield break;
			}

			obj.transform.position = Vector3.Lerp(obj.transform.position, pos, Time.deltaTime * speed);

			distance = Vector3.Distance(obj.transform.position, pos);
			yield return null;
		}

		obj.transform.position = pos;
		yield return null;
	}
}
