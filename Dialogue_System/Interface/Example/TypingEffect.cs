using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class TypingEffect : MonoBehaviour, ITypeEffect
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private int m_lettersPerSecond = 10;

    private bool m_instaFinish = false;
    private Task m_task;

    public void Clear() => m_text.text = "";

    public async void Begin(string text) {
        if (m_task != null) await m_task;
        m_task = Type(text);
    }

    public bool Check() {
        if (m_task != null) return m_task.IsCompleted;
        else return true;
    }

    public void Finish() => m_instaFinish = true;

    private async Task Type(string text)
    {
        Clear();

        foreach (char letter in text) {
            if (m_instaFinish) break;

            m_text.text += letter;
            if (letter == ' ') continue;
            await Task.Delay(1000/m_lettersPerSecond);
        }
        
        m_text.text = text;
        m_instaFinish = false;
    }
}
