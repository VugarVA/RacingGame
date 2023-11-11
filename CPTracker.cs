using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CPTracker : MonoBehaviour 
{ 
    public CarControl car; // assigning the car 
    private void OnTriggerEnter(Collider other) // unities own method which is called when two objects collide with each other 
    { 
        if(other.tag == "Checkpoint") // checking if the car has collided with something with a tag of checkpoint 
        { 
            car.CPHit(other.GetComponent<Checkpoints>().cpNumber); //if it has, it will call the CPHit function from the CarControl with the checkpoint number of the colliding object 
        } 
    } 
} 
