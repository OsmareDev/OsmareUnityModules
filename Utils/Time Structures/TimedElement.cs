using System;

public class TimedElement<T>
{
    private float m_extraTime;
    private bool m_rechargeTime;
    public DateTime Timestamp { get; private set; }
    private T m_element;
    public T Element { 
        get { if (m_rechargeTime) Timestamp = DateTime.Now.AddSeconds(m_extraTime); return m_element; } 
        set { if (m_rechargeTime) Timestamp = DateTime.Now.AddSeconds(m_extraTime); m_element = value; }
    }

    public TimedElement(T element, float extraTimeToLive = 0f, bool rechargeTime = true)
    {
        Timestamp = DateTime.Now.AddSeconds(extraTimeToLive);
        m_extraTime = extraTimeToLive;
        m_rechargeTime = rechargeTime;
        Element = element;
    }

    // sobrecarga implicita del valor a TimedElement
    // public static implicit operator T(TimedElement<T> instance) => instance.Element;
    // public static implicit operator TimedElement<T>(T value) => new TimedElement<T>(value);
}
