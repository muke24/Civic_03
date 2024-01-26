using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleSettings : MonoBehaviour
{
	[Header("UI")]
	[SerializeField]
	private InputField plateIf;
	[SerializeField]
	private InputField stateIf;
	[SerializeField]
	private GameObject colourWheel;
	[SerializeField]
	private Color selectedColour;
	[SerializeField]
	private Image backgroundImage;
	[SerializeField]
	private Image textImage;
	[Header("Car")]
	[SerializeField]
	private Text[] plateCar;
	[SerializeField]
	private Text[] stateCar;
	[SerializeField]
	private Material backgroundMaterial;
	[SerializeField]
	private Material textMaterial;

	// APPLY ALL
	public void ApplyAll()
	{
        // On close button
		//ApplyTextColour();
		//ApplyBackgroundColour();
		ApplyPlateText();
		ApplyStateText();
	}

	// SETTING
	public void SetBackgroundColour()
    {
        // Get current selected colour
        Color curCol = backgroundMaterial.color;
		backgroundImage.color = curCol;
    }

	public void SetTextColour()
	{
		// Get current selected colour
		Color curCol = textMaterial.color;
		textMaterial.color = curCol;
	}


	// APPLYING
    void ApplyBackgroundColour()
    {
		backgroundMaterial.color = selectedColour;
	}

    void ApplyTextColour()
    {
		textMaterial.color = selectedColour;
	}
    
    void ApplyPlateText()
    {
		for (int i = 0; i < plateCar.Length; i++)
		{
			plateCar[i].text = plateIf.text;
		}
    }

    void ApplyStateText()
    {
		for (int i = 0; i < stateCar.Length; i++)
		{
			stateCar[i].text = stateIf.text;
		}
	}

	public void OnOpen()
	{
		SetBackgroundColour();
		SetTextColour();
	}
}
