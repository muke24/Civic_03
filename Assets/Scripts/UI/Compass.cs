using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
	[SerializeField]
	private Transform cameraPivot;
	[SerializeField]
	private Image compassImage;

	private Material compassMaterial;

	void Start()
	{
		// Ensure there's a material attached to the compass image
		if (compassImage.material != null)
		{
			compassMaterial = compassImage.material;
		}
		else
		{
			Debug.LogError("No material found on compass image");
		}
	}

	void Update()
	{
		if (compassMaterial != null)
		{
			float rot = Input.compass.trueHeading + cameraPivot.rotation.eulerAngles.y;

			// Adjust the material's texture offset to rotate the compass
			Vector2 offset = new Vector2(rot / 360f, 0);
			compassMaterial.mainTextureOffset = offset;
		}
	}
}
