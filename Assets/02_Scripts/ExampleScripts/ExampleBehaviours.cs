using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public struct PoolableObject
{
    public PoolableObject(EPoolObjectType key, GameObject gameObject)
    {
        m_key = key;
        m_poolableObject = gameObject;
    }
    public GameObject m_poolableObject;
    public EPoolObjectType m_key;
}

public class ExampleBehaviours : MonoBehaviour
{


    public GameObject m_lastPoolableObject;
    private List<PoolableObject> m_poolableObjects = new List<PoolableObject>();
    public Text m_resultText;
    public EPoolObjectType m_key;

    public void OnAskForPoolObject()
    {
        m_lastPoolableObject = PoolPartyManager.Instance.GetPoolableObject(m_key);
        if (m_lastPoolableObject == null)
        {
            m_resultText.text = "Couldn't find a gameObject with that key";
        }
        else
        {
            m_resultText.text = "Game object found!";
            m_lastPoolableObject.transform.SetParent(transform, false);
            m_lastPoolableObject.SetActive(true);

            m_poolableObjects.Add(new PoolableObject(m_key, m_lastPoolableObject));
        }
    }

    public void OnReturnObjectToPool()
    {
        if (m_poolableObjects.Count > 0)
        {
            PoolableObject poolableObject = m_poolableObjects[m_poolableObjects.Count - 1];
            PoolPartyManager.Instance.ReturnToPool(poolableObject.m_key, poolableObject.m_poolableObject);
            m_poolableObjects.Remove(poolableObject);
        }
        else
        {
            m_resultText.text = "Nothing to return";
        }
    }
}
