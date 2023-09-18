using System.Collections.Generic;
using System;

public class TimedQueue<T> : BaseTimedStructure
{
    public Queue<TimedElement<T>> Queue;
    private Action<T> m_processValueAction;

    #region Constructors
    public TimedQueue(Action<T> processValueAction = null, int defaultNumberOfElements = 1, Action sweepStructureAction = null) {
        Queue = new Queue<TimedElement<T>>(defaultNumberOfElements);

        if (sweepStructureAction != null) m_sweepStrutureAction = sweepStructureAction;
        else m_sweepStrutureAction = SweepStructureAction;

        m_processValueAction = processValueAction;
    }
    #endregion

    #region SweepSystem
    protected override Action m_sweepStrutureAction { get; set; }
    private void SweepStructureAction() {
        foreach (TimedElement<T> item in Queue) {
            if ((item.Timestamp - DateTime.Now).TotalMilliseconds > 0) break;

            Queue.Dequeue();
            if (m_processValueAction != null) m_processValueAction(item.Element);
        }
    }
    #endregion

    #region QueueFunctions
    public int Count { get => Queue.Count; }
    public void Clear() => Queue.Clear();
    public T Dequeue() => Queue.Dequeue().Element;
    public void Enqueue(T item, float timeToLive, bool rechargeTimeAtAccess) => Queue.Enqueue(new TimedElement<T>(item, timeToLive, rechargeTimeAtAccess));
    public T Peek() => Queue.Peek().Element;
    public TimedElement<T>[] ToArray() => Queue.ToArray();
    public void TrimExcess() => Queue.TrimExcess();
    public bool TryDequeue(out T result) {
        bool attempt = Queue.TryDequeue(out TimedElement<T> te);
        result = te.Element;
        return attempt;
    }
    public bool TryPeek(out T result) {
        bool attempt = Queue.TryPeek(out TimedElement<T> te);
        result = te.Element;
        return attempt;
    }
    #endregion
}