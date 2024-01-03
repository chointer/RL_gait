using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public GameObject[] joints;
    [HideInInspector]
    public ArticulationJointController[] jointControllerList;


    private void Start()
    {
        jointControllerList = new ArticulationJointController[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            jointControllerList[i] = joints[i].GetComponent<ArticulationJointController>();
        }
    }


    public void RotateAll(float[] actionList)
    {
        for (int i = 0; i < joints.Length; i++)
        {
            jointControllerList[i].RotateAmount(actionList[i]);
        }
    }

    public void GetJointPosition(int[] index)
    {
        string message = "";
        for (int i = 0; i < index.Length; i++)
        {
            message += (int)(joints[index[i]].GetComponent<ArticulationBody>().jointPosition[0] * 180 / 3.1415);
            message += " ";
        }
        Debug.Log(message);
    }

    public void GetJointAngularVelocity(int[] index)
    {
        string message = "";
        for (int i = 0; i < index.Length; i++)
        {
            var angVel = joints[index[i]].GetComponent<ArticulationBody>().angularVelocity;
            message += (angVel.magnitude.ToString("F2") + "; " + angVel[0].ToString("F2") + ", " + angVel[1].ToString("F2") + ", " + angVel[2].ToString("F2"));
            message += " ";
        }
        Debug.Log(message);
    }

}