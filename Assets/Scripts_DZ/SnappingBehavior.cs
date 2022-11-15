using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This script is attached to the snapping collider game object for each combined block
 */

public class SnappingBehavior : MonoBehaviour
{
    private GameObject previewObj;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Transform otherTF = other.gameObject.transform;
        if (otherTF.tag == "element")
        {
            //duplicate to show preview, otherwise hand-tracking overwrites object position
            previewObj = Instantiate(otherTF.gameObject);

        }
 
    }

    private void OnTriggerStay(Collider other)
    {
        Transform otherTF = other.gameObject.transform;
        if (otherTF.tag == "element")
        {
            print("HHAHHAHAHAHAH");
            //correct the other's orientation
            //rounding xyz rotation to closest 0, 90 or 180 degrees
            Vector3 rotationAngles = otherTF.eulerAngles;
            rotationAngles.x = Mathf.Round(rotationAngles.x / 90) * 90;
            rotationAngles.y = Mathf.Round(rotationAngles.y / 90) * 90;
            rotationAngles.z = Mathf.Round(rotationAngles.z / 90) * 90;
            otherTF.eulerAngles = rotationAngles;

            if (previewObj)
            {
                previewObj.transform.eulerAngles = rotationAngles;
                //add position snapping
            }
            

            //snap position


        }
    }


}
