using UnityEngine;

public class BodyExplosion : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField]
    GameObject fullBody;
    [SerializeField]
    GameObject[] bodyParts;
    Rigidbody[] rigidbodies;
    Vector3[] iniPositions;
    Transform[] transformParents;

    [Header("Explosion")]
    [SerializeField]
    Transform explosionCenter;
    [SerializeField]
    float explosionForce;
    [SerializeField]
    float upForce;
    [SerializeField]
    float explosionRadius;
    [SerializeField]
    bool isDead;

    [SerializeField]
    float dieCounter;
    float currentDieCounter; 

    void Start()
    {
        rigidbodies = new Rigidbody[bodyParts.Length];
        iniPositions = new Vector3[bodyParts.Length];
        transformParents = new Transform[bodyParts.Length];
 
        for (int i = 0; i < bodyParts.Length; i++)
        {
            rigidbodies[i] = bodyParts[i].GetComponent<Rigidbody>();


            iniPositions[i] = bodyParts[i].transform.localPosition;

            transformParents[i] = bodyParts[i].transform.parent; 
        }

        currentDieCounter = dieCounter; 
    }

    public void PooledStart()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].transform.parent = transformParents[i];
            bodyParts[i].transform.localPosition = iniPositions[i];
            bodyParts[i].transform.localScale = Vector3.one; 
        }
    }

    void Update()
    {
        if (isDead)
        {
            Die();
            currentDieCounter -= Time.deltaTime;

            for (int i = 0; i < bodyParts.Length; i++)
            {
                bodyParts[i].transform.localScale = new Vector3(Mathf.Lerp(bodyParts[i].transform.localScale.x, 0, 0.5f*Time.deltaTime),
                    Mathf.Lerp(bodyParts[i].transform.localScale.y, 0, 0.5f * Time.deltaTime), 
                    Mathf.Lerp(bodyParts[i].transform.localScale.z, 0, 0.5f * Time.deltaTime));
            }

            if (currentDieCounter <= 0)
            {
                for (int i = 0; i < bodyParts.Length; i++)
                {
                    bodyParts[i].SetActive(false); 
                }
                isDead = false;
                currentDieCounter = dieCounter;
            }
        }
    }

    public void Die()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            bodyParts[i].transform.SetParent(null);
            fullBody.SetActive(false);
            bodyParts[i].SetActive(true);
            rigidbodies[i].AddExplosionForce(RandomExplosionForce(), explosionCenter.position, explosionRadius, upForce, ForceMode.Impulse);
        }
    }

    float RandomExplosionForce()
    {
        return Random.Range(explosionForce, explosionForce + 1);
    }
}
