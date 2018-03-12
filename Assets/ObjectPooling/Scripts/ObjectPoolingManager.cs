using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    static GenericPooling<Enemy> enemyPool;
    public static GenericPooling<Enemy> EnemyPool
    {
        get
        {
            return enemyPool;
        }
    }

    static GenericPooling<Ally> allyPool;
    public static GenericPooling<Ally> AllyPool
    {
        get
        {
            return allyPool;
        }
    }

    static GenericPooling<ArcherTurret> archerTurretPool;
    public static GenericPooling<ArcherTurret> ArcherTurretPool
    {
        get
        {
            return archerTurretPool;
        }
    }

    static GenericPooling<WarriorTurret> warriorTurretPool;
    public static GenericPooling<WarriorTurret> WarriorTurretPool
    {
        get
        {
            return warriorTurretPool;
        }
    }

    void Awake()
    {
        enemyPool = new GenericPooling<Enemy>();
        allyPool = new GenericPooling<Ally>(); 
        archerTurretPool = new GenericPooling<ArcherTurret>();
        warriorTurretPool = new GenericPooling<WarriorTurret>(); 
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
