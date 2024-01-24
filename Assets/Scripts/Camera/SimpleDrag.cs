using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDrag : MonoBehaviour
{
	[SerializeField] private LayerMask targetLayer;
	[SerializeField] private float rotationRate = 3.0f;
	[SerializeField] private bool xRotation;
	[SerializeField] private bool yRotation;
	[SerializeField] private bool invertX;
	[SerializeField] private bool invertY;
	[SerializeField] private bool touchAnywhere;
	private float m_previousX;
	private float m_previousY;
	private Camera m_camera;
	private bool m_rotating = false;

	private Vector3 startRot;
	private float noTouchTime = 0;
	private bool animatingBack = false;

	[SerializeField]
	private float timeToReturnToStartRot = 5f;

	private void Awake()
	{
		m_camera = Camera.main;
		startRot = transform.rotation.eulerAngles;
	}

	private void Update()
	{
		if (!animatingBack)
		{
			if (transform.rotation.eulerAngles != startRot)
			{
				if (!m_rotating)
				{
					if (noTouchTime < timeToReturnToStartRot)
					{
						noTouchTime += Time.deltaTime;
					}
					else
					{
						noTouchTime = 0;
						StopAllCoroutines();
						StartCoroutine(LerpBack());
					}
				}
			}

			if (!touchAnywhere)
			{
				//No need to check if already rotating
				if (!m_rotating)
				{
					RaycastHit hit;
					Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
					if (!Physics.Raycast(ray, out hit, 1000, targetLayer))
					{
						return;
					}
				}
			}

			if (Input.GetMouseButtonDown(0))
			{
				noTouchTime = 0;

				m_rotating = true;
				m_previousX = Input.mousePosition.x;
				m_previousY = Input.mousePosition.y;
			}
			// get the user touch input
			if (Input.GetMouseButton(0))
			{
				var touch = Input.mousePosition;
				var deltaX = -(Input.mousePosition.y - m_previousY) * rotationRate;
				var deltaY = -(Input.mousePosition.x - m_previousX) * rotationRate;
				if (!yRotation) deltaX = 0;
				if (!xRotation) deltaY = 0;
				if (invertX) deltaY *= -1;
				if (invertY) deltaX *= -1;
				transform.Rotate(deltaX, deltaY, 0, Space.World);

				m_previousX = Input.mousePosition.x;
				m_previousY = Input.mousePosition.y;
			}
			if (Input.GetMouseButtonUp(0))
				m_rotating = false;
		}
	}

	IEnumerator LerpBack()
	{
		animatingBack = true;
		while (true)
		{

			if (transform.rotation != Quaternion.Euler(startRot))
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(startRot), timeToReturnToStartRot * Time.deltaTime);
				animatingBack = true;

				yield return null;
			}
			else
			{
				animatingBack = false;
				yield break;
			}
			animatingBack = false;

		}
	}
}