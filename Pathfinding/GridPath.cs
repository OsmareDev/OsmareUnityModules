using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GridPath : MonoBehaviour
{
    public Grid grid;

    private bool[,] m_gridData;

    [SerializeField] private Color m_colorForWalkable = Color.green;
    [SerializeField] private Color m_colorForNotWalkable = Color.red;

    public int nColumns = 1, nRows = 1;
    [SerializeField] private string m_docName;
    [SerializeField] private LayerMask m_collisionLayer;

    public void Start() {
        SaveSystem.Load<bool[,]>(m_docName, ref m_gridData);
    }

    [HideInInspector] public float progress;
    public async void ScanGridCollisions() {
        for (int i = 0; i < nColumns; ++i) {
            for (int j = 0; j < nRows; ++j) {
                if (Physics2D.OverlapBox((Vector2)transform.position + new Vector2(i * grid.cellSize.x, j * grid.cellSize.y) + (Vector2)grid.cellSize/2, grid.cellSize * 0.9f, 0f, m_collisionLayer)) 
                    m_gridData[i,j] = false;
                else 
                    m_gridData[i,j] = true;
            }
            progress = (i + 1) / (float)nColumns;
            await Task.Yield();
        }

        SaveSystem.Save<bool[,]>(m_docName, m_gridData);
        progress = 1f;
        SceneView.RepaintAll();
    }

    public void PopulateArray() {
        if (m_gridData == null && !SaveSystem.Load<bool[,]>(m_docName, ref m_gridData))
            m_gridData = new bool[nColumns, nRows];
        else {
            if (m_gridData.GetLength(0) != nColumns || m_gridData.GetLength(0) != nRows) {
                bool[,] aux = m_gridData;
                m_gridData = new bool[nColumns, nRows];
                ArrayHelpers.CopyElements<bool>(aux, m_gridData);
                SaveSystem.Save<bool[,]>(m_docName, m_gridData);
            }
        }
    }

    private void OnDrawGizmos() {
        if (Selection.activeGameObject != gameObject) return;

        for (int i = 0; i < nColumns; ++i) {
            for (int j = 0; j < nRows; ++j) {
                
                if (m_gridData[i,j]) {
                    Gizmos.color = m_colorForWalkable;
                } else {
                    Gizmos.color = m_colorForNotWalkable;
                }

                Gizmos.DrawCube(transform.position + new Vector3(i * grid.cellSize.x, j * grid.cellSize.y) + grid.cellSize/2, grid.cellSize * 0.9f);
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GridPath))]
class GridPathEditor : Editor {
    public void OnEnable() {
        GridPath gridManager = (GridPath)target;
        gridManager.PopulateArray();
    }

    public override void OnInspectorGUI() {
        GridPath gridManager = (GridPath)target;

        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck()) {
            gridManager.PopulateArray();
        }

        if (GUILayout.Button("Scan Grid Collisions"))
        {
            gridManager.ScanGridCollisions();
        }
        EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), gridManager.progress, gridManager.progress.ToString(" completed"));
    }
}
#endif