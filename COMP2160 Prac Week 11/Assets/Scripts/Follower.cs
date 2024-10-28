using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    
    [SerializeField] private Transform target1;

    [SerializeField] private Transform target2;

    public float mid = 0.5f;

    void Update()
    {
        Vector3 targetPosition = Vector3.Lerp(target1.position, target2.position, mid);
        transform.position = targetPosition;
    }
}
