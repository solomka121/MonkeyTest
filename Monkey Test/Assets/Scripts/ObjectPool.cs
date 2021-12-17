using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectPool : MonoBehaviour
{
    public List<ScoreIndicator> pooledObjects;
    public GameObject objectToPool;
    public Transform parent;
    public int startingAmount;

    void Start()
    {
        pooledObjects = new List<ScoreIndicator>();
        GameObject temp;

        for (int i = 0; i < startingAmount; i++)
        {
            if(parent == null)
                temp = Instantiate(objectToPool);
            else
                temp = Instantiate(objectToPool, parent);

            temp.SetActive(false);
            pooledObjects.Add(temp.GetComponent<ScoreIndicator>());
        }
    }



    public ScoreIndicator GetPooledObject()
    {
        for (int i = 0; i < startingAmount; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

  
}
