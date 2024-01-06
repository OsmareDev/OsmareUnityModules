using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PathController : MonoBehaviour
{
    public enum Dimensions {
        _2D,
        _3D
    }

    [SerializeField] private Dimensions m_dimension;
    [SerializeField] private bool m_useNavMesh;


    [SerializeField] private Transform m_from;
    [SerializeField] private List<Transform> m_to;

    [SerializeField] private LineRenderer m_linePath;
    [SerializeField] private float m_updatePathEveryXSeconds;

    [SerializeField] private UnityEngine.Object m_retrievePathScript;
    [SerializeField] private string m_functionName;

    private NavMeshTriangulation m_triangulation;
    private TimedTask m_drawPathTask;
    private Func<Vector3, Vector3, List<Vector3>> m_retrievePathFunction;


    #region PointManipulation
    public void SetFrom(Transform from) => m_from = from;
    public void AddDestiny(Transform destiny) => m_to.Add(destiny);
    public void ClearDestiny() => m_to.Clear();
    #endregion

    public void Start() {
        m_linePath.enabled = false;
        m_drawPathTask = new TimedTask(DrawPath);
        
        if (m_dimension == Dimensions._2D) {
            m_retrievePathFunction = Helpers.RetrieveFunc<Vector3, Vector3, List<Vector3>>(m_retrievePathScript, m_functionName);
            // ShowPath();
            return;
        }

        // 3D
        if (m_useNavMesh) {
            LoadNavmesh();
            m_retrievePathFunction = RetrieveNavMeshPath;
        } else {
            m_retrievePathFunction = Helpers.RetrieveFunc<Vector3, Vector3, List<Vector3>>(m_retrievePathScript, m_functionName);
        }

        // ShowPath();
    }
    public void ShowPath() {
        m_linePath.enabled = true;
        m_drawPathTask.StartSweep(m_updatePathEveryXSeconds);
    }

    public void HidePath() { 
        m_linePath.enabled = false;
        m_drawPathTask.StopSweep();
    }

    private void LoadNavmesh() {
        m_triangulation = NavMesh.CalculateTriangulation();
    }

    private void DrawPath() {
        // if there is no points left returns 
        if (m_to.Count( tr => tr != null ) < 1 ) return;

        // gets the closest one
        Vector3 destination = m_to.Where(tr => tr != null)
                                    .OrderBy(tr => Vector3.Distance(tr.position, m_from.position))
                                    .Take(1).ToList()[0].position;

        List<Vector3> path = m_retrievePathFunction(m_from.position, destination);

        m_linePath.positionCount = path.Count;
        int cont = 0;
        path.ForEach( point => {
            m_linePath.SetPosition(cont, point);
            cont++;
        });
    }

    private List<Vector3> RetrieveNavMeshPath(Vector3 begining, Vector3 destination) {
        NavMeshPath path = new NavMeshPath();
        List<Vector3> followPath = new List<Vector3>();

        if (NavMesh.CalculatePath(m_from.position, destination, NavMesh.AllAreas, path)) {
            followPath = new List<Vector3>(path.corners);
        }   

        followPath.Add(destination);
        return followPath;
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(PathController))]
class PathControllerEditor : Editor {

    SerializedProperty m_dimension, m_useNavMesh, m_from, m_to, m_linePath, m_updatePathEveryXSeconds, m_retrievePathScript, m_functionName;

    private void OnEnable() {
        m_dimension = serializedObject.FindProperty("m_dimension");
        m_useNavMesh = serializedObject.FindProperty("m_useNavMesh");
        m_from = serializedObject.FindProperty("m_from");
        m_to = serializedObject.FindProperty("m_to");
        m_linePath = serializedObject.FindProperty("m_linePath");
        m_updatePathEveryXSeconds = serializedObject.FindProperty("m_updatePathEveryXSeconds");
        m_retrievePathScript = serializedObject.FindProperty("m_retrievePathScript");
        m_functionName = serializedObject.FindProperty("m_functionName");
    }

    public override void OnInspectorGUI() {
        PathController script = (PathController)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_linePath, new GUIContent("Line Render For the path"), true);
        if (m_linePath.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no Line", MessageType.Error);

            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorGUILayout.PropertyField(m_from, new GUIContent("Origin Of The Path"), true);
        EditorGUILayout.PropertyField(m_to, new GUIContent("Destination Of The Path"), true);
        EditorGUILayout.PropertyField(m_updatePathEveryXSeconds, new GUIContent("Update Path Every X Seconds"), true);
        if (m_updatePathEveryXSeconds.floatValue <= 0) m_updatePathEveryXSeconds.floatValue = float.MinValue;

        EditorGUILayout.PropertyField(m_dimension, new GUIContent("Dimensions"), true);
        EditorGUI.indentLevel++;
        if (m_dimension.enumValueIndex == 0) {
            // 2D
            EditorGUILayout.PropertyField(m_functionName, new GUIContent("Name of the function to collect path"), true);
            EditorHelpers.CollectAnyThingWithTheFunction(ref m_retrievePathScript, m_functionName.stringValue, "Retrieve Path Script");
            if (m_retrievePathScript.objectReferenceValue == null)
                EditorGUILayout.HelpBox("There is no Script to get the path from, make sure to write the function correctly", MessageType.Error);
        } else {
            // 3D
            EditorGUILayout.PropertyField(m_useNavMesh, new GUIContent("Use Unity NavMesh"), true);
            if (!m_useNavMesh.boolValue) {
                EditorGUILayout.PropertyField(m_functionName, new GUIContent("Name of the function to collect path"), true);
                EditorHelpers.CollectAnyThingWithTheFunction(ref m_retrievePathScript, m_functionName.stringValue, "Retrieve Path Script");
                if (m_retrievePathScript.objectReferenceValue == null)
                    EditorGUILayout.HelpBox("There is no Script to get the path from, make sure to write the function correctly", MessageType.Error);
            }
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion