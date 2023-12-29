using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationJointController : MonoBehaviour
{
    public float rotationTorqueMax = 50f;
    private ArticulationBody articulation;

    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    public void RotateAmount(float rotationAmount)
    {
        float torque = rotationAmount * rotationTorqueMax;
        articulation.AddRelativeTorque(new Vector3(torque, 0, 0));
    }
}
