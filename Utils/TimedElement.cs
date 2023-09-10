using UnityEngine;

public class TimedElement<T>
{
    public float Timestamp { get; private set; }
    private T m_element;
    public T Element { 
        get { Timestamp = Time.time; return m_element; } 
        set { Timestamp = Time.time; m_element = value; }
    }

    public TimedElement(T element)
    {
        Element = element;
        Timestamp = Time.time;
    }
}
