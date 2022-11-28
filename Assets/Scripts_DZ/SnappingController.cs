using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SnappingController : MonoBehaviour
{
    // Role of Block
    public enum Role
    {
        BuiltBlock,
        SnappingBlock
    }
    public Role currentRole;



    // Snapping Preview Object
    private GameObject previewObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //////// BUILT BLOCK RELATED //////////
    private void OnTriggerEnter(Collider other)
    {
        if (currentRole == Role.BuiltBlock)
        {
            Transform otherTF = other.gameObject.transform;

            if (otherTF.tag == "element" && !previewObj)
            {
                //duplicate to show preview, otherwise hand-tracking overwrites object position
                previewObj = Instantiate(otherTF.gameObject);
                previewObj.tag = "preview";
                Destroy(previewObj.transform.GetChild(0).gameObject);

            }
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (currentRole == Role.BuiltBlock)
        {
            if (previewObj)
            {
                Destroy(previewObj.gameObject);

            }

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (currentRole == Role.BuiltBlock)
        {
            Transform otherTF = other.gameObject.transform;
            if (otherTF.tag == "element")
            {
                //correct the other's orientation
                //rounding xyz rotation to closest 0, 90 or 180 degrees
                Vector3 rotationAngles = otherTF.eulerAngles;
                rotationAngles.x = Mathf.Round(rotationAngles.x / 90) * 90;
                rotationAngles.y = Mathf.Round(rotationAngles.y / 90) * 90;
                rotationAngles.z = Mathf.Round(rotationAngles.z / 90) * 90;
                //print(rotationAngles);
                otherTF.eulerAngles = rotationAngles;

                Vector3 TFPos = otherTF.position;
                Vector3 basePos = transform.parent.position;

                if (previewObj)
                {
                    previewObj.transform.eulerAngles = rotationAngles;

                    //add position snapping
                    //six snapping points
                    Vector3[] snapPtArray = new Vector3[6];
                    snapPtArray[0] = new Vector3(basePos.x - 0.2f, basePos.y, basePos.z);
                    snapPtArray[1] = new Vector3(basePos.x + 0.2f, basePos.y, basePos.z);
                    snapPtArray[2] = new Vector3(basePos.x, basePos.y + 0.2f, basePos.z);
                    snapPtArray[3] = new Vector3(basePos.x, basePos.y - 0.2f, basePos.z);
                    snapPtArray[4] = new Vector3(basePos.x, basePos.y, basePos.z - 0.2f);
                    snapPtArray[5] = new Vector3(basePos.x, basePos.y, basePos.z + 0.2f);


                    //calculate distances to snapping points
                    float minDis = 100f;
                    int correctSnapPtIndex = 0;
                    for (int i = 0; i < snapPtArray.Length; i++)
                    {
                        float distance = Vector3.Distance(TFPos, snapPtArray[i]);
                        if (distance < minDis)
                        {
                            minDis = distance;
                            correctSnapPtIndex = i;
                        }
                    }
                    
                    // NOTE: preview gameoject and actual object both exist
                    //previewObj.transform.position = snapPtArray[correctSnapPtIndex];
                    previewObj.transform.DOMove(snapPtArray[correctSnapPtIndex], 0.1f).SetEase(Ease.OutSine);
                    otherTF.position = snapPtArray[correctSnapPtIndex];
                    
                    // If hand stops grabbing, destroy preview to confirm snapping
                    if (otherTF.GetComponent<BlockManager>().IsGrabbing == false)
                    {
                        Destroy(previewObj.gameObject);
                    }
                    
                }
            }
        }
    }





    
}
