using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class AlertReciever : Singleton<AlertReciever>
{
    [Header("Settings")] 
    [SerializeField] private string baseUrl = "https://www.waze.com/live-map/api/georss";
    [Space] 
    [SerializeField, Range(0.1f, 100f), Tooltip("Max distance for alerts. \n(kilometers)")]
    private float maxDistanceKm = 1f;
    [SerializeField, Range(1f, 60f), Tooltip("How long it takes to check for new alerts. \n(seconds)")]
    private float checkInterval = 60f;
    [SerializeField, Range(0.1f, 1f), Tooltip("Distance travelled to trigger an alert check. Overrides checkInterval. \n(kilometers)")]
    private float movementThreshold = 0.2f;

    [Header("Navigation")]
    [SerializeField] private bool simulateLocation = false;
    [SerializeField] private Location simulatedLocation;
    [Space] 
    [SerializeField] private bool simulateDirection = false;
    [SerializeField] private float simulatedDirection;
    [Space]
    
    [Header("Alerts")] 
    [SerializeField] private Alert[] currentAlerts;
    
    // Location tracking
    private Location currentLocation; // Current lat/lon from GPS
    private Location lastCheckedLocation; // Last location where data was fetched
    private float timeSinceLastCheck = 0f; // Timer for periodic checks
    private bool isLocationInitialized = false;
    private float currentDirection; // This is our direction with the main camera offset.
    private float currentRawDirection; // This is our direction without the main camera offset.
    
    private bool fetchingData = false;

    public Alert[] CurrentAlerts => currentAlerts;
    public Location CurrentLocation => currentLocation;
    public float CurrentDirection => currentDirection;
    public float MaxDistance => maxDistanceKm;
    
    void Start()
    {
        if (!simulateLocation)
        {
            // Location logic ignored
            Debug.LogError("Make sure to simulate location in the inspector.");
        }
        else
        {
            // Simulated location.
            currentLocation = simulatedLocation;
            lastCheckedLocation = currentLocation;
            isLocationInitialized = true;
            StartCoroutine(FetchPoliceData());
        }
    }

    void Update()
    {
        // If the location has not yet initialised, don't do anything.
        if (!isLocationInitialized) return;

        // Update timer
        timeSinceLastCheck += Time.deltaTime;

        if (simulateLocation)
        {
            // Simulated location.
            currentLocation = simulatedLocation;
        }
        else
        {
            // Raspberry Pi logic.
        }

        if (simulateDirection)
        {
            currentDirection = simulatedDirection;
        }
        else
        {
            Debug.LogError("Make sure to simulate direction in the inspector.");
            // Raspberry Pi logic.
        }

        // Check if it's time to fetch data (every minute or >200m movement)
        float distanceMoved = GpsHelper.CalculateDistance(currentLocation, lastCheckedLocation);
        if (timeSinceLastCheck >= checkInterval || distanceMoved > movementThreshold && !fetchingData)
        {
            StartCoroutine(FetchPoliceData());
            lastCheckedLocation = currentLocation; // Update last checked location
            timeSinceLastCheck = 0f; // Reset timer
        }
    }

    private IEnumerator FetchPoliceData()
    {
        fetchingData = true;
        
        // Build URL
        BoundingArea locationArea = BoundingBox(currentLocation, maxDistanceKm);

        string url = $"{baseUrl}?top={locationArea.top}&bottom={locationArea.bottom}&left={locationArea.left}&right={locationArea.right}&env=row&types=alerts";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                string json = request.downloadHandler.text;
                
                WazeResponse data = JsonUtility.FromJson<WazeResponse>(json);

                if (data == null || data.alerts == null || data.alerts.Length == 0)
                {
                    Debug.Log("No alerts found.");
                    currentAlerts = null;
                }
                else
                {
                    OnAlertsReceived(data.alerts);
                }
            }
        }
    }

    void OnAlertsReceived(Alert[] alerts)
    {
        fetchingData = false;
        
        // Store all received alerts
        currentAlerts = alerts;
    }

    /// <summary>
    /// Creates a bounding area around a location with a given distance in kilometers.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="distanceInKm"></param>
    /// <returns></returns>
    public BoundingArea BoundingBox(Location location, float distanceInKm)
    {
        float latitude = location.y;    // Correct: latitude
        float longitude = location.x;   // Correct: longitude

        float latInRadians = latitude * Mathf.Deg2Rad;

        float deltaLatitude = distanceInKm / 111f;
        float deltaLongitude = distanceInKm / (111f * Mathf.Cos(latInRadians));

        float left = longitude - deltaLongitude;
        float bottom = latitude - deltaLatitude;
        float right = longitude + deltaLongitude;
        float top = latitude + deltaLatitude;

        return new BoundingArea(left, bottom, right, top);
    }

    public struct BoundingArea
    {
        public float top;
        public float bottom;
        public float left;
        public float right;

        public BoundingArea(float left, float bottom, float right, float top)
        {
            this.left = left;
            this.bottom = bottom;
            this.right = right;
            this.top = top;
        }
    }
    
    // Data structure for Waze JSON response
    [Serializable]
    private class WazeResponse
    {
        public Alert[] alerts;
    }
}


[Serializable]
public class Alert
{
    public string type;
    public string subtype;
    public Location location;
    public string street;
}

[Serializable]
public class Location
{
    [Header("Longitude")] public float x;
    [Header("Latitude")] public float y;

    /// <summary>
    /// Initialise a location with longitude and latitude.
    /// </summary>
    /// <param name="longitude">Longitude</param>
    /// <param name="latitude">Latitude</param>
    public Location(float longitude, float latitude)
    {
        this.x = longitude;
        this.y = latitude;
    }
}