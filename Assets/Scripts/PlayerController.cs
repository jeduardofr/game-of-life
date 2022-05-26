using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float m_Speed;

	[SerializeField]
	private float m_MaxSpeed = 0.5f; 

	[SerializeField]
	private float m_HorizontalInput, m_ForwardInput;

	private Rigidbody m_Player;

    // Start is called before the first frame update
    void Start()
    {
		m_Player = GetComponent<Rigidbody>();

		m_Speed = m_MaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
		m_HorizontalInput = Input.GetAxis("Horizontal");
		m_ForwardInput = Input.GetAxis("Vertical");

		float speed = Mathf.Max(Mathf.Abs(m_HorizontalInput), Mathf.Abs(m_ForwardInput));
		speed *= m_Speed / m_MaxSpeed;

		Vector3 movimiento = new Vector3(m_HorizontalInput, m_ForwardInput, 0);
		transform.Translate(movimiento * m_Speed * Time.deltaTime);
    }
}
