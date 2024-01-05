using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GridPath : MonoBehaviour
{
    public Grid2D<Node> grid;
    [SerializeField] private Transform m_origin;
    public Vector2 m_cellSize {
        get {return new Vector2(grid.CellWidth, grid.CellHeight);} 
        set {grid.CellWidth = value.x; grid.CellHeight = value.y;}
    }
    [SerializeField] private Vector2Int m_cellNumber;

    [SerializeField] private Color m_colorForWalkable = Color.green;
    [SerializeField] private Color m_colorForNotWalkable = Color.red;
    [SerializeField] private Color m_colorForNotProcessed = Color.black;

    [SerializeField] private LayerMask m_collisionLayer;
    [SerializeField] private float m_percentageOfTheCellToCheck = 0.9f;
    [SerializeField] private string m_docName;

    #region UnityFunctions
    public void Awake() {
        PopulateArray();
    }

    private void OnDrawGizmos() {
        if (Selection.activeGameObject != gameObject) return;

        int nCols = (m_cellNumber.x > grid.NColumns) ? m_cellNumber.x : grid.NColumns;
        int nRows = (m_cellNumber.y > grid.NRows) ? m_cellNumber.y : grid.NRows;

        for (int i = 0; i < nCols; ++i) {
            for (int j = 0; j < nRows; ++j) {
                
                if (grid.IsValidPosition(i,j)) {
                    if (grid[i,j].walkable) {
                        Gizmos.color = m_colorForWalkable;
                    } else {
                        Gizmos.color = m_colorForNotWalkable;
                    }
                } else {
                    Gizmos.color = m_colorForNotProcessed;
                }

                if (i >= m_cellNumber.x || j >= m_cellNumber.y)
                    Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, Gizmos.color.a * 0.4f);

                Gizmos.DrawCube(grid.GridToWorldCoordinates(new Vector2(i,j)), m_cellSize * m_percentageOfTheCellToCheck);

                if (i < m_cellNumber.x && j < m_cellNumber.y) {
                    Gizmos.color = Color.grey;
                    Gizmos.DrawWireCube(grid.GridToWorldCoordinates(new Vector2(i,j)), m_cellSize);
                }
            }
        }
    }
    #endregion

    #region ScanGrid
    [HideInInspector] public float progress;
    public async void ScanGridCollisions() {
        grid = new Grid2D<Node>(m_cellNumber.x, m_cellNumber.y, m_cellSize, m_origin.position);

        await CheckIfCellsAreWalkable();

        SaveSystem.Save<Grid2D<Node>>(m_docName, grid);
        progress = 1f;
        SceneView.RepaintAll();
    }

    private async Task CheckIfCellsAreWalkable() {
        for (int i = 0; i < m_cellNumber.x; ++i) {
            for (int j = 0; j < m_cellNumber.y; ++j) {
                grid[i,j].GridX = i;
                grid[i,j].GridY = j;

                if (Physics2D.OverlapBox(grid.GridToWorldCoordinates(new Vector2(i, j)), m_cellSize * m_percentageOfTheCellToCheck, 0f, m_collisionLayer)) {
                    grid[i,j].walkable = false;
                } else {
                    grid[i,j].walkable = true;
                } 
                
                CheckNeighbours(i, j);
            }
            progress = ((i + 1) / (float)m_cellNumber.x);
            await Task.Yield();
        }
    }

    private void CheckNeighbours(int x, int y) {
        // we are going to check only the scanned neighbours
        Vector2Int[] dxy = {new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 1)};

        for (int i = 0; i < dxy.GetLength(0); ++i) {
            if (x + dxy[i].x < 0 || y + dxy[i].y < 0 || y + dxy[i].y >= grid.NRows || !grid[x + dxy[i].x, y + dxy[i].y].walkable) continue;
            grid[x, y].neightbors.Add(grid[x + dxy[i].x, y + dxy[i].y]);
            if (grid[x, y].walkable) grid[x + dxy[i].x, y + dxy[i].y].neightbors.Add(grid[x, y]);
        }
    }
    #endregion

    #region Astar
    public List<Vector3> FindPath(Vector3 start, Vector3 target)
    {
        Node startNode = grid.GetElementFromWorldCoordinates(start);
        Node targetNode = grid.GetElementFromWorldCoordinates(target);

        //Using an SortedSet we dont need to compare in each iteration everithing, just once
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);
        // int cont = 0;

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost) {
					if (openSet[i].hCost < currentNode.hCost)
						currentNode = openSet[i];
				}
			}

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) return RetracePath(startNode, targetNode);

            foreach (Node neighbor in currentNode.neightbors)
            {
                //if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;
                if (closedSet.Contains(neighbor)) continue;

                int newMovementCostToNeighbor = (int)currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.fCost = neighbor.gCost + neighbor.hCost;
                    neighbor.parent = currentNode;

                    openSet.Remove(neighbor);
                    openSet.Add(neighbor);
                }
            }
        }

        Debug.Log("no encontrado"); 
        return new List<Vector3>();
    }

    private List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(grid.GridToWorldCoordinates(new Vector2(currentNode.GridX, currentNode.GridY)));
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY) return (14 * dstY) + 10 * (dstX - dstY);

        return (14 * dstX) + 10 * (dstY - dstX);
    }
    #endregion

    public void PopulateArray() {
        if (grid.ArrayHasBeenDeleted()) grid = null;
        if (SaveSystem.Load<Grid2D<Node>>(m_docName, ref grid))
            m_cellNumber = new Vector2Int(grid.NColumns, grid.NRows);        
        else if (grid == null)
            grid = new Grid2D<Node>(m_cellNumber.x, m_cellNumber.y, new Vector2(1, 1), (m_origin) ? m_origin.position : transform.position);
    }
}

#region UnityEditor
#if UNITY_EDITOR
[CustomEditor(typeof(GridPath))]
class GridPathEditor : Editor {
    SerializedProperty m_origin, m_cellNumber, m_colorForWalkable, m_colorForNotWalkable, m_colorForNotProcessed, m_collisionLayer, m_docName, m_percentageOfTheCellToCheck;

    public void OnEnable() {
        GridPath gridManager = (GridPath)target;
        gridManager.PopulateArray();

        m_origin = serializedObject.FindProperty("m_origin");
        m_cellNumber = serializedObject.FindProperty("m_cellNumber");

        m_colorForWalkable = serializedObject.FindProperty("m_colorForWalkable");
        m_colorForNotWalkable = serializedObject.FindProperty("m_colorForNotWalkable");
        m_colorForNotProcessed = serializedObject.FindProperty("m_colorForNotProcessed");

        m_collisionLayer = serializedObject.FindProperty("m_collisionLayer");
        m_docName = serializedObject.FindProperty("m_docName");
        m_percentageOfTheCellToCheck = serializedObject.FindProperty("m_percentageOfTheCellToCheck");
    }

    public override void OnInspectorGUI() {
        GridPath script = (GridPath)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_origin, true);
        if (m_origin.objectReferenceValue == null) m_origin.objectReferenceValue = script.transform;
        script.grid.OriginPosition = ((Transform)m_origin.objectReferenceValue).position; // actualizamos la posicion del grid desde el inspector

        EditorGUI.BeginChangeCheck();
        Vector2 cellSize = EditorGUILayout.Vector2Field("cell size ", new Vector2(script.grid.CellWidth, script.grid.CellHeight));
        if (EditorGUI.EndChangeCheck()) {
            script.grid.CellWidth = cellSize.x;
            script.grid.CellHeight = cellSize.y;
            if (script.grid.CellWidth < 0) script.grid.CellWidth = 0;
            if (script.grid.CellHeight < 0) script.grid.CellHeight = 0;
        }

        EditorGUILayout.PropertyField(m_cellNumber, true);
        if (m_cellNumber.vector2IntValue.x < 0) m_cellNumber.vector2IntValue = new Vector2Int(0, m_cellNumber.vector2IntValue.y);
        if (m_cellNumber.vector2IntValue.y < 0) m_cellNumber.vector2IntValue = new Vector2Int(m_cellNumber.vector2IntValue.x, 0);

        EditorGUILayout.PropertyField(m_colorForWalkable, true);
        EditorGUILayout.PropertyField(m_colorForNotWalkable, true);
        EditorGUILayout.PropertyField(m_colorForNotProcessed, true);

        EditorGUILayout.PropertyField(m_collisionLayer, true);
        EditorGUILayout.PropertyField(m_percentageOfTheCellToCheck, true);
        Mathf.Clamp01(m_percentageOfTheCellToCheck.floatValue);
        
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_docName, true);
        bool checkFile = EditorGUI.EndChangeCheck();

        if (GUILayout.Button("Scan Grid Collisions"))
        {
            script.ScanGridCollisions();
        }
        EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), script.progress, script.progress.ToString(" completed"));

        serializedObject.ApplyModifiedProperties();

        if (checkFile) script.PopulateArray();
    }
}
#endif
#endregion