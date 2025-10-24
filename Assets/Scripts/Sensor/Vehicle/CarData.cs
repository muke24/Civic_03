//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityAndroidBluetooth;
//using UnityEngine;

//public class CarData : MonoBehaviour
//{
//	// ECU data variables
//	public float rpm;
//	public float coolantTemperature;
//	public float momentum;
//	public float engineLoad;
//	public float oilPressure;
//	public float speed;
//	public float steerRotation;

//	public float throttlePosition;
//	public float coolantTemp;
//	public float fuelLevel;

//	public bool foglightsOnly;
//	public bool foglights;
//	public bool lightOn;

//	public float brakeAxis;
//	public float accelAxis;

//	// Bluetooth
//	private BluetoothClient bluetoothClient;
//	private string targetDeviceName = "Your_OBD2_Reader_Name"; // Replace with your OBD2 reader's name

//	// Singleton
//	public static CarData instance { get; private set; }
//	private void Awake()
//	{
//		// If there is an instance, and it's not me, delete myself.

//		if (instance != null && instance != this)
//		{
//			Destroy(this);
//		}
//		else
//		{
//			instance = this;
//		}
//	}

//	#region Init
//	void Start()
//	{
//		bluetoothClient = new BluetoothClient();
//		bluetoothClient.Connected += OnDeviceConnected;
//		bluetoothClient.Disconnected += OnDeviceDisconnected;
//		bluetoothClient.MessageReceived += OnMessageReceived;
//		bluetoothClient.ConnectionLost += OnConnectionLost;

//		StartBluetoothProcess();
//	}

//	void StartBluetoothProcess()
//	{
//		if (!BluetoothClient.IsEnabled)
//		{
//			bluetoothClient.RequestEnableBluetooth();
//		}
//		else
//		{
//			ScanAndConnect();
//		}
//	}

//	void ScanAndConnect()
//	{
//		BluetoothClient.SearchDevices();
//		foreach (var device in BluetoothClient.GetDiscoveredDevices())
//		{
//			if (device.name == targetDeviceName)
//			{
//				bluetoothClient.Connect(device.address);
//				break;
//			}
//		}
//	}
//	#endregion

//	#region Connection
//	private void OnDeviceConnected(object sender, DeviceInfoEventArgs e)
//	{
//		Debug.Log("Connected to OBD2 reader: " + e.Device.name);
//		SendOBD2Command("ATZ"); // Example command to reset OBD2 device
//	}

//	private void OnDeviceDisconnected(object sender, System.EventArgs e)
//	{
//		Debug.Log("Disconnected from OBD2 reader");
//	}

//	private void OnConnectionLost(object sender, System.EventArgs e)
//	{
//		Debug.Log("Connection to OBD2 reader lost");
//	}
//	#endregion

//	#region Send/Recieve
//	private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
//	{
//		Debug.Log("Message received from OBD2 reader: " + e.Message);
//		// Here you can parse the received OBD2 data
//		ParseOBD2Data(e.Message);
//	}

//	public void SendOBD2Command(string command)
//	{
//		bluetoothClient.Send(command + "\r"); // Send command with carriage return
//	}
//	#endregion

//	#region Parsing Data
//	private void ParseOBD2Data(string data)
//	{
//		if (data.StartsWith("410C")) // RPM data
//		{
//			rpm = ParseRPMData(data);
//		}
//		else if (data.StartsWith("410D")) // Speed data
//		{
//			speed = ParseSpeedData(data);
//		}
//		else if (data.StartsWith("4111")) // Throttle Position
//		{
//			throttlePosition = ParseThrottlePosition(data);
//		}
//		else if (data.StartsWith("4105")) // Engine Coolant Temperature
//		{
//			coolantTemp = ParseCoolantTemperature(data);
//		}
//		else if (data.StartsWith("412F")) // Fuel Level Input
//		{
//			fuelLevel = ParseFuelLevel(data);
//		}
//		// Add more parsing logic for other data types
//	}

//	float ParseRPMData(string data)
//	{
//		// Assuming data is in the format "410C A B"
//		string[] tokens = data.Split(' ');
//		if (tokens.Length == 3)
//		{
//			int A = Convert.ToInt32(tokens[1], 16);
//			int B = Convert.ToInt32(tokens[2], 16);
//			return ((A * 256) + B) / 4.0f;
//		}
//		return 0;
//	}

//	float ParseSpeedData(string data)
//	{
//		// Assuming data is in the format "410D A"
//		string[] tokens = data.Split(' ');
//		if (tokens.Length == 2)
//		{
//			return Convert.ToInt32(tokens[1], 16);
//		}
//		return 0;
//	}

//	float ParseThrottlePosition(string data)
//	{
//		// Assuming data is in the format "4111 A"
//		string[] tokens = data.Split(' ');
//		if (tokens.Length == 2)
//		{
//			return (Convert.ToInt32(tokens[1], 16) * 100.0f) / 255.0f;
//		}
//		return 0;
//	}

//	float ParseCoolantTemperature(string data)
//	{
//		// Assuming data is in the format "4105 A"
//		string[] tokens = data.Split(' ');
//		if (tokens.Length == 2)
//		{
//			return Convert.ToInt32(tokens[1], 16) - 40.0f;
//		}
//		return 0;
//	}

//	float ParseFuelLevel(string data)
//	{
//		// Assuming data is in the format "412F A"
//		string[] tokens = data.Split(' ');
//		if (tokens.Length == 2)
//		{
//			return (Convert.ToInt32(tokens[1], 16) * 100.0f) / 255.0f;
//		}
//		return 0;
//	}
//	#endregion

//	void OnDestroy()
//	{
//		if (bluetoothClient != null)
//		{
//			bluetoothClient.Disconnect();
//		}
//	}
//}
