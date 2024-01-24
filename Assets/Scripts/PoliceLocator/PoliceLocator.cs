using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceLocator : MonoBehaviour
{
    [SerializeField]
    private Transform ArrowPivot;

    private string val
    {
        set
        {
            if (value != null)
            {
                SetArrow(value);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Connect to cocktooth

        // When connected, start coroutine to check for a new value
    }

    void SetArrow(string val)
    {

    }
}
