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
    private GroundCollision groundCollision;
    private RobotController robotController;
    private ArticulationBody rootArticulationBody;

    private Transform goalTrans;
    private Transform agentTrans;
    private float preDist;
    private float maxDist;

    private float penaltyGroundedConstA = -0.45f;
    private float penaltyGroundedConstB = -0.05f;

    // Link RobotController component
    public override void Initialize()
    {
        robotController = gameObject.GetComponent<RobotController>();
        rootArticulationBody = gameObject.GetComponent<ArticulationBody>();
        groundCollision = ground.GetComponent<GroundCollision>();

        GameObject goal = GameObject.FindGameObjectWithTag("Finish");
        goalTrans = goal.transform;
        agentTrans = gameObject.transform;
        maxDist = Vector3.Magnitude(goalTrans.position - agentTrans.position) * 1.2f;
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
        bool[] statusBody = groundCollision.GetBodyStatus();
        foreach (bool b in statusBody) 
            sensor.AddObservation(b);
        bool[] statusFoot = groundCollision.GetFootStatus();
        foreach (bool b in statusFoot) 
            sensor.AddObservation(b);
        // 4. gravity in local for the root Object? or for all? (3?)
        sensor.AddObservation(transform.InverseTransformDirection(Physics.gravity));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float distance = Vector3.Magnitude(goalTrans.position - agentTrans.position);

        // Reward; Distance to Goal
        if (distance > maxDist)
        {
            AddReward(-1f);
            EndEpisode();
        }
        else
        {
            AddReward((preDist - distance)/maxDist);    // Divide to normalize
            preDist = distance;
        }

        // Reward; Grounded
        bool[] statusBody = groundCollision.GetBodyStatus();
        int countGrounded = 0;
        foreach (bool b in statusBody)
            if (b is true) countGrounded++;
        if (countGrounded > 0) 
            AddReward(penaltyGroundedConstA * countGrounded + penaltyGroundedConstB);

        // Perform Action
        var actionBuffers = actions.ContinuousActions;
        var actionList = new float[actionBuffers.Length];
        for (int i = 0; i < actionBuffers.Length; i++)
        {
            actionList[i] = Mathf.Clamp(actionBuffers[i], -1, 1f);
        }
        robotController.RotateAll(actionList);
    }
    public void GoalAchieved()
    {
        Debug.Log("Goal Achieved !");
        AddReward(1.0f);
        EndEpisode();
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
