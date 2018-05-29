using UnityEngine;

public class BodyExplosion : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField]
    GameObject fullBody;
    [SerializeField]
    GameObject[] bodyParts;
    Rigidbody[] rigidbodies;
    BoxCollider[] bodyPartColliders;
    Vector3[] iniPositions;
    Transform[] transformParents;
    Renderer[] bodyPartRenderers;
    MaterialPropertyBlock materialPropertyBlock;  

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
        bodyPartColliders = new BoxCollider[bodyParts.Length];
        bodyPartRenderers = new Renderer[bodyParts.Length];
        iniPositions = new Vector3[bodyParts.Length];
        transformParents = new Transform[bodyParts.Length];
 
        for (int i = 0; i < bodyParts.Length; i++)
        {
            rigidbodies[i] = bodyParts[i].GetComponent<Rigidbody>();

            bodyPartColliders[i] = bodyParts[i].GetComponent<BoxCollider>();
            bodyPartColliders[i].enabled = false;

            bodyPartRenderers[i] = bodyParts[i].GetComponent<Renderer>();

            iniPositions[i] = bodyParts[i].transform.localPosition;

            transformParents[i] = bodyParts[i].transform.parent; 
        }

        materialPropertyBlock = new MaterialPropertyBlock(); 

        currentDieCounter = dieCounter; 
    }

    void IgnoreCollisions()
    {
        for (int i = 0; i < bodyPartColliders.Length; i++)
        {
            Physics.IgnoreCollision()
        }
    }

    public void PooledStart()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].transform.parent = transformParents[i];
            bodyParts[i].transform.localPosition = iniPositions[i]; 
        }
    }

    void Update()
    {
        if (isDead)
        {
            Die();
            currentDieCounter -= Time.deltaTime;

            for (int i = 0; i < bodyPartRenderers.Length; i++)
            {
                bodyPartRenderers[i].GetPropertyBlock(materialPropertyBlock);

                Color newColor = new Color(255, 255, 255, currentDieCounter/255);
                materialPropertyBlock.SetColor("_Color", newColor);

                bodyPartRenderers[i].SetPropertyBlock(materialPropertyBlock); 
            }

            if (currentDieCounter <= dieCounter - 1)
            {
                for (int i = 0; i < bodyPartColliders.Length; i++)
                {
                    bodyPartColliders[i].enabled = true; 
                }
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
