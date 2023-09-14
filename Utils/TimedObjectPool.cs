using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;

public class TimedObjectPool<T> : MonoBehaviour where T : MonoBehaviour, ILiveTime
{
    private TimedDictionaryManager<T, int> m_activeObjectPool;
    private ObjectPool<T> m_objectPool;
    private T m_objectToPool;

    public TimedObjectPool(int defaultCapacity = 10, int maxElements = 100, float extraTimeForBullet = 0f) {
        m_activeObjectPool = new TimedDictionaryManager<T, int>(extraTimeForBullet, ReturnObject, false);

        m_objectPool = new ObjectPool<T>(
            () => {
                //Debug.Log("se crea");
                return Instantiate(m_objectToPool);
            },
            obj => {
                //Debug.Log("se pide");
                obj.gameObject.SetActive(true);
                m_activeObjectPool[m_objectToPool, m_objectToPool.TimeToLive] = m_objectToPool.TimesAlive;
            },
            obj => {
                //Debug.Log("se deja");
                if (obj) obj.gameObject.SetActive(false);
            },
            obj => {
                //Debug.Log("se destruye");
                Destroy(obj.gameObject);
            }, false, defaultCapacity, maxElements
        );
    }

    public GameObject GetObject(ref T go) {
        m_objectToPool = go;
        go = m_objectPool.Get();
        return go.gameObject;
    }

    public void TryToReturnObject(T go) {
        if (m_activeObjectPool[go] > 0) {
            m_activeObjectPool[go]--;
        }
    }

    public void ReturnObject(T go) {
        m_objectPool.Release(go);
    }
}
