using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] GameObject bars;
    protected void Open(object sender, EventArgs e)
    {
        bars.SetActive(false);
    }

    protected void Close(object sender, EventArgs e)
    {
        bars.SetActive(true);
    }
}
