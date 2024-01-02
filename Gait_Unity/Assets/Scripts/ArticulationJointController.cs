using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationJointController : MonoBehaviour
{
    public float rotationTorqueMax = 50f;
    public string axisName = string.Empty;
    public string axisType = string.Empty;
    private ArticulationBody articulation;
    
    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    public void RotateAmount(float rotationAmount)
    {
        float torque = rotationAmount * rotationTorqueMax;
        if (axisType == "x") 
        {
            articulation.AddRelativeTorque(new Vector3(torque, 0, 0));
        }

        else if (axisType == "y")
        {
            articulation.AddRelativeTorque(new Vector3(0, torque, 0));
        }

        else if (axisType == "z")
        {
            articulation.AddRelativeTorque(new Vector3(0, 0, torque));
        }

        else
        {
            Debug.Log("Invalid axisType; " + axisType);
        }
    }

    private void FixedUpdate()
    {
        float val = Input.GetAxis(axisName);
        RotateAmount(val);
    }
}
