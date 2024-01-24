using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class which keeps texts in original positions so the parent mask may move freely without changing child positions
/// </summary>
public class TextMask : MonoBehaviour
{
    public Transform[] texts;
    public Transform[] unhighlightedTexts;
	public Vector3[] positions;

    public bool buttonPressed = false;

    void Start()
    {
		// List<Transform> textsWt = GetComponentsInChildren<Transform>().ToList();
		//textsWt.Remove(transform);
		//unhighlightedTexts = textsWt.ToArray();

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

		//if (buttonPressed)
  //      {
		//	for (int i = 0; i < texts.Length; i++)
		//	{
		//		texts[i].position = positions[i];

		//	}
		//}
    }
}
