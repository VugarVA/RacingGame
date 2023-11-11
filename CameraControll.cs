using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CameraControll : MonoBehaviour 
{ 
    public CarControl target; //assiging the car as the target 
    private Vector3 offsetDir; //creating a variable for the offset direction 
    public float minDistance, maxDistance; // creationg the minimum and maximum distance for the camera 
    private float activeDistance; // creating an active camera which will be the current camera distance 
    public Transform startTargetffset; // variable for the target offset at the start 
 
    void Start()    // Start is called before the first frame update 
    { 
        offsetDir = transform.position - startTargetffset.position; //setting an offset direction to the camera 
        activeDistance = minDistance; //setting the active distance to the minimum distance 
        offsetDir.Normalize(); 
    } 
 
    void Update()   // Update is called once per frame 
    { 
        activeDistance = minDistance + ((maxDistance - minDistance) * (target.RB.velocity.magnitude / target.maxSpeed)); // the distance between camera and user will be the minimum distance added to the multiplication of the different between minimum and maximum distance and the ratio between the current velocity and maximum velocity 
        transform.position = target.transform.position + (offsetDir * activeDistance); // transforms the camera to the cars position added to the product of active distance and the offset direction. 
    } 
} 
