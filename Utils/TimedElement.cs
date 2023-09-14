using UnityEngine;

public class TimedElement<T>
{
    private float m_extraTime;
    private bool m_rechargeTime;
    public float Timestamp { get; private set; }
    private T m_element;
    public T Element { 
        get { if (m_rechargeTime) Timestamp = Time.time + m_extraTime; return m_element; } 
        set { if (m_rechargeTime) Timestamp = Time.time + m_extraTime; m_element = value; }
    }

    public TimedElement(T element, float extraTimeToLive = 0f, bool rechargeTime = true)
    {
        Element = element;
        m_extraTime = extraTimeToLive;
        m_rechargeTime = rechargeTime;
        Timestamp = Time.time + m_extraTime;
    }
}
