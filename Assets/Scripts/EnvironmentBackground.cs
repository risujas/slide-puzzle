using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBackground : MonoBehaviour
{
	[SerializeField] private GameObject background1;
	[SerializeField] private GameObject background2;

	private Texture2D previousGraphic;
	private bool useFirstBackground = true;
	private List<Coroutine> activeCoroutines = new List<Coroutine>();

	public void SetBackground(Texture2D nextGraphic)
	{
		Sprite newSprite = Sprite.Create(nextGraphic, new Rect(0.0f, 0.0f, nextGraphic.width, nextGraphic.height), new Vector2(0.5f, 0.5f), 100.0f);

		foreach (var c in activeCoroutines)
		{
			StopCoroutine(c);
		}
		activeCoroutines.Clear();

		if (useFirstBackground)
		{
			background1.GetComponent<SpriteRenderer>().sprite = newSprite;
			activeCoroutines.Add(StartCoroutine(LerpAlpha(1f, 1f, background1.GetComponent<SpriteRenderer>())));
			activeCoroutines.Add(StartCoroutine(LerpAlpha(0f, 1f, background2.GetComponent<SpriteRenderer>())));
		}
		else
		{
			background2.GetComponent<SpriteRenderer>().sprite = newSprite;
			activeCoroutines.Add(StartCoroutine(LerpAlpha(0f, 1f, background1.GetComponent<SpriteRenderer>())));
			activeCoroutines.Add(StartCoroutine(LerpAlpha(1f, 1f, background2.GetComponent<SpriteRenderer>())));
		}

		useFirstBackground = !useFirstBackground;
		previousGraphic = nextGraphic;
	}

	private IEnumerator LerpAlpha(float target, float time, SpriteRenderer renderer)
	{
		float startAlpha = renderer.color.a;
		float elapsedTime = 0f;

		while (elapsedTime < time)
		{
			float alpha = Mathf.Lerp(startAlpha, target, elapsedTime / time);
			Color newColor = renderer.color;
			newColor.a = alpha;
			renderer.color = newColor;

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Color finalColor = renderer.color;
		finalColor.a = target;
		renderer.color = finalColor;
	}
}
