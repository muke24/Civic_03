using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using brab.bluetooth;
using System.Threading; // Use the new Bluetooth namespace

public class BluetoothConnector : MonoBehaviour
{
	private BluetoothAdapter bluetoothAdapter;
	private BluetoothSocket bluetoothSocket;
	private bool connected = false;
	private Thread dataReceiveThread;
	private bool isReceivingData = false;
	private bool isCheckingConnection = false;

	[SerializeField, Tooltip("Raspberry Pi Bluetooth Address")]
	private string bluetoothAddress = "B8:27:EB:D5:50:14";
	[SerializeField, Tooltip("idk look it up")]
	private string serviceUUID = "00001101-0000-1000-8000-00805F9B34FB"; // Example UUID
	[SerializeField]
	private float connectionAttemptRate = 1f;

	[SerializeField]
	private Image debugImg;

#if !UNITY_EDITOR
	void Start()
	{
		bluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
		StartCoroutine(TryConnect());

		// Start the data receiving thread
		isReceivingData = true;
		dataReceiveThread = new Thread(DataReceiveThread);
		dataReceiveThread.Start();

		// Start checking the connection status
        isCheckingConnection = true;
        StartCoroutine(CheckConnectionStatus());
	}
#endif

	IEnumerator CheckConnectionStatus()
	{
		while (isCheckingConnection)
		{
			if (!connected || bluetoothSocket == null)
			{
				HandleConnectionLost();
				break; // Exit the loop if the connection is lost
			}

			// Optionally, you can check the socket's state here
			// Implement additional checks as per your requirement

			yield return new WaitForSeconds(1); // Check every second, adjust as needed
		}
	}

	private void HandleConnectionLost()
	{
		Debug.LogError("Connection lost!");

		// Update UI or state
		if (debugImg != null)
		{
			debugImg.color = Color.red;
		}

		// Reset connection flag
		connected = false;

		// Reconnect or notify the user
		// Implement your reconnect logic here or notify the user of the connection loss
		// You can call TryConnect() again or provide an option to reconnect manually
	}

	IEnumerator TryConnect()
	{
		while (!connected)
		{
			var devices = bluetoothAdapter.getBondedDevices();
			foreach (BluetoothDevice device in devices)
			{
				Debug.Log(device.getName() + " | " + device.getAddress());

				if (device.getAddress() == bluetoothAddress)
				{
					Debug.Log("Raspberry Pi found! Attempting to connect...");
					UUID uuid = UUID.fromString(serviceUUID);
					bluetoothSocket = device.createRfcommSocketToServiceRecord(uuid);
					bluetoothSocket.connect();
					connected = true;
					break;
				}
			}

			if (!connected)
			{
				yield return new WaitForSeconds(connectionAttemptRate);
			}
		}
	}

	//void Update()
	//{
	//	if (connected)
	//	{
	//		// Handle data communication, e.g., reading from the Bluetooth socket
	//		// Implement your own logic to read data from bluetoothSocket.getInputStream()
	//	}
	//}

	private void DataReceiveThread()
	{
		while (isReceivingData)
		{
			if (connected && bluetoothSocket != null)
			{
				try
				{
					BtStream inputStream = bluetoothSocket.getInputStream();
					byte[] buffer = new byte[1024];
					int bytesRead = inputStream.Read(buffer, 0, buffer.Length);
					if (bytesRead > 0)
					{
						string receivedString = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
						ProcessReceivedString(receivedString);
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("Error in DataReceiveThread: " + ex.Message);
				}
			}
			Thread.Sleep(100); // Sleep to prevent busy waiting
		}
	}

	private void ProcessReceivedString(string data)
	{
		if (float.TryParse(data, out float receivedValue))
		{
			Debug.Log("Received float value: " + receivedValue);
			// Handle the received float value as needed
		}
	}

	void OnDestroy()
	{
		if (bluetoothSocket != null)
		{
			bluetoothSocket.close();
		}

		// Stop the data receiving thread
		isReceivingData = false;
		if (dataReceiveThread != null && dataReceiveThread.IsAlive)
		{
			dataReceiveThread.Abort();
		}

		// Stop checking the connection status
		isCheckingConnection = false;
	}
}