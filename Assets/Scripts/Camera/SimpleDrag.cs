using System.Collections;
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
		if (animatingBack) return;

		CheckAutoReturn();

		if (!touchAnywhere && !m_rotating)
		{
			RaycastHit hit;
			Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(ray, out hit, 1000, targetLayer))
				return;
		}

		HandleRotation();
	}

	private void CheckAutoReturn()
	{
		if (!m_rotating && transform.rotation.eulerAngles != startRot)
		{
			noTouchTime += Time.deltaTime;
			if (noTouchTime >= timeToReturnToStartRot)
			{
				noTouchTime = 0;
				StartCoroutine(LerpBack());
			}
		}
		else
		{
			noTouchTime = 0;
		}
	}

	private void HandleRotation()
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_rotating = true;
			m_previousX = Input.mousePosition.x;
			m_previousY = Input.mousePosition.y;
		}

		if (Input.GetMouseButton(0) && m_rotating)
		{
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

	IEnumerator LerpBack()
	{
		animatingBack = true;
		Quaternion targetRotation = Quaternion.Euler(startRot);
		while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timeToReturnToStartRot * Time.deltaTime);
			yield return null;
		}
		animatingBack = false;
	}

}
