using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHunter : MonoBehaviour
{
    // Start is called before the first frame update

    //public Inventory hunterInv;
   // public Collectable[] hunterinv;
    public TextMesh score;

    public TextMesh timer;
    //public TextMesh count;
    //public TextMesh winScreen;
    public enum AttachmentRule { KeepRelative, KeepWorld, SnapToTarget }

    // Collectable cube, cylinder, sphere;

    int total = 0;
    int values;
    public LayerMask c0llectibles_layer;
    public GameObject spanwer;
    public TextMesh instructions;
    GameObject hidden;
    //private MeshRenderer mesh;
    int numObjects = 12;
    public GameObject Rees;
    float currentTime;
    Vector3 prevPointerPos;
    //public Camera fpcamera;
    int treasure1Count, treasure2Count, treasure3Count, treasure4Count = 0;
    void Start()
    {
        hidden = GameObject.Find("hidden");
        //mesh = spanwer.GetComponent<MeshRenderer>();
        Time.timeScale = 1.5f;
        //total = 0;
        //values = 0;
        score.text = (total + " out of " + numObjects + " collected.");
        //Rees = GameObject.Find("reeeee");
        
        currentTime = 0;
        //hunterInv = new Inventory();
        // hunterinv = new Collectable[3];
        //cube = ((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Treasure1.prefab", typeof(GameObject))).GetComponent<Collectable>();
        //cylinder = ((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Treasure2.prefab", typeof(GameObject))).GetComponent<Collectable>();
        //sphere = ((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Treasure3.prefab", typeof(GameObject))).GetComponent<Collectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > 0 && Time.time > (5f + currentTime)) {
            this.transform.GetChild(0).GetComponent<Light>().enabled = true;
        }
        timer.text = Time.time.ToString();
        if(Input.GetKeyDown("e"))
            {
                print("e pressed");
                RaycastHit outHit;
                //Debug.DrawRay(transform.position, transform.forward * 100, Color.red, 10000);
                print(Physics.Raycast(transform.position, transform.forward, out outHit, 10000.0f));
                if(Physics.Raycast(transform.position, transform.forward, out outHit, 10f))
                {
                    //print(outHit.transform.gameObject.GetComponent("Audio Source"));
                    if (outHit.transform.gameObject.GetComponent("Collectable"))
                    {
                        print("valid object");
                        var prefabPath = outHit.transform.gameObject.GetComponent<Collectable>().pathName;
                        //GameObject hitObject = ((GameObject)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + prefabPath + typeof(GameObject)));
                        GameObject hitObject = ((GameObject)Resources.Load(prefabPath));

                        if(outHit.transform.gameObject.GetComponent<Collectable>().trap) {
                            print("is trap");
                            outHit.transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
                            outHit.transform.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-750, 500, 0));
                            outHit.transform.gameObject.GetComponent<Collectable>().trap = false;
                            this.GetComponent<AudioSource>().enabled = true;
                        } else if(outHit.transform.gameObject.GetComponent<Collectable>().tutorial) {
                            instructions.text = "";
                            Destroy(outHit.transform.gameObject);
                        } else {
                            print("isnt trap");
                            if(outHit.transform.gameObject.GetComponent<Collectable>().actualtrap) {
                                GameObject Rees = GameObject.Find("reeeee");
                                Rees.transform.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,-1700));
                                Rees.transform.gameObject.GetComponent<AudioSource>().enabled = true;
                                currentTime = Time.time;
                                this.transform.GetChild(0).GetComponent<Light>().enabled = false;
                            }
                            values += hitObject.transform.gameObject.GetComponent<Collectable>().worth;
                            total += 1;
                            if(total == 11) {
                                numObjects ++;
                                //mesh.enabled = !mesh.enabled;
                                hidden.GetComponent<MeshRenderer>().enabled = true;
                                hidden.GetComponent<SphereCollider>().enabled = true;
                                hidden.GetComponent<AudioSource>().enabled = true;
                            }
                            score.text = (total + " out of " + numObjects + " collected.");
                            Destroy(outHit.transform.gameObject);
                        }
                        

                    }
            }
            }
            }

 }
