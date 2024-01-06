using System.Threading.Tasks;
using UnityEngine;
using System;

public abstract class BaseTimedStructure
{
    #region SweepSystem
    protected abstract Action m_sweepStrutureAction {get; set;}
    private bool m_sweepRunning = false;
    private async void Sweep(float secondsBetweenSweeps) {
        if (m_sweepRunning) return;
        m_sweepRunning = true;
        //AppDomain.CurrentDomain.ProcessExit += StopSweep; // This does not work in the editor
        Application.quitting += StopSweep; // unity only
        while(m_sweepRunning) {
            m_sweepStrutureAction();

            int delay = ((int)(secondsBetweenSweeps * 1000) <= 0) ? 1 : (int)(secondsBetweenSweeps * 1000);
            await Task.Delay(delay);
        }

        //AppDomain.CurrentDomain.ProcessExit -= StopSweep; // This does not work in the editor
        Application.quitting -= StopSweep; // unity only
    }

    public void StartSweep(float secondsBetweenSweeps) => Sweep(secondsBetweenSweeps);
    public void StopSweep() => m_sweepRunning = false;
    //public void StopSweep(object sender, EventArgs e) => m_sweepRunning = false; // This does not work in the editor
    #endregion 
}