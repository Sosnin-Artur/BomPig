using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{        
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    
    public static ObjectPooler SharedInstance;
    
    private List<GameObject> _pooledObjects;

    void Awake()
    {
        SharedInstance = this;
    }
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        _pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            _pooledObjects.Add(obj);                
        }
    }

    public GameObject GetPooledObject(Vector2 position)
    {        
        for (int i = 0; i < _pooledObjects.Count; i++)
        {            
            if (!_pooledObjects[i].activeInHierarchy)
            {
                _pooledObjects[i].SetActive(true);
                _pooledObjects[i].transform.position = position;
                return _pooledObjects[i];
            }
        }        
        return null;
    }

}
