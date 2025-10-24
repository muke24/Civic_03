using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarNotif : MonoBehaviour
{
	private bool _policeActive = false;
	private bool policeActive
	{
		get
		{
			return _policeActive;
		}
		set
		{
			_policeActive = value;
		}
	}

	private bool cameraActive = false;


	public Vector3 policePosition = Vector3.forward;
	public Vector3 cameraPosition = Vector3.forward;

	private float arrowRotation;
	private bool timerActive = false;
	private float timer = 5;

	void Update()
	{
		if (timerActive)
		{
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				timer = 5;
				timerActive = false;
				policeActive = false;
				cameraActive = false;
			}
		}
	}

	public void OnValueRecieved(string val)
	{
		if (!timerActive)
		{
			timerActive = true;
		}

		if (val.Contains("pol"))
		{
			policeActive = true;
		}
		else if (val.Contains("cam"))
		{
			cameraActive = false;
		}
	}

	void ActivateNotif()
	{

	}

	void DeactivateNotif()
	{

	}
}
