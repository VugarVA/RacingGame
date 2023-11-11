using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
 
public class RaceInfo : MonoBehaviour 
{ 
    public static RaceInfo instance; // creating an instance so that any class can access it 
    public int numberLaps; // storing the number of laps to be carried from main menu to the race scene 
    public float carMaxSpeed; // storing the AI Difficuly which is the maximum speed of the car so that it can be carried from main menu to the race scene 
 
    private void Awake() // unity method to be called when the game starts for the first time 
    { 
        if(instance == null) // checking if there is an instance of the object 
        { 
            instance = this; // making sure that this class is the instance 
            DontDestroyOnLoad(gameObject); // unity method to not destory the object and keep its contents when moving from one scene to another 
        }else{ // else function 
            Destroy(gameObject); // it will destory the object with all of its contents 
        } 
    } 
 
} 
