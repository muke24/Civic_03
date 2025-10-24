using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    [SerializeField] private int maxLeds = 11;

    [SerializeField, Tooltip("Multiplier for the flash speed")]
    private float flashSpeed = 1;
    [SerializeField, Tooltip("Multiplier for distance which affects the flash speed")]
    private float distanceMultiplier = 1;
    [SerializeField, Tooltip("Time that LED's will stay on when all are lit")]
    private float flashStayTime = 1f;
    [SerializeField, Tooltip("Max distance in which the LED's will flash if police are behind")]
    private float maxRearDistanceKm = 0.25f;

    [Header("UI")] 
    [SerializeField] private Image arrowUi;

    // Reference to images that represent our LEDs.
    [SerializeField] private Image[] leftTop = new Image[12];
    [SerializeField] private Image[] rightTop = new Image[12];
    [SerializeField] private Image[] leftBottom = new Image[11];
    [SerializeField] private Image[] rightBottom = new Image[11];

    // Colour to set the LEDs. Later when we add more alerts (eg. speed camera alerts), we can add different colours.
    [SerializeField] private Color policeColour;
    [SerializeField] private Color offColour;

    // Audio which plays when LED's start flashing.
    [SerializeField] private AudioSource audioOnFlash;

    // Update texts to display max scan distance and the distance to the closest police alert.
    [SerializeField] private Text maxDistanceText;
    [SerializeField] private Text distanceToPoliceText;

    private bool HasPolice
        => AlertReciever.Instance.CurrentAlerts != null &&
           AlertReciever.Instance.CurrentAlerts.Any(alert => alert.type == "POLICE");

    // See if LEDs are flashing within editor (for debugging).
    [SerializeField] private bool flashingLeds = false;

    private void Awake()
    {
        TurnOffLeds();
    }

    void Update()
    {
        PointPoliceArrow();
    }

    void UpdateDistanceUi(float distance)
    {
        maxDistanceText.text = AlertReciever.Instance.MaxDistance.ToString("0.00km");
        distanceToPoliceText.text = distance.ToString("0.00km");
    }

    void PointPoliceArrow()
    {
        // Check if there are any alerts to process.
        if (!HasPolice)
        {
            NoPolice();
            return;
        }

        // Variables to track the closest police alert.
        Alert closestPoliceAlert = null;
        float minDistance = float.MaxValue;
        
        // Iterate through alerts to find the closest police alert.
        foreach (var alert in AlertReciever.Instance.CurrentAlerts)
        {
            if (alert.type == "POLICE")
            {
                float distance = GpsHelper.CalculateDistance(AlertReciever.Instance.CurrentLocation, alert.location);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoliceAlert = alert;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////

        // Hide the arrow.
        HideArrow();
        
        // If a police alert was found, point the arrow towards it.
        if (closestPoliceAlert != null)
        {
            
            
            // Update the distance UI;
            UpdateDistanceUi(minDistance);

            // Get the angle at which the closest police is facing from us.
            float relativeAngle = GpsHelper.CalculateFacingDirection(closestPoliceAlert);

            // If the police alert is in front of us then show arrow.
            if (relativeAngle > -90 && relativeAngle < 90)
            {
                // FOR ARDUINO PROJECT: Ignore this method and print the relative angle alongside the alert and its data
                
                // If police are ahead of us.
                OnPoliceAhead(relativeAngle, minDistance);
            }
            else
            {
                
            }
        }
        else
        {
            // No police found logic.
            NoPolice();
        }
    }

    void StartLeds(float distance, float direction)
    {
        if (!flashingLeds)
        {
            StartCoroutine(FlashLeds(distance, direction));
        }

        
    }

    void HideArrow()
    {
        // Default to assume police aren't ahead of us.
        arrowUi.rectTransform.eulerAngles = new Vector3(0, 0, 0);
        arrowUi.color = Color.clear; // Arrow must be clear instead of off colour.

        
    }

    void OnPoliceAhead(float relativeAngle, float distance)
    {
        
        
        arrowUi.rectTransform.eulerAngles = new Vector3(0, 0, -relativeAngle);
        arrowUi.color = policeColour;
        
        StartLeds(distance, relativeAngle);
    }
    

    void NoPolice()
    {
        arrowUi.rectTransform.eulerAngles = new Vector3(0, 0, 0);
        arrowUi.color = Color.clear; // Arrow must be clear instead of off colour.

        TurnOffLeds();
        flashingLeds = false;
    }

    void TurnOffLeds()
    {
        for (int i = 0; i < 12; i++) // Clear all 12 top LEDs
        {
            leftTop[i].color = offColour;
            rightTop[i].color = offColour;
        }

        for (int i = 0; i < 11; i++) // Clear all 11 bottom LEDs
        {
            leftBottom[i].color = offColour;
            rightBottom[i].color = offColour;
        }
    }
    
    private IEnumerator FlashLeds(float closestAlertdistance, float direction)
    {
        flashingLeds = true;

        float progress = 0;

        while (true)
        {
            // If the coroutine exists and no police are found, turn off LEDs and exit.
            if (!HasPolice)
            {
                NoPolice();
                yield break;
            }

            // Normalise distance based on the Maximum allowed distance and the closest police distance.
            float normalisedDistance = GpsHelper.Scale01(0, AlertReciever.Instance.MaxDistance, closestAlertdistance);

            // Increase the flash by a default flash speed plus distance (with distance controlled by a multiplier).
            progress = Mathf.Min(progress + (flashSpeed + normalisedDistance * distanceMultiplier) * Time.deltaTime, 1);

            // Convert the progress to an indexable number.
            var ledProgress = (int)(progress * maxLeds);

            // Play sound before the first LED's turn on.
            if (ledProgress == 0)
            {
                // Play sound when flashing starts.
                PlaySound();
            }

            // Turn on (or off) LEDs based on the progress.
            for (int i = 0; i < 12; i++)
            {
                leftTop[i].color = (i <= ledProgress) ? policeColour : offColour;
                rightTop[i].color = (i <= ledProgress) ? policeColour : offColour;
            }

            for (int i = 0; i < 11; i++)
            {
                leftBottom[i].color = (i <= ledProgress) ? policeColour : offColour;
                rightBottom[i].color = (i <= ledProgress) ? policeColour : offColour;
            }

            // If the progress has finished.
            if (progress >= 1)
            {
                // All LEDs are on, wait briefly, then turn off.
                yield return new WaitForSeconds(flashStayTime / distanceMultiplier);

                // After we have waited, turn LEDs off.
                TurnOffLeds();

                // Reset progress.
                progress = 0;
            }

            yield return null;
        }
    }

    void PlaySound()
    {
        audioOnFlash.Play();
    }
}