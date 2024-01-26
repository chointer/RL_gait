using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCollision : MonoBehaviour
{
    private RobotAgent robotAgent;
    private void Start()
    {
        robotAgent = GameObject.FindObjectOfType<RobotAgent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        string collidedTag = collision.gameObject.tag;
        if (collidedTag == "Body" || collidedTag == "Foot")
        {
            robotAgent.GoalAchieved();
        }
    }
}
