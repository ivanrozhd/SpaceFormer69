using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detail : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rocket rocket = other.GetComponent<Rocket>();
        
        if(rocket == null)
            return;
        
        rocket.addPart();

        Transform parent = gameObject.transform.parent;
        if (parent != null)
        {
            parent.gameObject.GetComponent<CharacterController>().DropObject();
        }
        Destroy(gameObject);
    }
}
