using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void Awake()
    {
        transform.position = Vector3.zero + offset;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(target.position);
        transform.position = target.position + offset;    
    }
}
