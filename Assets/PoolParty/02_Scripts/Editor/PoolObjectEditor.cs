using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PoolObject))]
[CanEditMultipleObjects]
public class PoolObjectEditor : Editor {

    private PoolObject m_reference;

    public override void OnInspectorGUI()
    {
        if(m_reference == null)
        {
            m_reference = (PoolObject)target;
        }

        EditorGUILayout.LabelField("Pool name: ", m_reference.PoolName);
        EditorGUILayout.ObjectField("Game object: ", m_reference.PoolableObject, typeof(GameObject), false);
        EditorGUILayout.IntField("Initial count: ", m_reference.InitialCount);
        EditorGUILayout.Toggle("Incremental: ", m_reference.IncrementalPool);
    }
}
