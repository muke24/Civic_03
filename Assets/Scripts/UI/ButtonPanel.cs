using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    [SerializeField]
    private float buttonSmoothing = 5f;
    [SerializeField]
    private float sizeMultiplier = 2f;
	[SerializeField]
	private float sizeTime = 1f;
	[SerializeField]
	private float backplier = 1f;
	[SerializeField]
	private float forplier = 1f;

	private enum CurrentPage { Home = 0, Lighting = 1, Settings = 2}
    private CurrentPage currentPage = CurrentPage.Home;
	private int lastPage = 0;
	Vector2 ogSize = Vector2.zero;

	[SerializeField]
	private Transform buttonHighlight;
    [SerializeField]
    private TextMask buttonPositions;

    private Vector3 highlightScale;

    void Start()
    {
        highlightScale = buttonHighlight.localScale;

	}


    public void HomeButton()
    {
        lastPage = (int)currentPage;
		currentPage = CurrentPage.Home;
        OnButtonPress((int)currentPage);
    }

    public void LightingButton()
    {
		lastPage = (int)currentPage;
		currentPage = CurrentPage.Lighting;
		OnButtonPress((int)currentPage);

	}

	public void SettingsButton()
    {
		lastPage = (int)currentPage;
		currentPage = CurrentPage.Settings;
		OnButtonPress((int)currentPage);

	}

	void OnButtonPress(int button)
    {
        StopAllCoroutines();

        StartCoroutine(MoveHighlight(buttonPositions.positions[button]));
    }

	IEnumerator MoveHighlight(Vector3 position)
	{
		RectTransform highlightRect = buttonHighlight.GetComponent<RectTransform>();
		Vector2 originalSize = highlightRect.sizeDelta;
		Vector3 startPosition = buttonHighlight.position;

		float timeElapsed = 0f;
		while (timeElapsed < sizeTime)
		{
			// Move the highlight towards the target position
			buttonHighlight.position = Vector3.Lerp(startPosition, position, timeElapsed / sizeTime);

			// Determine scale factor based on the phase of the animation
			float progress = timeElapsed / sizeTime;
			float scale;
			if (progress < 0.5f) // First half of the animation
			{
				// Scale up using forplier
				scale = Mathf.Sin(progress * Mathf.PI) * forplier;
			}
			else // Second half of the animation
			{
				// Scale down using backplier
				scale = Mathf.Sin(progress * Mathf.PI) * backplier;
			}
			highlightRect.sizeDelta = originalSize + new Vector2(scale, 0) * sizeMultiplier;

			timeElapsed += Time.deltaTime;
			yield return null;
		}

		// Set the highlight to the target position and original size at the end of the animation
		buttonHighlight.position = position;
		highlightRect.sizeDelta = originalSize;
	}

}
