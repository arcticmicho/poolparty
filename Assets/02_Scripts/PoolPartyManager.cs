using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PoolPartyManager : MonoBehaviour {

    private static PoolPartyManager m_instance;
    public static PoolPartyManager Instance
    {
        get 
        {
            if(m_instance == null)
            {
                PoolPartyManager poolParty = GameObject.FindObjectOfType(typeof(PoolPartyManager)) as PoolPartyManager;
                if(poolParty != null)
                {
                    m_instance = poolParty;
                }else
                {
                    Debug.LogError("Couldn't find a PoolPartyManager instance in the scene. Please add a PoolPartyManager prefab to the scene");
                }
            }

            return m_instance;
        }
    }

 

    [SerializeField]
    private List<string> m_keys = new List<string>();
    public List<string> Keys
    {
        get { return m_keys; }
    }

    [SerializeField]
    private List<PoolObject> m_values = new List<PoolObject>();
    public List<PoolObject> Values
    {
        get { return m_values; }
    }

	void Start () 
    {
	
	}

    public GameObject GetPoolableObject(string key)
    {
        if (m_keys.Contains(key))
        {
            int index = m_keys.IndexOf(key);
            return m_values[index].GetPoolObject();
        }else
        {
            Debug.LogError("Couldn't find a pool with key: " + key);
            return null;
        }
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        if (m_keys.Contains(key))
        {
            int index = m_keys.IndexOf(key);
            PoolObject pool = m_values[index];
            bool succes = pool.ReturnToPool(obj);
            if(!succes)
            {
                Debug.LogError("Trying to return a gameObject to a pool that doesn't belong. Are you using the right key? or is the gameObject a poolable object?");
            }
        }
    }

    public void AddPool(PoolObject pool)
    {
        if (m_keys != null && !m_keys.Contains(pool.PoolName))
        {
            m_values.Add(pool);
            m_keys.Add(pool.PoolName);
        }
    }

    public bool RemovePool(PoolObject pool)
    {
        if (m_keys.Contains(pool.PoolName))
        {
            int index = m_keys.IndexOf(pool.PoolName);
            m_keys.Remove(pool.PoolName);
            m_values.RemoveAt(index);

            return true;
        }
        else
        {
            return false;
        }
    }
	

}