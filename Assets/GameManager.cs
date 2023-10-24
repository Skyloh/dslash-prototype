using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private int m_numDirections = 4;
    [SerializeField]
    private float m_weight = 0.8f;
    [SerializeField]
    private float m_speed = 10f;
    [SerializeField, Range(0.05f, 1f)]
    private float m_accuracy = 0.2f;
    [SerializeField]
    private Transform m_arrow;

    private Vector2[] m_directions;
    private Vector2 m_cumulative;
    private int m_current = 0;


    private void Awake()
    {
        float angle_between = 360f / m_numDirections;

        m_cumulative = Vector2.zero;

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
            Vector2 input = context.ReadValue<Vector2>();
            float magnitude = input.magnitude;

            input.Normalize();

            m_cumulative.x += m_weight * input.x;
            m_cumulative.y += m_weight * input.y;
            m_cumulative.Normalize();

            if (magnitude > m_speed)
            {
                PerformSlash();
            }
        }
    }

    private void PerformSlash()
    {
        Debug.Log(Mathf.Abs(Vector2.Dot(m_directions[m_current], m_cumulative)));
        if (Vector2.Dot(m_directions[m_current], m_cumulative) > (1 - m_accuracy))
        {
            Debug.Log("Success!");

            RotateArrowTowards(m_directions[PickNextRandom()]);
        }
    }

    private int PickNextRandom()
    {
        int value;

        do
        {
            value = Random.Range(0, m_numDirections);
        } while (value == m_current);

        m_current = value;

        return value;
    }

    private void RotateArrowTowards(Vector2 dir)
    {
        m_arrow.right = dir;
    }
}
