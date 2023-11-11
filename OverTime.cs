using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class OverTime : MonoBehaviour 
{ 
    public float disableTime; // storing the value for the time duration for the GO to disapear at the start of the race 
     
    void Update() // Update is called once per frame 
    { 
        disableTime -= Time.deltaTime; //every second, the disableTime decreases by one 
        if(disableTime <= 0) // checking if disableTime is below or equal to zero 
        { 
            gameObject.SetActive(false); // if it is, then the GO! sign will turn off so that the user can't see it 
        } 
    } 
} 
