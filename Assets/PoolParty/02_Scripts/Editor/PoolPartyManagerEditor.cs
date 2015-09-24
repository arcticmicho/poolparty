using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;


public struct ResultMessage
{
    public string m_resultMessage;
    public bool m_isError;
    
    public ResultMessage(string message, bool isError)
    {
        m_resultMessage = message;
        m_isError = isError;
    }
}

[CustomEditor(typeof(PoolPartyManager))]
[CanEditMultipleObjects]
public class PoolPartyManagerEditor : Editor {

    private PoolPartyManager reference;
    private PoolObject newPoolObject;

    private const string POOL_SUFFIX = "_Pool";

    private string m_newPoolName;
    private Object m_newGameObject;
    private int m_newInitialCount;
    private bool m_newIncremental;
    private ResultMessage m_result;

    public Vector2 scrollPosition = Vector2.zero;

    private GUIStyle labelStyle = null;
    private GUIStyle labelStyleAddPool = null;
    private GUIStyle resultLabelStyle = null;
    
    public void Awake()
    {
        
    }

	public override void OnInspectorGUI()
    {
        if(!Application.isPlaying){

            if(labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.GetStyle("flow var 3"));
                labelStyle.fontSize = 10;
            }
            if(labelStyleAddPool == null)
            {
                labelStyleAddPool = new GUIStyle(GUI.skin.GetStyle("flow var 1"));
                labelStyleAddPool.fontSize = 10;
                labelStyleAddPool.fontStyle = FontStyle.Bold;
            }
            if(resultLabelStyle == null)
            {
                resultLabelStyle = new GUIStyle();
            }
            GUILayout.BeginHorizontal(labelStyleAddPool);
            GUILayout.Label("Registred Pools");
            GUILayout.EndHorizontal();
            GUILayout.Box("Add a Pool", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });

            
            if(reference == null)
            {
                reference = (PoolPartyManager) target;
            }

            if (reference.Keys != null && reference.Values != null)
            {
                foreach (string key in reference.Keys)
                {
                    int index = reference.Keys.IndexOf(key);
                    PoolObject pool = reference.Values[index];
                    GUILayout.BeginVertical();
                    EditorGUILayout.LabelField("Pool name: ", pool.PoolName);
                    EditorGUILayout.ObjectField("Game object: ", pool.PoolableObject, typeof(GameObject), false);
                    EditorGUILayout.IntField("Initial count: ", pool.InitialCount);
                    EditorGUILayout.Toggle("Incremental: ", pool.IncrementalPool);

                    if (GUILayout.Button("Remove Pool"))
                    {
                        Debug.LogWarning("Pool to remove: " + pool.PoolName);
                        bool result = RemovePool(pool);
                        break;
                    }
                    GUILayout.EndVertical();
                    GUILayout.Box("Add a Pool", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                    EditorGUILayout.Space();
                } 
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal(labelStyle);
            EditorGUILayout.LabelField("Add a Pool: ");
            GUILayout.EndHorizontal();
            GUILayout.Box("Add a Pool", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });

            GUILayout.BeginVertical();
            m_newPoolName = EditorGUILayout.TextField("Pool name: ", m_newPoolName);
            m_newGameObject = EditorGUILayout.ObjectField("GameObject: ", m_newGameObject, typeof(GameObject), false);
            m_newInitialCount = EditorGUILayout.IntField("Initial count: ", m_newInitialCount);
            m_newIncremental = EditorGUILayout.Toggle("Incremental: ", m_newIncremental);
            if(GUILayout.Button("Add Pool"))
            {
                m_result = AddPoolToPoolManager(reference);
                
            }
            GUILayout.EndVertical();
            EditorGUILayout.Space();

            if(m_result.m_isError)
            {
                resultLabelStyle.normal.textColor = Color.red;
            }else
            {
                resultLabelStyle.normal.textColor = Color.green;
            }
            EditorGUILayout.LabelField(m_result.m_resultMessage, resultLabelStyle);
        }else
        {
            EditorGUILayout.HelpBox("PoolPartyManager can't be edited when the game is running", MessageType.Info);
        }
    }

    private ResultMessage AddPoolToPoolManager(PoolPartyManager poolManager)
    {
        if(!poolManager.Keys.Contains(m_newPoolName))
        {
            if(m_newGameObject != null)
            {
                if (!string.IsNullOrEmpty(m_newPoolName))
                {
                    GameObject obj = new GameObject(m_newPoolName + POOL_SUFFIX);
                    PoolObject pool = obj.AddComponent<PoolObject>();
                    pool.PoolName = m_newPoolName;
                    pool.PoolableObject = m_newGameObject as GameObject;
                    pool.InitialCount = m_newInitialCount;
                    pool.IncrementalPool = m_newIncremental;
                    obj.transform.parent = poolManager.gameObject.transform;

                    poolManager.AddPool(pool);
                    ResetNewPoolValues();
                    bool result = UpdatePoolObjectsType();
                    if(result)
                    {
                        return new ResultMessage("Success!",false);
                    }else
                    {
                        return new ResultMessage("Error trying to write the Enum file, please verify that you have the permission to write over this file and try again.",true);
                    }

                    
                }else
                {
                    return new ResultMessage("Pool name can't be null or empty",true);
                }

                
            }else
            {
                return new ResultMessage("GameObject can't be null",true);
            }
        }else
        {
            return new ResultMessage("Key already exist",true);
        }
    }

    private bool RemovePool(PoolObject pool)
    {
        bool result = reference.RemovePool(pool);
        if(result)
        {
            Transform transform = reference.gameObject.transform.FindChild(pool.PoolName + POOL_SUFFIX);
            if(transform != null)
            {
                DestroyImmediate(transform.gameObject);
            }
            result &= UpdatePoolObjectsType();

        }
        return result;
    }

    private void ResetNewPoolValues()
    {
        m_newPoolName = string.Empty;
        m_newGameObject = null;
        m_newInitialCount = 0;
        m_newIncremental = false;
    }

    private bool UpdatePoolObjectsType()
    {
        return EnumWriterTool.WriteEnumToFile("Assets/PoolParty/02_Scripts/EPoolObjectType.cs", reference.Keys.ToArray(), "EPoolObjectType");
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
}
