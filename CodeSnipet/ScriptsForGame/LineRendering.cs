using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineRendering : MonoBehaviour
{
    [SerializeField] private GameObject points;
    [SerializeField] private WayFinding _wayFinding;

    private void Start()
    {
        _wayFinding.setUpLine(points);
    }
}