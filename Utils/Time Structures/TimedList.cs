using System.Collections.Generic;
using System.Linq;
using System;

public class TimedList<T> : BaseTimedStructure
{
    public 
    List<TimedElement<T>> List;
    private Action<T> m_processValueAction;

    #region Constructors
    public TimedList(Action<T> processValueAction = null, int defaultNumberOfElements = 1, Action sweepStructureAction = null) {
        List = new List<TimedElement<T>>(defaultNumberOfElements);

        if (sweepStructureAction != null) m_sweepStrutureAction = sweepStructureAction;
        else m_sweepStrutureAction = SweepStructureAction;

        m_processValueAction = processValueAction;
    }
    #endregion

    #region SweepSystem
    protected override Action m_sweepStrutureAction { get; set; }
    private void SweepStructureAction() {
        var elementsToRemove = List.Where( element => (element.Timestamp - DateTime.Now).TotalMilliseconds < 0 ).ToList();
        
        elementsToRemove.ForEach( element => { 
            if ( m_processValueAction != null ) m_processValueAction(element.Element);
            List.Remove(element);
        });
    }
    #endregion
    // TODO : implement the List functions
    #region ListFunctions
    #endregion
}