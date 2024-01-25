using System.Collections;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
	[SerializeField]
	private float moveSpeed;
	[SerializeField]
	private Vector2[] buttonPositions;
	[SerializeField]
	private RectTransform highlightTransform;

	[SerializeField]
	private RectTransform[] highlightedTexts;
	[SerializeField]
	private Vector2 multiplier;
	private bool isMoving;

	//void Awake()
	//{
	//	highlightTransform.anchoredPosition = buttonPositions[0];
	//}

	public void Home()
	{
		MoveHighlight(0);
	}

	public void Lighting()
	{
		MoveHighlight(1);
	}

	public void Vehicle()
	{
		MoveHighlight(2);
	}

	void MoveHighlight(int i)
	{
		if (!isMoving)
		{
			StartCoroutine(LerpHighlight(buttonPositions[i]));
		}
	}

	IEnumerator LerpHighlight(Vector2 targetPosition)
	{
		isMoving = true;

		while (Vector2.Distance(highlightTransform.anchoredPosition, targetPosition) > 0.01f)
		{
			highlightTransform.anchoredPosition = Vector2.Lerp(highlightTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
			for (int i = 0; i < highlightedTexts.Length; i++)
			{
				highlightedTexts[i].anchoredPosition = buttonPositions[i] * multiplier;
			}
			yield return null;
		}

		highlightTransform.anchoredPosition = targetPosition; // Ensure it's exactly at the target position
		isMoving = false;
	}
}
