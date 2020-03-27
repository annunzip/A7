using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectedWalking : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject walkingSpace;
    Vector3 prevForwardVector;
    Vector3 prevLocation;
    float prevYawRelativeToCenter;
    float leftOrRight (Vector3 pointToTest,Vector3 vectorSource, Vector3 vectorDestination) {
        float result = (pointToTest.x-vectorSource.x) * (vectorDestination.z-vectorSource.z)-(pointToTest.z-vectorSource.z)*(vectorDestination.x-vectorSource.x);
        return result;
    }

    float angleBetweenVectors(Vector3 A, Vector3 B) {
        A.y = 0;
        B.y = 0;
        Vector3 A1;
        Vector3 B1;
        A.Normalize();
        B.Normalize();
        float dot = (A.x * B.x) + (A.z * B.z);
        float result = Mathf.Acos(dot);
        result = result * (180/Mathf.PI);
        return result;
    }

    void Start()
    {
        prevForwardVector = gameObject.transform.forward;
        prevYawRelativeToCenter = angleBetweenVectors(prevForwardVector, walkingSpace.transform.position-gameObject.transform.position);
        prevLocation = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //print(howMuchToAccelerate);
        float howMuchUserRotated = angleBetweenVectors(prevForwardVector,gameObject.transform.forward);
        int directionUserRotated = (leftOrRight(gameObject.transform.forward+prevForwardVector, gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward)<0)?1:-1;
        float deltaYawRelativeToCenter = prevYawRelativeToCenter - angleBetweenVectors(gameObject.transform.forward, walkingSpace.transform.position-gameObject.transform.position);
        float distanceFromCenter = gameObject.transform.localPosition.magnitude;
        float longestDimensionOfPE = .05F;
        float howMuchToAccelerate = (float)((deltaYawRelativeToCenter<0)? -.13F: .3F) * howMuchUserRotated * directionUserRotated * Mathf.Clamp(distanceFromCenter/longestDimensionOfPE/2,0,1);
        if(Mathf.Abs(howMuchToAccelerate) > 0) {
            walkingSpace.transform.RotateAround(gameObject.transform.position, new Vector3 (0,1,0), howMuchToAccelerate);
        }
        Vector3 trajectoryVector = gameObject.transform.position-prevLocation;
        prevLocation = gameObject.transform.position;
        trajectoryVector.Normalize();
        Vector3 howMuchToTranslate = trajectoryVector * .001F;
        walkingSpace.transform.position += howMuchToTranslate;
        prevForwardVector = gameObject.transform.forward;
        prevYawRelativeToCenter = angleBetweenVectors(gameObject.transform.forward, walkingSpace.transform.position-gameObject.transform.position);
    }
}
