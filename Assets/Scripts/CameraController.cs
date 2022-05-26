using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Transform m_Player, m_PlayerCamera, m_FocusPoint;

	[SerializeField]
	private float m_Zoom = -12;

	[SerializeField]
	private float m_ZoomSpeed = 3;

	[SerializeField]
	private float m_ZoomMax = -0.5f, m_ZoomMin = -10f;

	[SerializeField]
	private float m_CameraHeight = 4f;

	[SerializeField]
	private float m_CamRotX, m_CamRotY;

	[SerializeField]
	private float m_MouseSensitivity = 2;
    // Start is called before the first frame update
    void Start()
    {
 		// Validation for transforms
 		if (m_Player == null)
 			Debug.Log("Falta asignar el personaje al CameraController");
 		
 		if (m_PlayerCamera == null)
 			Debug.Log("Falta asignar la cámara al CameraController");
 
 		if (m_FocusPoint == null)
 			Debug.Log("Falta asignar el punto de foco al CameraController");
 
 		// Relation between components
 		m_FocusPoint.SetParent(m_Player);
 		m_PlayerCamera.SetParent(m_FocusPoint);
 
 		// Assign position & rotation
 		m_FocusPoint.localPosition = new Vector3(0, m_CameraHeight, 0);
 		m_FocusPoint.localRotation = Quaternion.Euler(0, 0, 0);
 		m_FocusPoint.localScale = new Vector3(1, 1, 1);
 
 		m_PlayerCamera.localPosition = new Vector3(0, 0, m_Zoom);
 		m_PlayerCamera.LookAt(m_Player);
 		m_PlayerCamera.localScale = new Vector3(1, 1, 1);       
    }

    // Update is called once per frame
    void Update()
    {
		// Zoom
		m_Zoom += Input.GetAxis("Mouse ScrollWheel") * m_ZoomSpeed;
		m_Zoom = Mathf.Clamp(m_Zoom, m_ZoomMin, m_ZoomMax);
		m_PlayerCamera.localPosition = new Vector3(0, 0, m_Zoom);

		// Rotation
		if (Input.GetMouseButton(1)) {
			m_CamRotX += Input.GetAxis("Mouse X") * m_MouseSensitivity;
			m_CamRotY += Input.GetAxis("Mouse Y") * m_MouseSensitivity;
			m_CamRotY = Mathf.Clamp(m_CamRotY, -20, 70);
			m_FocusPoint.localRotation = Quaternion.Euler(m_CamRotY, 0, 0);
			m_Player.localRotation = Quaternion.Euler(0, m_CamRotX, 0);
			m_PlayerCamera.LookAt(m_Player);
		}

		m_PlayerCamera.localRotation = Quaternion.identity;
    }
}
