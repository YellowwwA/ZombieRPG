using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    //public Transform target;
    public Vector3 offset;
    public GameObject player;
    public float cameraSpeed = 5.0f;

    void Awake()
    {
        //transform.position = FindObjectOfType<Player>().transform.position + offset;
        player.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //    transform.position = target.position + offset;
        //transform.position = FindObjectOfType<Player>().transform.position + offset;
        transform.position = player.transform.position + offset;
        //Vector3 dir = player.transform.position - this.transform.position;
        //Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, dir.z * cameraSpeed * Time.deltaTime);
        //this.transform.Translate(moveVector);
    
    
    }
}
