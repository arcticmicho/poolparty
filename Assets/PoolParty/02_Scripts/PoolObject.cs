using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PoolObject : MonoBehaviour {

    [SerializeField][HideInInspector]
    private List<GameObject> m_activeObjects;
    [SerializeField][HideInInspector]
    private List<GameObject> m_inactiveObjects;
    [SerializeField]
    private string m_poolName;
    public string PoolName
    {
        get
        {
            return m_poolName;
        }
        set
        {
            m_poolName = value;
        }
    }

    [SerializeField]
    private GameObject m_poolableObject = null;
    public GameObject PoolableObject
    {
        set
        {
            m_poolableObject = value;
        }
        get
        {
            return m_poolableObject;
        }
    }

    [SerializeField]
    private int m_initialCount;    
    public int InitialCount
    {
        set
        {
            m_initialCount = value;
        }
        get
        {
            return m_initialCount;
        }
    }

    [SerializeField]
    private bool m_incrementalPool;
    public bool IncrementalPool
    {
        set
        {
            m_incrementalPool = value;
        }
        get
        {
            return m_incrementalPool;
        }
    }

    public PoolObject(string poolName, GameObject poolableObject, int initialCount, bool incrementalPool)
    {
        m_poolName = poolName;
        m_poolableObject = poolableObject;
        m_initialCount = initialCount;
        m_incrementalPool = incrementalPool;
    }

	void Start () 
    {
        m_activeObjects = new List<GameObject>();
        m_inactiveObjects = new List<GameObject>();

        for(int i=0; i<m_initialCount; i++)
        {
            GameObject obj = GameObject.Instantiate(m_poolableObject);
            obj.transform.SetParent(gameObject.transform, false);
            obj.SetActive(false);
            m_inactiveObjects.Add(obj);
        }
	}

    public GameObject GetPoolObject()
    {
        if(m_inactiveObjects.Count > 0)
        {
            GameObject obj = m_inactiveObjects[0];
            m_inactiveObjects.Remove(obj);
            m_activeObjects.Add(obj);
            return obj;
        }else if(m_incrementalPool)
        {
            GameObject obj = GameObject.Instantiate(m_poolableObject);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            m_activeObjects.Add(obj);
            Debug.LogWarning("Creating another instance of the object!");
            return obj;
        }else
        {
            return null;
        }
    }

    public bool ReturnToPool(GameObject obj)
    {
        if(m_activeObjects.Contains(obj))
        {
            m_activeObjects.Remove(obj);
            obj.SetActive(false);
            obj.transform.parent = transform;
            m_inactiveObjects.Add(obj);
            return true;
        }else
        {
            return false;
        }
    }
	
}
