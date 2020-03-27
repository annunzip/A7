using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHunter : MonoBehaviour
{
    // Start is called before the first frame update

    public Inventory hunterInv;
   // public Collectable[] hunterinv;
    public TextMesh score;
    public TextMesh count;
    public TextMesh winScreen;
    public enum AttachmentRule { KeepRelative, KeepWorld, SnapToTarget }

    // Collectable cube, cylinder, sphere;

    int total;
    int values;
    public LayerMask c0llectibles_layer;
    public GameObject previouslyGrabbed;
    public GameObject rightPointerObject;
    Vector3 prevPointerPos;
    public Camera fpcamera;
    int treasure1Count, treasure2Count, treasure3Count, treasure4Count = 0;
    void Start()
    {
        total = 0;
        values = 0;
        //hunterInv = new Inventory();
       // hunterinv = new Collectable[3];
       //cube = ((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Treasure1.prefab", typeof(GameObject))).GetComponent<Collectable>();
       //cylinder = ((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Treasure2.prefab", typeof(GameObject))).GetComponent<Collectable>();
       //sphere = ((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Treasure3.prefab", typeof(GameObject))).GetComponent<Collectable>();
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
                        score.text = ("The total number of treasures you've gotten is: " + total + "\n The total value of your collected objects is: " + values + "\n Author: Patrick Annunziata\n Collaborator: Justin Tse");
                        Destroy(outHit.transform.gameObject);

                    }

                



                // print("entered physics raycast");
                //  if(outHit.collider.name == "Treasure1")
                // {
                //    c
                //    pickUp(outHit, "cube");
                // }
                // else if(outHit.collider.name == "Treasure2")
                // {
                //   print("entered Treasure2");
                //   pickUp(outHit, "cylinder");
                //}
                //else if(outHit.collider.name == "Treasure3")
                // {
                //  print("entered Treasure3");
                //    pickUp(outHit, "sphere");
                // }
            }
            }
       // if(this.GetComponent<Light>().enabled)
       // {
        //    this.GetComponent<Light>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
       // }

        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            print("A Hit");
            Collider[] overlappingThings = Physics.OverlapSphere(rightPointerObject.transform.position, 1, c0llectibles_layer);
            if (overlappingThings.Length > 0)
            {
                print("Entered Grab");
                Collectable nearestCollectible = getClosestHitObject(overlappingThings);
                attachGameObjectToAChildGameObject(nearestCollectible.gameObject, rightPointerObject, AttachmentRule.SnapToTarget, AttachmentRule.SnapToTarget, AttachmentRule.KeepWorld, true);
                //I'm not bothering to check for nullity because layer mask should ensure I only collect collectibles.
                previouslyGrabbed = nearestCollectible.gameObject;
                var trappedCollectible = previouslyGrabbed;
                if(trappedCollectible.gameObject.GetComponent<Collectable>().trap)
                {
                    print("Found Trap");
                    //trappedCollectible.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    trappedCollectible.GetComponent<AudioSource>().enabled = true;
                    //this.GetComponent<AudioSource>().enabled = true;
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10.0f, 10.0f)*1000, Random.Range(-10.0f, 10.0f)*100, Random.Range(-10.0f, 10.0f)*1000));
                   // this.GetComponent<Light>().enabled = true;
                    this.GetComponent<ParticleSystem>().Play();
                    ParticleSystem.EmissionModule em = this.GetComponent<ParticleSystem>().emission;
                    em.enabled = true;
                    this.GetComponent<Rigidbody>().transform.Rotate(10000, 0, 10000);

                }
            }
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.B))
        {
            print("B");
            letGo();
        }
            prevPointerPos = rightPointerObject.gameObject.transform.position;
    }

    Collectable getClosestHitObject(Collider[] hits)
    {
        float closestDistance = 10000.0f;
        Collectable closestObjectSoFar = null;
        foreach (Collider hit in hits)
        {
            Collectable c = hit.gameObject.GetComponent<Collectable>();
            if (c)
            {
                float distanceBetweenHandAndObject = (c.gameObject.transform.position - rightPointerObject.gameObject.transform.position).magnitude;
                if (distanceBetweenHandAndObject < closestDistance)
                {
                    closestDistance = distanceBetweenHandAndObject;
                    closestObjectSoFar = c;
                }
            }
        }
        return closestObjectSoFar;
    }

    public void attachGameObjectToAChildGameObject(GameObject GOToAttach, GameObject newParent, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule, bool weld)
    {
        GOToAttach.transform.parent = newParent.transform;
        handleAttachmentRules(GOToAttach, locationRule, rotationRule, scaleRule);
        if (weld)
        {
            simulatePhysics(GOToAttach, Vector3.zero, false);
        }
    }

    void letGo()
    {
        Vector3 subtract = new Vector3(0, .5f, 0);
        var satchelLocation = fpcamera.transform.position - subtract;
        if (previouslyGrabbed)
            
        {
            print("im here");
            Collider[] overlappingThingsWithRightHand = Physics.OverlapSphere(rightPointerObject.transform.position, 0.01f, c0llectibles_layer);
            if (overlappingThingsWithRightHand.Length > 0)
                if(Vector3.Distance(previouslyGrabbed.transform.position, satchelLocation) < .5f) {
                    print("collected");
                    var prefabPath = previouslyGrabbed.GetComponent<Collectable>().pathName;
                    //GameObject hitObject = ((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + prefabPath + typeof(GameObject)));
                    GameObject hitObject = ((GameObject)Resources.Load(prefabPath));
                    hunterInv.inv.Add(hitObject);
                    values += hitObject.transform.gameObject.GetComponent<Collectable>().worth;
                    total++;
                    if (previouslyGrabbed.GetComponent<Collectable>().pathName == "Treasure1")
                    {
                        treasure1Count++;
                    } else if(previouslyGrabbed.GetComponent<Collectable>().pathName == "Treasure2")
                    {
                        treasure2Count++;
                    } else if(previouslyGrabbed.GetComponent<Collectable>().pathName == "Treasure3")
                    {
                        treasure3Count++;
                    } else if(previouslyGrabbed.GetComponent<Collectable>().pathName == "Treasure4")
                    {
                        treasure4Count++;
                    }
                    score.text = ("The total number of treasures you've gotten is: " + total + "\n The total value of your collected objects is: " + values + "\nTreasure 1 is worth 10 points\nTreasure 2 is worth 5 points\nTreasure 3 is worth 2 points\nTreasure 4 is worth 20 points" + "\n Author: Patrick Annunziata\n Collaborator: Justin Tse");
                    count.text = (treasure1Count + " of treasure 1\n" + treasure2Count + " of treasure 2\n" + treasure3Count + " of treasure 3\n" + treasure4Count + " of treasure 4.");
                    Destroy(previouslyGrabbed);
                }
            {
                print("over here");
                detachGameObject(previouslyGrabbed.gameObject, AttachmentRule.KeepWorld, AttachmentRule.KeepWorld, AttachmentRule.KeepWorld);
                simulatePhysics(previouslyGrabbed.gameObject, (rightPointerObject.gameObject.transform.position - prevPointerPos) / Time.deltaTime, true);
                previouslyGrabbed = null;
            }
        }
    }

    public static void detachGameObject(GameObject GOToDetach, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule)
    {
        //making the parent null sets its parent to the world origin (meaning relative & global transforms become the same)
        GOToDetach.transform.parent = null;
        handleAttachmentRules(GOToDetach, locationRule, rotationRule, scaleRule);
    }

    public static void handleAttachmentRules(GameObject GOToHandle, AttachmentRule locationRule, AttachmentRule rotationRule, AttachmentRule scaleRule)
    {
        GOToHandle.transform.localPosition =
        (locationRule == AttachmentRule.KeepRelative) ? GOToHandle.transform.position :
        //technically don't need to change anything but I wanted to compress into ternary
        (locationRule == AttachmentRule.KeepWorld) ? GOToHandle.transform.localPosition :
        new Vector3(0, 0, 0);

        //localRotation in Unity is actually a Quaternion, so we need to specifically ask for Euler angles
        GOToHandle.transform.localEulerAngles =
        (rotationRule == AttachmentRule.KeepRelative) ? GOToHandle.transform.eulerAngles :
        //technically don't need to change anything but I wanted to compress into ternary
        (rotationRule == AttachmentRule.KeepWorld) ? GOToHandle.transform.localEulerAngles :
        new Vector3(0, 0, 0);

        GOToHandle.transform.localScale =
        (scaleRule == AttachmentRule.KeepRelative) ? GOToHandle.transform.lossyScale :
        //technically don't need to change anything but I wanted to compress into ternary
        (scaleRule == AttachmentRule.KeepWorld) ? GOToHandle.transform.localScale :
        new Vector3(1, 1, 1);
    }

    public void simulatePhysics(GameObject target, Vector3 oldParentVelocity, bool simulate)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb)
        {
            if (!simulate)
            {
                Destroy(rb);
            }
        }
        else
        {
            if (simulate)
            {
                //there's actually a problem here relative to the UE4 version since Unity doesn't have this simple "simulate physics" option
                //The object will NOT preserve momentum when you throw it like in UE4.
                //need to set its velocity itself.... even if you switch the kinematic/gravity settings around instead of deleting/adding rb
                Rigidbody newRB = target.AddComponent<Rigidbody>();
                newRB.velocity = oldParentVelocity;
            }
        }
    }

}
