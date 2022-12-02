using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkeletonAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField] private float detectionDelay = 0.05f;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    private void Start()
    {
        //Detecting Player and Obstacles around
        InvokeRepeating(nameof(PerformDetection), 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }
    public Vector2 GetMovingDirection()
    {
        return movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
    }
}
