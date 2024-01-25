using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
	[SerializeField]
	private Transform cameraPivot;
	[SerializeField]
	private Image[] compassImages; // Array of 3 images

	void Update()
	{
		float rot = Input.compass.trueHeading + cameraPivot.rotation.eulerAngles.y;
		float offset = rot / 360f * 2500; // Calculate offset for images

		for (int i = 0; i < 3; i++)
		{
			// Move each image
			Vector3 position = compassImages[i].transform.localPosition;
			position.x = i * 2500 - offset;

			// Check if the image has moved off-screen and reposition it
			if (position.x < -2500)
			{
				position.x += 2500 * 3;
			}
			else if (position.x > 2500 * 2)
			{
				position.x -= 2500 * 3;
			}

			compassImages[i].transform.localPosition = position;
		}
	}
}
