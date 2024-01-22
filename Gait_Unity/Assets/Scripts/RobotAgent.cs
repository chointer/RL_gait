using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Threading;
using System;

public class RobotAgent : Agent
{
    public GameObject ground;
    private CollisionManager collisionManager;
    private RobotController robotController;
    private ArticulationBody rootArticulationBody;
    
    // Link RobotController component
    public override void Initialize()
    {
        robotController = gameObject.GetComponent<RobotController>();
        rootArticulationBody = gameObject.GetComponent<ArticulationBody>();
        collisionManager = ground.GetComponent<CollisionManager>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        for (int i = 0; i < robotController.joints.Length; i++)
        {
            ArticulationBody joint = robotController.joints[i].GetComponent<ArticulationBody>();
            // 1. joint angles (12)
            sensor.AddObservation(joint.jointPosition[0] / Mathf.PI);
            // 2. joint angular velocity (12)
            sensor.AddObservation(joint.angularVelocity.magnitude);
        }
        // 3. Collision status
        bool[] statusBody = collisionManager.GetBodyStatus();
        foreach (bool b in statusBody) 
            sensor.AddObservation(b);
        bool[] statusFoot = collisionManager.GetFootStatus();
        foreach (bool b in statusFoot) 
            sensor.AddObservation(b);
        // 4. gravity in local for the root Object? or for all? (3?)
        sensor.AddObservation(transform.InverseTransformDirection(Physics.gravity));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // TODO: Add Rewards

        // Perform Action
        var actionBuffers = actions.ContinuousActions;
        var actionList = new float[actionBuffers.Length];
        for (int i = 0; i < actionBuffers.Length; i++)
        {
            actionList[i] = Mathf.Clamp(actionBuffers[i], -1, 1f);
        }
        robotController.RotateAll(actionList);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Vector3 gravityInLocalSpace = transform.InverseTransformDirection(Physics.gravity);
        //ArticulationReducedSpace driveForce = rootArticulationBody.driveForce;
        //Debug.Log(driveForce.ToString());

        var continuousActionsOut = actionsOut.ContinuousActions;

        for (int i = 0; i < continuousActionsOut.Length; i++)
        {
            continuousActionsOut[i] = Input.GetAxis(robotController.jointControllerList[i].axisName);
        }
    }
}
