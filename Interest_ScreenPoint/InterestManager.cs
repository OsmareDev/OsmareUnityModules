using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Threading.Tasks;

public class InterestManager : MonoBehaviour
{
    [SerializeField] private List<Transform> m_points;
    private IInteractable m_currentPoint;
    [SerializeField] private Transform m_visualizator;
    [SerializeField] private RectTransform m_canvas;
    [SerializeField] private float m_range = 20f;
    [SerializeField] private Camera m_camera;


    [SerializeField] private Image m_selectorInCanvas;
    [SerializeField] private float m_refreshSeconds = 0.1f;
    private bool m_running = true, m_inside = false;

    private TimedTask m_task;

    public void Start() {
        m_task = new TimedTask(ShowPoint);
    }
    public void StartTask() {
        m_task.StartSweep(m_refreshSeconds);
    }
    public void StopTask() {
        m_task.StopSweep();
    }
    public void Interact() {
        if (m_currentPoint != null && m_running && m_inside) m_currentPoint.Interact();
    }
    public void SetPoint(Transform t) => m_points.Add(t);

    private void ShowPoint() {
        m_selectorInCanvas.enabled = false;
        m_inside = false;

        Transform m_nearestPointToShow = m_points.OrderBy(tr => Vector3.Distance(tr.position, m_visualizator.position)).Take(1).ToList()[0];
        Vector3 direction = m_nearestPointToShow.position - m_visualizator.position;
        
        // if the point is backwards dont calculate the position in screen, it may lead to bad results
        if (Vector3.Dot(direction, m_camera.transform.forward) < 0) return;

        RaycastHit hit;

        if (Physics.Raycast(m_visualizator.position, direction.normalized, out hit, m_range) && hit.transform == m_nearestPointToShow) {
            hit.collider.TryGetComponent<IInteractable>(out m_currentPoint);

            // mostrar
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(m_camera, m_nearestPointToShow.position);
            Debug.Log(screenPos);
            if (RectTransformUtility.RectangleContainsScreenPoint(m_canvas, screenPos)) {
                m_selectorInCanvas.rectTransform.position = screenPos;
                m_selectorInCanvas.enabled = true;
                m_inside = true;
            }
        }
    }
}
