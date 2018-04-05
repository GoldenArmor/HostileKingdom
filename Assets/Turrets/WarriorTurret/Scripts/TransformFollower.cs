using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    [SerializeField]
    Transform myTransform;
    [SerializeField]
    Transform transformToFollow;

	void Update ()
    {
        myTransform.position = transformToFollow.position; 
	}
}
