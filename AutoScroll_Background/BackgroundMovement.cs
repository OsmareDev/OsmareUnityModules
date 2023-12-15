using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private bool m_counterRotation;
    [SerializeField] private bool m_horizontal = false;

    [SerializeField] private RawImage m_img;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] private float m_initialRotation;
    

    void Start() { 
        if (m_img == null) return;
        
        m_img.gameObject.transform.eulerAngles = m_img.gameObject.transform.eulerAngles + new Vector3(0f, 0f, m_initialRotation);
    }

    void Update()
    {
        if (m_img == null) return;
        
        Vector2 amount;
        m_img.gameObject.transform.eulerAngles = m_img.gameObject.transform.eulerAngles + new Vector3(0f, 0f, m_rotationSpeed) * Time.deltaTime;
        
        if (m_horizontal) {
            amount = new Vector2(m_speed, 0f);

            if (m_counterRotation) {
                amount = Quaternion.Euler(0f, 0f, -m_img.gameObject.transform.rotation.eulerAngles.z) * Vector2.right * m_speed;
            }
        } else {
            amount = new Vector2(0f, m_speed);

            if (m_counterRotation) {
                amount = Quaternion.Euler(0f, 0f, -m_img.gameObject.transform.rotation.eulerAngles.z) * Vector2.up * m_speed;
            }
        }

        m_img.uvRect = new Rect(m_img.uvRect.position + amount * Time.deltaTime, m_img.uvRect.size);
    }

    #region Tests

    public void SetSpeed(float value) => m_speed = value;
    public void SetImage(RawImage value) => m_img = value;

    #endregion
}
