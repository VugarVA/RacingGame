using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CameraSwitch : MonoBehaviour 
{ 
    public GameObject[] cameras; // creating an array to store all the cameras available 
    private int currentCam; // creating a variable to keep track of which camera is currently on 
 
    void Update()    // Update is called once per frame 
    { 
        if(Input.GetKeyDown(KeyCode.C)) // to check if the user presses the button 'C' 
        { 
            currentCam++; // increases hte current cam number to change the camera 
            if(currentCam >= cameras.Length) // checking if will go over the camera amount in the camera array 
            { 
                currentCam = 0; // if it goes over, it resets to zero (i.e., the first camera it started with) 
            } 
            for (int i = 0; i<cameras.Length;i++) // for loop to go through the camera array 
            { 
                if(i == currentCam) // checking if the current camera which we needed earlier is the same as i 
                { 
                    cameras[i].SetActive(true); // if it is, then it will be turned on 
                } else{ 
                    cameras[i].SetActive(false);// if it is not, then it will be turned off 
                } 
            } 
        } 
    } 
} 
