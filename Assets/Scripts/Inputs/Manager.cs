using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{
	public enum GearType { Drive, Sport }
	public enum Gear { Auto = -2, Reverse = -1, Neutral = 0, First = 1, Second = 2, Third = 3, Forth = 4, Fifth = 5, Sixth = 6, Seventh = 7 }

	public bool IsConnected => isConnected;

	//
	private bool isConnected = false;
	private bool metricUnits = true;
	private float compassAngle = 0;
	private float speed = 0;
	private float steeringAngle = 0;
	private GearType gearType = GearType.Drive;
	private Gear gear = Gear.Auto;


	void ApplyConnection(bool isConnected)
	{
		this.isConnected = isConnected;
	}
}
