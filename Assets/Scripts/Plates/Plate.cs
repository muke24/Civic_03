using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    public string plate = "LAPTOP";
    public string state = "NSW";

    public Color backgroundColour;
    public Color textColour;

    [SerializeField]
    private Material backgroundMaterial;
	[SerializeField]
	private Material outlineMaterial;
    [SerializeField]
    private Text plateText;
	[SerializeField]
	private Text stateText;

	// Start is called before the first frame update
	void Start()
    {
        ApplyToPlate();
	}

	void ApplyToPlate()
    {
		backgroundMaterial.color = backgroundColour;
        outlineMaterial.color = textColour;
        outlineMaterial.SetColor("_EmissionColor", textColour);
        plateText.color = textColour;
        plateText.text = plate;
		stateText.color = textColour;
        stateText.text = state;
    }
}
