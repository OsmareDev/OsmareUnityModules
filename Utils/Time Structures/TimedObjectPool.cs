using UnityEngine;
using System.Linq;

public class TimedObjectPool<T> where T : MonoBehaviour, ILiveTime {
    private TimedDictionary<T, int> m_activePool;
    private TimedList<T> m_inactivePool;

    private int m_maxNumberOfelements;
    private float m_timeUntilDestroy;

    public TimedObjectPool(int defaultNumberOfElements = 10, int maxNumberOfelements = 100, float timeUntilDestroy = 5f) {
        m_activePool = new TimedDictionary<T, int>(ReleaseElement, null, defaultNumberOfElements);
        m_inactivePool = new TimedList<T>(obj => Object.Destroy(obj.gameObject), defaultNumberOfElements);

        m_maxNumberOfelements = maxNumberOfelements;
        m_timeUntilDestroy = timeUntilDestroy;
    }

    public void StartSweep(float secondsBetweenSweeps) {
        m_activePool.StartSweep(secondsBetweenSweeps);
        m_inactivePool.StartSweep(secondsBetweenSweeps);
    }

    public void StopSweep() {
        m_activePool.StopSweep();
        m_inactivePool.StopSweep();
    }

    public T2 GetElement<T2>(T2 prefab) where T2 : T {
        if (m_activePool.Dictionary.Count >= m_maxNumberOfelements) return null;
        
        TimedElement<T> obj = m_inactivePool.List.FirstOrDefault( item => item.Element.GetType() == prefab.GetType() );
        if (obj != null) {
            m_inactivePool.List.Remove(obj);
            m_activePool[obj.Element, obj.Element.TimeToLive, false] = obj.Element.TimesAlive;
            obj.Element.gameObject.SetActive(true);
            return (T2)obj.Element;
        } else {
            T2 newObj = Object.Instantiate(prefab);
            m_activePool[newObj, newObj.TimeToLive, false] = newObj.TimesAlive;
            return newObj;
        }
    }   

    public void ReleaseElement(T element) {
        if (m_activePool.Remove(element)) {
            element.gameObject.SetActive(false);
            m_inactivePool.List.Add(new TimedElement<T>(element, m_timeUntilDestroy));
        }
    } 

    public void TryReleaseElement(T element) {
        if (m_activePool.TryGetValue(element, out int value)) {
            if (value < 0) {
                m_activePool.Remove(element);
                element.gameObject.SetActive(false);
                m_inactivePool.List.Add(new TimedElement<T>(element, m_timeUntilDestroy));
            } else {
                m_activePool[element]--;
            }
        }
    }
}

