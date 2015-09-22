using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExampleBehaviours : MonoBehaviour {


    public GameObject m_currenPoolableObject;
    public Text m_resultText;
    public string m_key;

	public void OnAskForPoolObject()
    {
        if (m_currenPoolableObject == null)
        {
            m_currenPoolableObject = PoolPartyManager.Instance.GetPoolableObject(m_key);
            if (m_currenPoolableObject == null)
            {
                m_resultText.text = "Couldn't find a gameObject with that key";
            }else
            {
                m_resultText.text = "Game object found!";
                m_currenPoolableObject.transform.SetParent(transform, false);
                m_currenPoolableObject.SetActive(true);
            }            
        }else
        {
            m_resultText.text = "PoolObject already taken";
        }
        
    }

    public void OnReturnObjectToPool()
    {
        if(m_currenPoolableObject != null)
        {
            PoolPartyManager.Instance.ReturnToPool(m_key, m_currenPoolableObject);
            m_currenPoolableObject = null;
            m_resultText.text = "Object returned!";
        }else
        {
            m_resultText.text = "Nothing to return";
        }
    }
}
