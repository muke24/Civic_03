using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GpsHelper
{
    /// <summary>
    /// Scales a value between 0 and 1 given its old range.
    /// </summary>
    /// <param name="OldMin">Old minumum.</param>
    /// <param name="OldMax">Old maximum.</param>
    /// <param name="OldValue">Value to scale.</param>
    /// <returns></returns>
    public static float Scale01(float OldMin, float OldMax, float OldValue)
    {
        float NewMin = 0;
        float NewMax = 1;

        float OldRange = OldMax - OldMin;
        float NewRange = NewMax - NewMin;
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        if (NewValue > NewMax)
        {
            NewValue = NewMax;
        }
        else if (NewValue < NewMin)
        {
            NewValue = NewMin;
        }

        return NewValue;
    }
    
    /// <summary>
    /// Haversine formula to calculate distance between two lat/lon points (in kilometers)
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    public static float CalculateDistance(Location point1, Location point2)
    {
        // point1.x is longitude, point1.y is latitude
        float lon1 = point1.x * Mathf.Deg2Rad;
        float lat1 = point1.y * Mathf.Deg2Rad;
        float lon2 = point2.x * Mathf.Deg2Rad;
        float lat2 = point2.y * Mathf.Deg2Rad;

        float dLat = lat2 - lat1;
        float dLon = lon2 - lon1;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(lat1) * Mathf.Cos(lat2) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        float earthRadiusKm = 6371f;

        return earthRadiusKm * c;
    }
    
    public static float CalculateFacingDirection(Alert alert, bool debug = false)
    {
        
        // Calculate the raw bearing from current location to alert location
        float rawAngle = CalculateAngle(AlertReciever.Instance.CurrentLocation, alert.location);

        // Adjust the angle by the device's current direction to get the relative angle
        float relativeAngle = rawAngle - AlertReciever.Instance.CurrentDirection - Camera.main.transform.parent.eulerAngles.y;

        // Normalize the relative angle to -180° to 180° for consistency
        relativeAngle = NormalizeAngle(relativeAngle);

        if (debug)
        {
            // Output the result for debugging
            Debug.Log($"Alert: {alert.type} at ((Longitude){alert.location.x}, (Latitude){alert.location.y}), " +
                      $"Bearing: {rawAngle:F1}°, Relative Angle: {relativeAngle:F1}°");
        }

        return relativeAngle;
    }

    public static float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    public static float CalculateAngle(Location from, Location to)
    {
        // Convert latitude and longitude to radians
        float phi1 = from.y * Mathf.Deg2Rad; // Latitude of current location
        float phi2 = to.y * Mathf.Deg2Rad;   // Latitude of alert location
        float deltaLambda = (to.x - from.x) * Mathf.Deg2Rad; // Longitude difference
        float lambda1 = from.x * Mathf.Deg2Rad; // Longitude of current location (not used directly but kept for clarity)

        // Calculate bearing components
        float y = Mathf.Sin(deltaLambda) * Mathf.Cos(phi2);
        float x = Mathf.Cos(phi1) * Mathf.Sin(phi2) - Mathf.Sin(phi1) * Mathf.Cos(phi2) * Mathf.Cos(deltaLambda);
        float theta = Mathf.Atan2(y, x);

        // Convert to degrees and normalize to 0-360
        float bearing = (theta * Mathf.Rad2Deg + 360) % 360;
        return bearing;
    }
}
