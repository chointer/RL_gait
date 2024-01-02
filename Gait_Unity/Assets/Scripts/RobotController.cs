using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    [System.Serializable]
    public struct Joint 
    {
        public string inputAxis;
        public GameObject robotPart;
    }

    public Joint[] joints;
    public float strength = 5;
    private ArticulationJointController[] jointControllerList;


    private void Start()
    {
        jointControllerList = new ArticulationJointController[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            jointControllerList[i] = joints[i].robotPart.GetComponent<ArticulationJointController>();
        }
    }


    public void RotateAll(float[] actionList)
    {
        for (int i = 0; i < joints.Length; i++)
        {
            jointControllerList[i].RotateAmount(actionList[i] * strength);
        }
    }

    public void GetJointPosition(int index)
    {
        ArticulationBody articulationBody = joints[index].robotPart.GetComponent<ArticulationBody>();
        Debug.Log(articulationBody.jointPosition[0] * 180 / 3.1415);
    }
    /*void FixedUpdate()
    {
        if (Time.time > 1f)
        {
            RotateAll(GenerateRandomActionList());
        }
    }


    public float[] GenerateRandomActionList()
    {
        var randomActionList = new float[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            randomActionList[i] = (Random.value * 2 - 1);
        }

        Debug.Log("GenerateRandomActionList; " + randomActionList[0] + " " + randomActionList[1] + " " + randomActionList[2] + " ");        
        return randomActionList;
    }*/
}
