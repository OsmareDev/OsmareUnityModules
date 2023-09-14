using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TimedDictionaryManager<T1, T2>
{
    public Dictionary<T1, TimedElement<T2>> Dictionary {get; set;}
    public float TimeThreshold {get; set;}
    private Action<T1> m_processKey;
    private Action<T2> m_processValue;
    private bool m_rechargeElements;

    public int Count { get { return Dictionary.Count; } }

    #region Contructors
    public TimedDictionaryManager( float timeThreshold, bool rechargeTime = true ) { 
        this.TimeThreshold = timeThreshold;
        Dictionary = new Dictionary<T1, TimedElement<T2>>();
        m_rechargeElements = rechargeTime;
    }
    public TimedDictionaryManager( float timeThreshold, Action<T1> funcK, bool rechargeTime = true ) { 
        this.TimeThreshold = timeThreshold;
        Dictionary = new Dictionary<T1, TimedElement<T2>>();
        m_processKey = funcK;
        m_rechargeElements = rechargeTime;
    }
    public TimedDictionaryManager( float timeThreshold, Action<T2> funcV, bool rechargeTime = true ) { 
        this.TimeThreshold = timeThreshold;
        Dictionary = new Dictionary<T1, TimedElement<T2>>();
        m_processValue = funcV;
        m_rechargeElements = rechargeTime;
    }
    public TimedDictionaryManager( float timeThreshold, Action<T1> funcK, Action<T2> funcV, bool rechargeTime = true ) { 
        this.TimeThreshold = timeThreshold;
        Dictionary = new Dictionary<T1, TimedElement<T2>>();
        m_processKey = funcK;
        m_processValue = funcV;
        m_rechargeElements = rechargeTime;
    }
    #endregion

    private float m_sweepTime = 0f;

    public void SweepDictionary() {
        if (m_sweepTime < Time.time) {
            var elementsToRemove = Dictionary.Where( element => TimeThreshold < (Time.time - element.Value.Timestamp) ).ToList();
            elementsToRemove.ForEach( element => { 
                if ( m_processValue != null ) m_processValue(element.Value.Element);
                Dictionary.Remove(element.Key);
                if ( m_processKey != null ) m_processKey(element.Key);
            });
            m_sweepTime = Time.time + TimeThreshold;
        }
    }

    public bool TryGetValue(T1 key, out T2 value) {
        if (Dictionary.TryGetValue(key, out TimedElement<T2> element)) {
            value = element.Element;
            return true;
        } else {
            value = default(T2);
            return false;
        }
    }

    public bool Remove(T1 key) => Dictionary.Remove(key);

    public T2 this[T1 index, float time = 0f] {
        get {
            return Dictionary[index].Element;
        }
        set {
            //TODO : make the default value of TimedElement a new TimedElement so this if can dissapear
            if (Dictionary.TryGetValue(index, out TimedElement<T2> element)) {
                element.Element = value;
                Dictionary[index] = element;
            } else {
                Dictionary[index] = new TimedElement<T2>(value);
            }
        }
    }
}