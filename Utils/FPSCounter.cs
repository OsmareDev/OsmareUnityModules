using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private float timer;
    private int frames = 0;
    [SerializeField] private TMP_Text m_text;

    void Start() {
        timer = 1f;
    }

    void Update()
    {
        frames++;

        if (timer < Time.time) {
            timer = Time.time + 1f;
            m_text.text = frames.ToString();
            frames = 0;
        }
    }
}
