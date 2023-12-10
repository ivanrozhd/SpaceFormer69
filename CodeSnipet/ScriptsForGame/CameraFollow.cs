using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float distanceAway = 5;

    [SerializeField]
    private float distanceUp = 2;

    [SerializeField]
    private float smooth = 100;

    [SerializeField]
    private Transform followObject;
    private Vector3 toPosition;
    // Update is called once per frame
    void LateUpdate()
    {
        toPosition = followObject.position + Vector3.up * distanceUp - followObject.forward * distanceAway;

        transform.position = Vector3.Lerp(transform.position, toPosition, Time.deltaTime * smooth);

        transform.LookAt(followObject);
    }
  
}
