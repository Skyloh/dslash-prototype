using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public UnityEvent<bool> OnSlashPerformed; // where the bool is "was it successful"

    [SerializeField] 
    private int m_numDirections = 4;
    [SerializeField]
    private float m_weight = 0.8f;
    [SerializeField]
    private float m_speed = 10f;
    [SerializeField, Range(0.05f, 1f)]
    private float m_accuracy = 0.2f;
    [SerializeField, Range(-1f, 0f)]
    private float m_reselect_dotbounds = -0.15f;
    [SerializeField]
    private float m_slerp = 0.0125f;

    [Space(10)]

    [SerializeField]
    private Transform m_arrow;

    private Vector2[] m_directions;
    private int m_current = 0;

    private Vector2 m_prior_mouse_pos;

    private void Awake()
    {
        float angle_between = 360f / m_numDirections;

        m_prior_mouse_pos = Vector2.zero;
        m_directions = new Vector2[m_numDirections];

        for (int i = 0; i < m_numDirections; ++i)
        {
            m_directions[i] = Quaternion.Euler(0f, 0f, angle_between * i) * Vector2.up;
        }

        RotateArrowTowards(m_directions[m_current]);
    }

    public void HandleInput(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            Vector2 input_position = context.ReadValue<Vector2>();
            Vector2 delta = m_prior_mouse_pos - input_position;

            m_prior_mouse_pos = input_position;

            float magnitude = delta.magnitude;
            delta.Normalize();

            if (magnitude > m_speed)
            {
                PerformSlash(delta);
            }
        }
    }

    private void PerformSlash(Vector2 dir)
    {
        bool did_succeed = false;

        if (-Vector2.Dot(m_directions[m_current], dir) > (1 - m_accuracy))
        {
            did_succeed = true;
            RotateArrowTowards(m_directions[PickNextRandom()]);
        }

        OnSlashPerformed.Invoke(did_succeed);
    }

    private int PickNextRandom()
    {
        int value;

        do
        {
            value = Random.Range(0, m_numDirections);
            // try to get a vector that points away.
        } while (Vector2.Dot(m_directions[m_current], m_directions[value]) >= m_reselect_dotbounds);

        m_current = value;

        return value;
    }

    private void RotateArrowTowards(Vector2 dir)
    {
        StopAllCoroutines();

        StartCoroutine(IERotateTowards(dir));
    }

    private IEnumerator IERotateTowards(Vector2 dir)
    {
        Vector3 new_dir = new(dir.x, dir.y, 0f);

        while (Vector2.Dot(m_arrow.right, new_dir) < 0.985f)
        {
            m_arrow.right = Vector3.Slerp(m_arrow.right, new_dir, m_slerp);

            yield return new WaitForEndOfFrame();
        }

        m_arrow.right = new_dir;
    }
}
