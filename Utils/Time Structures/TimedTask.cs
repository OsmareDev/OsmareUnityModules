using System.Collections.Generic;
using System.Linq;
using System;

public class TimedTask : BaseTimedStructure
{
    #region Constructors
    protected override Action m_sweepStrutureAction { get; set; }
    public TimedTask(Action sweepStructureAction = null) {
        if (sweepStructureAction != null) m_sweepStrutureAction = sweepStructureAction;
    }
    #endregion
}