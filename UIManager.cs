using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using TMPro; 
public class UIManager : MonoBehaviour 
{ 
    public static UIManager instance; // adding the instance of the UIManager class 
    public TMP_Text lapCounterText, bestLapText, currentLapText, posText, countDownText, goText, raceResultText, bestLapTimeText; // adding the Text Mesh Pro Texts to be edited 
    public GameObject resultsPanel, pauseScreen; // adding the Panel screens to be edited with 
    public bool isPaused = false; // boolean variable to see if the user has paused it or not 
 
    private void Awake() // unity function called when the program starts for the first time 
    { 
        instance = this; // making this class an instance 
    } 
 
    void Update()    // Update is called once per frame 
    { 
        if(Input.GetKeyDown(KeyCode.Escape)) // checking if the user presses the 'ESC' button 
        { 
            Pause(); // if the user does, Pause function is called 
        } 
    } 
 
    public void QuitRace() // quit race function to take the user to the main menu 
    { 
        Time.timeScale = 1f; // makes the in game time 1 (default) 
        RaceManager.instance.QuitRace(); // calls the Quit Race function in the RaceManager class 
    } 
 
    public void Pause() // function to pause the game 
    { 
        isPaused = !isPaused; // making isPaused boolean the opposite of what it was before pressing escape 
        pauseScreen.SetActive(isPaused); //making the pause screen panel with all of its Text Mesh Pro Texts the same boolean (on or off) as the isPaused variable 
        if (isPaused) // if it is paused 
        { 
            Time.timeScale = 0f; // pauses the in game speed to 0 
        }else{ 
            Time.timeScale = 1f; // if it is not paused then changes the in game speed to 1 (default) 
        } 
    } 
 
    public void QuitGame() //funtion to quit the game 
    { 
        Application.Quit(); // closes the application/game 
    } 
} 
