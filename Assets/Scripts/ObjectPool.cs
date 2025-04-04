using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPool Instance;

    void Awake()
    {
        Instance = this;
    }


    public List<Pool> pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> poolDictinonary;

    void Start()
    {
        poolDictinonary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform.position, Quaternion.identity);
                obj.transform.SetParent(this.transform);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }

            poolDictinonary.Add(pool.tag, objPool);
        }
    }


    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rotation)
    {
        // Debug.Log(tag);
        GameObject objectToSpawn = poolDictinonary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        objectToSpawn.transform.rotation = rotation;

        poolDictinonary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
