using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CubeFourScript : MonoBehaviour
{
    private Renderer m_renderer;

    private OVRHand[] m_hands;


    private bool[] m_isIndexStaying;
    TextMesh textObject;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_hands = new OVRHand[]
        {
            GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor/OVRHandPrefab").GetComponent<OVRHand>(),
            GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor/OVRHandPrefab").GetComponent<OVRHand>()
        };
        m_isIndexStaying = new bool[2] { false, false };

        //we don't want the cube to move over collision, so let's just use a trigger
        GetComponent<Collider>().isTrigger = true;
        
    }


    void Update()
    {

    }


    private void OnTriggerEnter(Collider collider)
    {
        //get hand associated with trigger
        int handIdx = GetIndexFingerHandId(collider);

        //if there is an associated hand, it means that an index of one of two hands is entering the cube
        //change the color of the cube accordingly (blue for left hand, green for right one)
        if (handIdx != -1)
        {
            Destroy(m_renderer);

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("FackCubeFour"))
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("CubeFive"))
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.green;
                }
            }
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Text"))
            {
                TextMeshPro renderer = go.GetComponent<TextMeshPro>();
                if (renderer != null)
                {
                    renderer.text = "Step 5: Touch the next green rectangle.";
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {

    }


    private int GetIndexFingerHandId(Collider collider)
    {
        //Checking Oculus code, it is possible to see that physics capsules gameobjects always end with _CapsuleCollider
        if (collider.gameObject.name.Contains("_CapsuleCollider"))
        {
            //get the name of the bone from the name of the gameobject, and convert it to an enum value
            string boneName = collider.gameObject.name.Substring(0, collider.gameObject.name.Length - 16);
            OVRPlugin.BoneId boneId = (OVRPlugin.BoneId)Enum.Parse(typeof(OVRPlugin.BoneId), boneName);

            //if it is the tip of the Index
            if (boneId == OVRPlugin.BoneId.Hand_Index3)
                //check if it is left or right hand, and change color accordingly.
                //Notice that absurdly, we don't have a way to detect the type of the hand
                //so we have to use the hierarchy to detect current hand
                if (collider.transform.IsChildOf(m_hands[0].transform))
                {
                    return 0;
                }
                else if (collider.transform.IsChildOf(m_hands[1].transform))
                {
                    return 1;
                }
        }

        return -1;
    }

}