using UnityEngine;
using UnityEngine.InputSystem;

public class MouseChaser : MonoBehaviour
{
    [SerializeField]
    private float m_lerp = 0.125f;
    [SerializeField]
    private float m_toggle_speed = 10f;

    [SerializeField]
    private Camera m_pov;

    private TrailRenderer m_trail;
    private Vector3 m_pos;
    private Vector2 m_prior_mouse_pos;
    private float m_avg_speed;

    public void HandleInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input_position = context.ReadValue<Vector2>();
            m_pos = m_pov.ScreenToWorldPoint(new Vector3(input_position.x, input_position.y, 10f));

            float speed = (m_prior_mouse_pos - input_position).magnitude;
            m_prior_mouse_pos = input_position;

            m_avg_speed = (speed + m_avg_speed) / 2f;

            m_trail.emitting = m_avg_speed > m_toggle_speed;
        }
    }


    private void Awake()
    {
        m_trail = GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        transform.position =
                Vector3.Lerp(
                    transform.position,
                    m_pos,
                    m_lerp);
    }
}
