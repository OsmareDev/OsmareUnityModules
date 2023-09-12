using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DictionaryManager<T1, T2>
{
    public Dictionary<T1, TimedElement<T2>> Dictionary {get; set;}
    public float TimeThreshold {get; set;}

    public DictionaryManager( float timeThreshold ) { 
        this.TimeThreshold = timeThreshold;
        Dictionary = new Dictionary<T1, TimedElement<T2>>();
    }
    private float m_sweepTime = 0f;

    public void SweepDictionary() {
        if (m_sweepTime < Time.time) {
            var elementsToRemove = Dictionary.Where( element => TimeThreshold < (Time.time - element.Value.Timestamp) ).ToList();
            elementsToRemove.ForEach( element => { 
                // TODO : make it more generic
                T2 value = Dictionary[element.Key].Element;
                if ( value is (GameObject)) GameObject.Destroy((GameObject)(object)value);
                Dictionary.Remove(element.Key);});
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

    public T2 this[T1 index] {
        get {
            return Dictionary[index].Element;
        }
        set {
            Dictionary[index] = new TimedElement<T2>(value);
        }
    }
}
