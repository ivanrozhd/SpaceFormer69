using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // parts from lowest to highest
    [SerializeField] List<GameObject> parts;
    [SerializeField] List<GameObject> parts_transparent;
    [SerializeField] private int partsNeeded;
    [SerializeField] private int _partsCollected = 0;
    private float progress = 0f;
    private int stages;
    private int currentStage = 0;

    private void Start()
    {
        stages = parts.Count;
        for (int i = 0; i < stages; i++)
        {
            parts[i].SetActive(false);
            parts_transparent[i].SetActive(false);
        }

        parts_transparent[0].SetActive(true);
    }

    public void addPart()
    {
        if (_partsCollected >= partsNeeded)
            return;
        
        _partsCollected++;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        progress = _partsCollected / (float)partsNeeded;
        if (progress >= (currentStage + 1) / (float) stages)
        {
            NextStage();
        }
    }

    private void NextStage()
    {
        parts[currentStage].SetActive(true);
        parts_transparent[currentStage].SetActive(false);
        currentStage++;
        if (currentStage < stages)
        {
            parts_transparent[currentStage].SetActive(true);
        }
        if (currentStage == stages)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            // finish game
        }
    }
}
