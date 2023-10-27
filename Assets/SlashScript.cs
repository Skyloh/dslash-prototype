using UnityEngine;

public class SlashScript : MonoBehaviour
{
    [SerializeField]
    private Transform m_arrow;

    private ParticleSystem m_system;

    private void Awake()
    {
        m_system = GetComponent<ParticleSystem>();
    }

    public void OnActivate(bool fire)
    {
        if (fire)
        {
            m_system.Stop();

            //float angle_diff = m_arrow.eulerAngles.z - transform.eulerAngles.z - 45f;
            // transform.RotateAround(m_arrow.position, Vector3.forward, angle_diff);

            m_system.Play();
        }
    }
}
