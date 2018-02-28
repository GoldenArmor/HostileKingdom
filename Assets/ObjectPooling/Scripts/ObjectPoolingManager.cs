using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = new GameObject("ObjectPoolingManager").AddComponent<ObjectPoolingManager>();
            }
            return instance;
        }
    }


    GenericPooling<Characters> characterPool;
    public GenericPooling<Characters> CharacterPool
    {
        get
        {
            return characterPool;
        }
    }

    GenericPooling<Turret> turretPool;
    public GenericPooling<Turret> TurretPool
    {
        get
        {
            return turretPool;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            characterPool = new GenericPooling<Characters>();
            turretPool = new GenericPooling<Turret>(); 
        }
    }
}

public class GenericPooling<T> where T : MonoBehaviour, IPooledObject
{
    List<T> pooledObjects;

    public GenericPooling()
    {
        pooledObjects = new List<T>();
    }

    public void Clear()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].IsActive())
            {
                Object.Destroy(pooledObjects[i]);
            }
        }
        pooledObjects.Clear();
    }

    public T GetObject(GameObject objectPrefab, Transform spawnPoint)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].IsActive())
            {
                InitializeObject(pooledObjects[i], spawnPoint);
                return pooledObjects[i];
            }
        }

        if (objectPrefab.GetComponent<T>() != null)
        {
            T newObject = Object.Instantiate(objectPrefab).GetComponent<T>();
            newObject.name = objectPrefab.name;
            InitializeObject(newObject, spawnPoint);
            pooledObjects.Add(newObject);
            return newObject;
        }
        else
        {
            return null;
        }
    }

    void InitializeObject(T newObject, Transform spawnPoint)
    {
        newObject.transform.position = spawnPoint.position;
        newObject.transform.rotation = spawnPoint.rotation;
        newObject.PooledAwake();
        newObject.PooledStart(); 
    }
}

public interface IPooledObject
{
    bool IsActive();
    void PooledAwake();
    void PooledStart();
}
