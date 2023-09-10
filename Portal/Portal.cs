using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal m_nextPortal;
    public Portal NextPortal {
        get {return m_nextPortal;}
        set {m_nextPortal = value;}
    }
    
    [SerializeField] private float m_semiEjeVertical = 1f;
    [SerializeField] private float m_semiEjeHorizontal = 1f;

    [SerializeField] private LayerMask m_ignoreMask;

    private static DictionaryManager<Collider2D, GameObject> m_decoyList;
    //TODO : make that you can change the value from the inspector
    [SerializeField] public static float timeThreshold = 0.1f;

    class PortalConection {
        public Portal conectedPortal;
        public GameObject go;

        public PortalConection(Portal p, GameObject go) {
            this.conectedPortal = p;
            this.go = go;
        }

        ~PortalConection() {
            GameObject.Destroy(go);
        }
    }

    void Start() {
        if (m_decoyList == null) m_decoyList = new DictionaryManager<Collider2D, GameObject>(timeThreshold);
    }

    void Update() { 
        CheckPortal();
        m_decoyList.SweepDictionary();
    }

    #region PortalInterchanges
    void CheckPortal() {
        List<Collider2D> collidersInside = OverlapEllipse();

        collidersInside.ForEach( 
            col => {
                if (m_decoyList.TryGetValue(col, out GameObject decoy)) {
                    decoy.transform.position = PositionInConectedPortal(col);
                } else {
                    GameObject go = col.gameObject;
                    Vector2 newPos = PositionInConectedPortal(col);
                    go = Instantiate(go, newPos, go.transform.rotation);
                    DeactivateAllComponents(go);
                    m_decoyList[col] = go;
                }
            }
        );
    }

    Vector2 PositionInConectedPortal(Collider2D col) {
        Vector2 relativePos = col.transform.position - transform.position;

        if (IsInsideEllipse(relativePos)) col.transform.position = (Vector2)m_nextPortal.transform.position + RelativeToConnectedPortal(m_nextPortal, relativePos);
        else relativePos = RelativeToConnectedPortal(m_nextPortal, relativePos);

        return (Vector2)m_nextPortal.transform.position + relativePos;
    }

    private Vector2 RelativeToConnectedPortal(Portal portal, Vector3 point)
    {
        float relativeH = portal.m_semiEjeHorizontal/m_semiEjeHorizontal;
        float relativeV = portal.m_semiEjeVertical/m_semiEjeVertical;

        point.x = point.x * relativeH;
        point.y = point.y * relativeV;

        float normalizedX = point.x / (portal.m_semiEjeHorizontal/2);
        float normalizedY = point.y / (portal.m_semiEjeVertical/2);

        float hypotenusa = (normalizedX * normalizedX + normalizedY * normalizedY);

        if (hypotenusa <= 1f) return -point * (1 + (1 - hypotenusa));
        else return -point * (1 - (hypotenusa - 1));
    }

    private void DeactivateAllComponents(GameObject go) {
        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        RectTransform rectTransform = go.GetComponent<RectTransform>();

        Component[] allComponents = go.GetComponents<Component>();
        foreach (Component component in allComponents)
        {
            if (component != spriteRenderer && component != rectTransform)
            {
                Destroy(component); 
            }
        }
    }

    #endregion

    #region EllipseCalculations

    private List<Collider2D> OverlapEllipse() {
        float biggerRadius = (m_semiEjeHorizontal/2 > m_semiEjeVertical/2) ? m_semiEjeHorizontal/2 : m_semiEjeVertical/2;
        List<Collider2D> possibleColliders = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, biggerRadius, ~m_ignoreMask));
    
        return possibleColliders.Where( col => IsColliderInsideEllipse(col)).ToList();
    }

    private bool IsColliderInsideEllipse(Collider2D collider)
    {
        Vector3 colliderCenter = collider.bounds.center;
        Vector3 offset = colliderCenter - transform.position;

        Vector3[] colliderCorners = GetColliderCorners(collider);
        foreach (Vector3 corner in colliderCorners)
        {
            Vector3 pointRelativeToEllipse = corner - transform.position;
            if (IsInsideEllipse(pointRelativeToEllipse))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsInsideEllipse(Vector3 point)
    {
        float normalizedX = point.x / (m_semiEjeHorizontal/2);
        float normalizedY = point.y / (m_semiEjeVertical/2);

        return normalizedX * normalizedX + normalizedY * normalizedY <= 1.0f;
    }

    private Vector3[] GetColliderCorners(Collider2D collider) {
        Bounds bounds = collider.bounds;
        Vector3[] corners = new Vector3[4];
        corners[0] = new Vector3(bounds.min.x, bounds.min.y, 0);
        corners[1] = new Vector3(bounds.min.x, bounds.max.y, 0);
        corners[2] = new Vector3(bounds.max.x, bounds.min.y, 0);
        corners[3] = new Vector3(bounds.max.x, bounds.max.y, 0);
        return corners;
    }
    #endregion

    private void OnDrawGizmos() {
        float biggerRadius = (m_semiEjeHorizontal/2 > m_semiEjeVertical/2) ? m_semiEjeHorizontal/2 : m_semiEjeVertical/2;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, biggerRadius);
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(Portal))]
class PortalEditor : Editor {

    SerializedProperty m_nextPortal, m_semiEjeVertical, m_semiEjeHorizontal, m_ignoreMask, m_timeThreshold;

    private void OnEnable() {
        m_nextPortal = serializedObject.FindProperty("m_nextPortal");
        m_semiEjeHorizontal = serializedObject.FindProperty("m_semiEjeHorizontal");
        m_semiEjeVertical = serializedObject.FindProperty("m_semiEjeVertical");
        m_ignoreMask = serializedObject.FindProperty("m_ignoreMask");
        m_timeThreshold = serializedObject.FindProperty("m_timeThreshold");

        Portal script = (Portal)target;
        serializedObject.Update();
        m_semiEjeHorizontal.floatValue = script.transform.localScale.x;
        m_semiEjeVertical.floatValue = script.transform.localScale.y;
        serializedObject.ApplyModifiedProperties();

    }

    public override void OnInspectorGUI() {
        Portal script = (Portal)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_nextPortal, true);
        if (m_nextPortal.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("This portal does not connect with another", MessageType.Warning);
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_semiEjeHorizontal, true);
        EditorGUILayout.PropertyField(m_semiEjeVertical, true);
        if (EditorGUI.EndChangeCheck()) {
            script.transform.localScale = new Vector3(m_semiEjeHorizontal.floatValue, m_semiEjeVertical.floatValue, script.transform.lossyScale.z);
        }
        if (m_semiEjeHorizontal.floatValue != script.transform.localScale.x) m_semiEjeHorizontal.floatValue = script.transform.localScale.x;
        if (m_semiEjeVertical.floatValue != script.transform.localScale.y) m_semiEjeVertical.floatValue = script.transform.localScale.y;

        EditorGUILayout.PropertyField(m_ignoreMask, true);

        //Portal.timeThreshold = EditorGUILayout.FloatField("Time threshold", Portal.timeThreshold);
        //if (Portal.timeThreshold < 0.01f) Portal.timeThreshold = 0.01f;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion