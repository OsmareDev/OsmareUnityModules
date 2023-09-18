using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TimedDictionary<TKey, TValue> : BaseTimedStructure
{
    public Dictionary<TKey, TimedElement<TValue>> Dictionary;
    private Action<TValue> m_processValueAction;
    private Action<TKey> m_processKeyAction;

    #region Constructors
    public TimedDictionary(Action<TKey> processKeyAction = null, Action<TValue> processValueAction = null, int defaultNumberOfElements = 1, Action sweepStructureAction = null) {
        Dictionary = new Dictionary<TKey, TimedElement<TValue>>(defaultNumberOfElements);

        if (sweepStructureAction != null) m_sweepStrutureAction = sweepStructureAction;
        else m_sweepStrutureAction = SweepStructureAction;

        m_processValueAction = processValueAction;
        m_processKeyAction = processKeyAction;
    }
    #endregion

    #region SweepSystem
    protected override Action m_sweepStrutureAction { get; set; }
    private void SweepStructureAction() {
        var elementsToRemove = Dictionary.Where( element => (element.Value.Timestamp - DateTime.Now).TotalMilliseconds < 0 ).ToList();
        
        elementsToRemove.ForEach( element => { 
            if ( m_processValueAction != null ) m_processValueAction(element.Value.Element);
            if ( m_processKeyAction != null ) m_processKeyAction(element.Key);
            Dictionary.Remove(element.Key);
        });
    }
    #endregion

    #region DictionaryFunctions
    public TValue this[TKey index, float timeToLive = 0f, bool rechargeTimeAtAccess = true] {
        get {
            return Dictionary[index].Element;
        }
        set {
            if (Dictionary.TryGetValue(index, out TimedElement<TValue> element)) {
                element.Element = value;
                Dictionary[index] = element;
            } else {
                Dictionary[index] = new TimedElement<TValue>(value, timeToLive, rechargeTimeAtAccess);
            }
        }
    }

    public void Add(TKey key, TValue value, float timeToLive = 0f, bool rechargeTimeAtAccess = true) => Dictionary.Add(key, new TimedElement<TValue>(value, timeToLive, rechargeTimeAtAccess));
    public void Clear() => Dictionary.Clear();
    public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);
    // public bool ContainsValue(TValue value) => Dictionary.ContainsValue(value);
    public int EnsureCapacity(int capacity) => Dictionary.EnsureCapacity(capacity);
    public bool Remove(TKey key, out TValue value) {
        if (Dictionary.Remove(key, out TimedElement<TValue> tValue)) {
            value = tValue.Element;
            return true;
        } else {
            value = default;
            return false;
        }
    }
    public bool Remove(TKey key) => Dictionary.Remove(key);
    public void TrimExcess() => Dictionary.TrimExcess();
    public void TrimExcess(int capacity) => Dictionary.TrimExcess(capacity);
    public bool TryAdd(TKey key, TValue value, float timeToLive = 0f, bool rechargeTimeAtAccess = true ) => Dictionary.TryAdd(key, new TimedElement<TValue>(value, timeToLive, rechargeTimeAtAccess));
    public bool TryGetValue(TKey key, out TValue value) { 
        if (Dictionary.TryGetValue(key, out TimedElement<TValue> tValue)) {
            value = tValue.Element;
            return true;
        } else {
            value = default;
            return false;
        }
    }
    #endregion
}