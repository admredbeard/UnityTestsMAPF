using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinerendererPooling : MonoBehaviour
{
    public static LinerendererPooling SharedInstance;

    [SerializeField]
    private GameObject LinerObj;
    [SerializeField]
    private int poolAmount;

    private List<GameObject> pooledObjects;

    private int currentPoolSize;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int j = 0; j < poolAmount; j++)
        {
            GameObject obj = Instantiate(LinerObj, transform.position, Quaternion.identity, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        currentPoolSize = poolAmount;
    }

    public GameObject GetPooledVFX()
    {
        int count = 0;
        foreach (GameObject lineR in pooledObjects)
        {
            count++;
            if (!lineR.activeInHierarchy)
            {
                if (count > currentPoolSize * 0.8f)
                {
                    GameObject obj = Instantiate(LinerObj, transform.position, Quaternion.identity, transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    currentPoolSize++;
                }
                return lineR;
            }
        }
        return null;

    }
}

public class HitFXPooling : MonoBehaviour
{
    

   
}