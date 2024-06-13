using System.Collections;
using UnityEngine;

public class EnvironmentBackground : MonoBehaviour
{
	[SerializeField] private GameObject backgroundPrefab;

	private GameObject currentBackground;
	private GameObject previousBackground;
	private Coroutine currentFadeIn;

	public void SetBackground(Texture2D nextGraphic)
	{
		previousBackground = currentBackground;

		Sprite newSprite = Sprite.Create(nextGraphic, new Rect(0.0f, 0.0f, nextGraphic.width, nextGraphic.height), new Vector2(0.5f, 0.5f), 100.0f);
		currentBackground = Instantiate(backgroundPrefab, transform);
		SpriteRenderer renderer = currentBackground.GetComponent<SpriteRenderer>();
		renderer.sprite = newSprite;

		if (currentFadeIn != null)
		{
			StopCoroutine(currentFadeIn);
		}

		currentFadeIn = StartCoroutine(LerpAlpha(1f, 1f, renderer));

		if (previousBackground != null)
		{
			StartCoroutine(LerpAlpha(0f, 1f, previousBackground.GetComponent<SpriteRenderer>()));
			StartCoroutine(DestroyAfterTime(2f, previousBackground));
		}
	}

	private IEnumerator DestroyAfterTime(float time, GameObject obj)
	{
		yield return new WaitForSeconds(time);
		Destroy(obj);
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
