using System.Collections;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
	[SerializeField]
	private float sizeMultiplier = 2f;
	[SerializeField]
	private float sizeTime = 1f;
	[SerializeField]
	private float backplier = 1f; // Backwards multiplier
	[SerializeField]
	private float forplier = 1f; // Forwards multiplier

	private enum CurrentPage { Home, Lighting, Settings }
	private CurrentPage currentPage = CurrentPage.Home;

	[SerializeField]
	private RectTransform buttonHighlight;
	[SerializeField]
	private Vector3[] buttonPositions;

	private Coroutine highlightCoroutine;
	private Vector2 originalHighlightSize;

	private static readonly float[] sineTable;
	private static readonly int sineTableSize = 1000;

	static ButtonPanel()
	{
		sineTable = new float[sineTableSize];
		for (int i = 0; i < sineTableSize; i++)
		{
			sineTable[i] = Mathf.Sin(i * Mathf.PI * 2 / sineTableSize);
		}
	}

	void Start()
	{
		originalHighlightSize = buttonHighlight.sizeDelta;
	}

	public void OnHomePress()
	{
		OnButtonPress(CurrentPage.Home);
	}

	public void OnLightingPress()
	{
		OnButtonPress(CurrentPage.Lighting);
	}

	public void OnVehiclePress()
	{
		OnButtonPress(CurrentPage.Settings);
	}

	private void OnButtonPress(CurrentPage page)
	{
		CurrentPage newPage = page;
		if (currentPage != newPage)
		{
			currentPage = newPage;
			MoveHighlightToPage(newPage);
		}
	}

	private void MoveHighlightToPage(CurrentPage page)
	{
		int pageIndex = (int)page;
		if (pageIndex < 0 || pageIndex >= buttonPositions.Length)
		{
			Debug.LogError("Page index is out of range: " + pageIndex);
			return;
		}

		if (highlightCoroutine != null)
		{
			StopCoroutine(highlightCoroutine);
		}
		highlightCoroutine = StartCoroutine(MoveHighlight(buttonPositions[pageIndex]));
	}

	private IEnumerator MoveHighlight(Vector3 position)
	{
		Vector3 startPosition = buttonHighlight.position;
		float timeElapsed = 0f;

		while (timeElapsed < sizeTime)
		{
			float progress = timeElapsed / sizeTime;
			buttonHighlight.position = CustomLerp(startPosition, position, progress);

			float scale = SineTableLookup(progress * sineTableSize) * (progress < 0.5f ? forplier : backplier);
			buttonHighlight.sizeDelta = originalHighlightSize + new Vector2(scale * sizeMultiplier, 0);

			timeElapsed += Time.deltaTime;
			yield return null;
		}

		buttonHighlight.position = position;
		buttonHighlight.sizeDelta = originalHighlightSize;
	}

	private static Vector3 CustomLerp(Vector3 a, Vector3 b, float t)
	{
		return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
	}

	private static float SineTableLookup(float index)
	{
		int i = Mathf.Clamp((int)index, 0, sineTableSize - 1);
		return sineTable[i];
	}
}
