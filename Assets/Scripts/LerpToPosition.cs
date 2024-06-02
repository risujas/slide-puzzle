using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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
		yield return null;
	}
}
