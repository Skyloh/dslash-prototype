using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreScript : MonoBehaviour
{
    private int m_successful;
    private float m_start_time;

    [SerializeField]
    private TextMeshProUGUI m_tmp;
    [SerializeField]
    private TextMeshProUGUI m_tmp_time;

    [SerializeField]
    private int m_time = 15;

    public UnityEvent<bool> OnGameEnd;

    private void Awake()
    {
        m_tmp.text = "Move the mouse quickly to slash.";
        m_start_time = Time.realtimeSinceStartup;

        InvokeRepeating(nameof(Tick), 2f, 1f);
    }

    private void Tick()
    {
        int seconds = m_time - Mathf.FloorToInt((Time.realtimeSinceStartup - m_start_time - 2f) % 60f);

        m_tmp_time.text = $"{seconds}";

        if (seconds == 0)
        {
            m_tmp_time.text = "END";
            CancelInvoke();
            OnGameEnd.Invoke(false);
        }
    }

    public void OnSlash(bool slash)
    {
        if (slash)
        {
            m_successful++;
            Wiggle();
        }

        m_tmp.text = $"{m_successful}";
    }

    private void Wiggle()
    {
        StopAllCoroutines();
        StartCoroutine(IEWiggle());
    }

    private IEnumerator IEWiggle()
    {
        int iter = 25;

        while (iter > 0)
        {
            m_tmp.rectTransform.Rotate(0f, 0f, Mathf.Cos(iter));

            yield return new WaitForFixedUpdate();

            iter -= 1;
        }
    }
}
