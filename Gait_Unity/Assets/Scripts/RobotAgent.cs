using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RobotAgent : Agent
{
    private RobotController robotController;
    
    // Link RobotController component
    public override void Initialize()
    {
        robotController = gameObject.GetComponent<RobotController>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {

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
        var continuousActionsOut = actionsOut.ContinuousActions;
        /*
        if (Time.time > 1f)
        {
            for (int i = 0; i < continuousActionsOut.Length; i++)
            {
                continuousActionsOut[i] = (Random.value * 2 - 1);
            }
        }
        */
        continuousActionsOut[0] = Input.GetAxis("Horizontal") * 3;
        //Debug.Log();
        continuousActionsOut[1] = Input.GetAxis("Vertical") * 3;
        Debug.Log(continuousActionsOut[0] + " " + continuousActionsOut[1]);

        //robotController.GetJointPosition(0);
        //Debug.Log("Heuristic; " + continuousActionsOut[0] + " " + continuousActionsOut[1] + " " + continuousActionsOut[2] + " ");
    }
}
