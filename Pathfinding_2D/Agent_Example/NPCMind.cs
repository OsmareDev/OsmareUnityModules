using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMind : MonoBehaviour
{
    List<Vector3> path = new List<Vector3>();
    int pathIndex;

    [SerializeField] private GridPath m_path;
    [SerializeField] private Transform m_target;
    [SerializeField] private TopDown2DCollisionController m_cc;
    [SerializeField] private float m_timeForActualizePath = 0.5f;
    private float m_lastTimeActualization = 0f;

    // Update is called once per frame
    void Update()
    {
        ActualizePath();
        if (path.Count == 0) {
            Debug.Log(transform.name);
            return;
        }
        if (Vector3.Distance(path[pathIndex], transform.position) < 0.1) pathIndex++;
        if (Vector3.Distance(transform.position, m_target.position) < 1) {
            Debug.Log("te tengo");
            return;
        }
        
        if (pathIndex < path.Count) {
            Vector3 direction = (path[pathIndex] - transform.position).normalized;
            m_cc.Move(direction * 5 * Time.deltaTime);
        } else {
            pathIndex--;
        }
    }

    void ActualizePath() {
        if (m_lastTimeActualization < Time.time) {
            m_lastTimeActualization = Time.time + m_timeForActualizePath;

            path = m_path.FindPath(transform.position, m_target.position);
            pathIndex = 0;
        }
    }

    void OnDrawGizmos() {
        if (!Application.isPlaying) return;

        path = m_path.FindPath(transform.position, m_target.position);

        
        Vector3 lastPos = transform.position;
        foreach (Vector3 pos in path) {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(lastPos, pos);
            lastPos = pos;

            Gizmos.color = Color.red;
            Node nodo = m_path.grid.GetElementFromWorldCoordinates(pos);
            foreach (Node n in nodo.neightbors) {
                Vector3 nPos = m_path.grid.GridToWorldCoordinates(new Vector2(n.GridX, n.GridY));
                //Gizmos.DrawLine(pos, nPos);
            }
        }
        // Gizmos.DrawCube(pos, new Vector3(1,1,1));
    }
}
