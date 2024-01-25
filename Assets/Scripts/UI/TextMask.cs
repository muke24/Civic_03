using UnityEngine;

public class TextMask : MonoBehaviour
{
    public Transform[] texts;
    public Transform[] unhighlightedTexts;
	public Vector3[] positions;

    private bool buttonPressed = false;

    void Awake()
    {
		positions = new Vector3[unhighlightedTexts.Length];
		for (int i = 0; i < unhighlightedTexts.Length; i++)
		{
			positions[i] = unhighlightedTexts[i].transform.position;
		}
		transform.position = positions[0];
	}

	// Update is called once per frame
	void Update()
    {
		if (positions[0] == Vector3.zero)
		{
			positions = new Vector3[unhighlightedTexts.Length];
			for (int i = 0; i < unhighlightedTexts.Length; i++)
			{
				positions[i] = unhighlightedTexts[i].transform.position;
			}
			transform.position = positions[0];
		}

		for (int i = 0; i < texts.Length; i++)
		{
			texts[i].position = positions[i];
		}
    }
}
