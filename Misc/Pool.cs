using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private static Pool _instance;
    private static Queue<GameObject> _queue;
    
    private Pool() { _queue = new Queue<GameObject>(); }
    public static Pool GetInstance()
    {
        if (_instance == null)
            _instance = new Pool();
    
        return _instance;
    }
    
    public GameObject GetFromPool() 
    {
        GameObject poolObject = _queue.Dequeue();
        poolObject.SetActive(true);
        poolObject.transform.localScale = new Vector3(1, 1, 1);
        return poolObject;
    } 
    
    public void AddToPool(GameObject poolObject)
    {
        if(poolObject.GetComponent<Wall>() != null)
        {
            MeshRenderer msh = poolObject.GetComponent<MeshRenderer>();
            Color color = msh.material.GetColor("_Color");
            msh.material.color = new Color(color.r, color.g, color.b, 1);
        }

        poolObject.transform.localScale = new Vector3(1, 1, 1);
        poolObject.SetActive(false);
        _queue.Enqueue(poolObject);
    }
    
    public void ClearPool()
    {
        _queue.Clear();
    }
}

