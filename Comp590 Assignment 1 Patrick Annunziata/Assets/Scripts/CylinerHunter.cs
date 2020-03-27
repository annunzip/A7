using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinerHunter : MonoBehaviour
{
    public Inventory hunterInv;
   // public Collectable[] hunterinv;
    public TextMesh score;
    public TextMesh count;
    public TextMesh winScreen;
    public enum AttachmentRule { KeepRelative, KeepWorld, SnapToTarget }
    int total;
    int values;
    public LayerMask c0llectibles_layer;
    public GameObject previouslyGrabbed;
    public GameObject rightPointerObject;
    Vector3 prevPointerPos;
    public Camera fpcamera;
    int treasure1Count, treasure2Count, treasure3Count, treasure4Count = 0;
    // Start is called before the first frame update
    void Start()
    {
        total = 0;
        values = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("e"))
            {
                print("e pressed");
                RaycastHit outHit;
                //Debug.DrawRay(transform.position, transform.forward * 100, Color.red, 10000);
                //print(Physics.Raycast(transform.position, transform.forward, out outHit, 10000.0f));
                if(Physics.Raycast(transform.position, transform.forward, out outHit, 100f))
                {
                    if (outHit.transform.gameObject.GetComponent("Collectable"))
                    {
                        var prefabPath = outHit.transform.gameObject.GetComponent<Collectable>().pathName;
                        //GameObject hitObject = ((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + prefabPath + typeof(GameObject)));
                        GameObject hitObject = ((GameObject)Resources.Load(prefabPath));
                        hunterInv.inv.Add(hitObject);
                        values += hitObject.transform.gameObject.GetComponent<Collectable>().worth;
                        total++;
                        //score.text = ("The total number of treasures you've gotten is: " + total + "\n The total value of your collected objects is: " + values + "\n Author: Patrick Annunziata\n Collaborator: Justin Tse");
                        Destroy(outHit.transform.gameObject);

                    }
                }
            }
    }
}
