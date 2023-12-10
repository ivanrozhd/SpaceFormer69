using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private bool taken = false;


    public bool getTaken()
    {
        return taken;
    }

    public void setTaken(bool value)
    {
        taken = value;
    }
}
