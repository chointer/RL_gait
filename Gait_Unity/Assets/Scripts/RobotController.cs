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
}