using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * int numberBodyContacts: 1 �̻��̸� �г�Ƽ�� �ο��ϱ� ���� ��
 * bool[] �� ��ǰ�� status ���
 * Start: Dictionary�� Array�� ��� ��ǰ ������ �߰��Ѵ�.
 *          Dict; string Name - (string Tag, int index)
 *          Bool[]; Status
 *          Status[index]�� Name�� ������ ����ִ�.
 *          Name�� gameObject.name, Tag�� gameObject.tag�� "Body"�� "Foot"�� �ִ�.
 *          Status�� ����� ������ true, �׷��� ������ false
*/


public class CollisionManager : MonoBehaviour
{
    public GameObject robot;

    [HideInInspector]
    public int NofBodyContacts = 0;
    private int NofBody = 0;
    private int NofFoot = 0;

    private bool[] statusBody;
    private bool[] statusFoot;

    private Dictionary<string, (string, int)> dictTagIdx = new Dictionary<string, (string, int)>();
    

    private void Start()
    {
        FillDictionaries(robot.transform);
        statusBody = new bool[NofBody];
        statusFoot = new bool[NofFoot];
    }

    private void FillDictionaries(Transform parent)
    {
        if (parent.gameObject.CompareTag("Body"))
            dictTagIdx.Add(parent.gameObject.name, (parent.gameObject.tag, NofBody++));

        else if (parent.gameObject.CompareTag("Foot"))
            dictTagIdx.Add(parent.gameObject.name, (parent.gameObject.tag, NofFoot++));


        for (int i = 0; i < parent.childCount; i++)
        {
            Transform transform = parent.GetChild(i);
            FillDictionaries(transform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string objectName = collision.gameObject.name;
        if (dictTagIdx.ContainsKey(objectName))
        {
            if (dictTagIdx[objectName].Item1 == "Body")
            {
                statusBody[dictTagIdx[objectName].Item2] = true;
                NofBodyContacts++;
            }
            
            else if (dictTagIdx[objectName].Item1 == "Foot")
                statusFoot[dictTagIdx[objectName].Item2] = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        string objectName = collision.gameObject.name;
        if (dictTagIdx.ContainsKey(objectName))
        {
            if (dictTagIdx[objectName].Item1 == "Body")
            {
                statusBody[dictTagIdx[objectName].Item2] = false;
                NofBodyContacts--;
            }

            else if (dictTagIdx[objectName].Item1 == "Foot")
                statusFoot[dictTagIdx[objectName].Item2] = false;
        }
    }
    
    public bool[] GetBodyStatus()
    {
        return statusBody;
    }

    public bool[] GetFootStatus()
    {
        return statusFoot;
    }
}
