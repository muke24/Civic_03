using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
	[SerializeField]
	private Transform cameraPivot;
	[SerializeField]
	private Image[] compassImages; // Array of 3 images
	[SerializeField]
	private float compassSmoothing = 0.5f;

	private float m_lastMagneticHeading = 0f;

	[SerializeField]
	private Image compassAvailImg;
	[SerializeField]
	private Image gyroAvailImg;

	void Start()
	{
		Permission.RequestUserPermission(Permission.FineLocation);
		Permission.RequestUserPermission(Permission.CoarseLocation);

		Input.location.Start();
		Input.compass.enabled = true;

		gyroAvailImg.color = SystemInfo.supportsGyroscope ? Color.green : Color.red;
		compassAvailImg.color = Input.compass.enabled ? Color.green : Color.red;

		//Debug.Log("Gryo On: " + SystemInfo.supportsGyroscope);
		//Debug.Log("Compass On: " + Input.compass.enabled);
	}

	void Update()
	{
		//do rotation based on compass
		float currentMagneticHeading = (float)Math.Round(AlertReciever.Instance.CurrentDirection, 2);
		if (m_lastMagneticHeading < currentMagneticHeading - compassSmoothing || m_lastMagneticHeading > currentMagneticHeading + compassSmoothing)
		{
			m_lastMagneticHeading = currentMagneticHeading;
		}

		float rot = m_lastMagneticHeading + cameraPivot.rotation.eulerAngles.y;
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
