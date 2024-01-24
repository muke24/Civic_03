using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
	//[SerializeField]
	//private RectTransform compassBarTransform;

	//[SerializeField]
	//private RectTransform policeMarkerTransform;
	//[SerializeField]
	//private RectTransform northMarkerTransform;
	//[SerializeField]
	//private RectTransform southMarkerTransform;

	//[SerializeField]
	//private Transform cameraTransform;
	//[SerializeField]
	//private Transform policeObjectTransform;
	[SerializeField]
	private Transform cameraPivot;

	[SerializeField]
	private RawImage compassScrollTexture;

	void Update()
	{
		float rot = Input.compass.trueHeading + cameraPivot.rotation.eulerAngles.y;

		compassScrollTexture.uvRect = new Rect(rot / 360f, 0, 1, 1);
	}
}
