using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.SceneManagement; 
using TMPro; 
 
public class MainMenus : MonoBehaviour 
{ 
    public TMP_InputField lapsInput; // assiging an input field for the user to input 
    public int laps; // creating variable to store the number of laps the user wants 
    public string scene; // storing the name of the scene which should be played after 
 
    public void QuitGame() // function to quit the game 
    { 
        Application.Quit(); // closes the application 
    } 
    public void ChangeLaps() // changes the lap after the user inputs the disired lap amount 
    { 
        laps = int.Parse(lapsInput.text); // changes the string input of the user and stores it in laps variable 
        RaceInfo.instance.numberLaps = laps; // assigns it to the instance of RaceInfo which carries to the race scene 
    } 
 
    public void Easy() // setting the ai difficuly to easy 
    { 
        RaceInfo.instance.carMaxSpeed = 30f; // changing the car's max speed to be 30 in the RaceInfo instance to be carried to the race scene 
        SceneManager.LoadScene(scene); // loading the racing scene 
    } 
 
    public void Medium() // setting the ai difficuly to medium 
    { 
        RaceInfo.instance.carMaxSpeed = 20f; // changing the car's max speed to be 20 in the RaceInfo instance to be carried to the race scene 
        SceneManager.LoadScene(scene); // loading the racing scene 
    } 
 
    public void Hard() // setting the ai difficuly to hard 
    { 
        RaceInfo.instance.carMaxSpeed = 10f; // changing the car's max speed to be 20 in the RaceInfo instance to be carried to the race scene 
        SceneManager.LoadScene(scene); // loading the racing scene 
    } 
} 
