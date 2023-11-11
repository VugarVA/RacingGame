using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.SceneManagement; 
 
public class RaceManager : MonoBehaviour 
{ 
    public static RaceManager instance; // adding the RaceManager instance 
    public Checkpoints[] allCP; // creates an array with all the checkpoints stored inside it 
    public int totalLaps; // stores the total number of laps needed to be completed 
    public CarControl playerCar; // stores the player car 
    public List<CarControl> aiCars = new List<CarControl>(); // creates a list with all the AI cars added to the list 
    public int playerPos; // stores the position of the player car 
    public float aiDefSpeed = 30f, playerDefSpeed = 30f, rubberBandSpeedMod = 3.5f, rubberBandAccel = 0.5f; // prewritten float for ai and player default speed, and rubberband/slipstrean moddifier and acceleration 
    public bool isStarting; // checking if the game countdown is playing 
    public float timeStartCountDiff = 1f; // stores amount of seconds needed between each number of 3 - 2 - 1 - GO! 
    private float startCounter; // variable storing the time at the start 
    public int currentCountDown = 3; // number which is currently displaying on the user screen 
    public int playerStartPos, aiNumbers; // player starting grid position and the amount of AI on the grid 
    public Transform[] startPoints; // all the available grid positions stored in an array 
    public List<CarControl> carSpawn = new List<CarControl>(); // list of all the prefab made AI cars to be placed on the grid 
    public bool raceComplete = false; // checks if the race is completed or not 
    public string raceCompleteScene; // storing the main menu scene 
 
    private void Awake() // unity function called at the start of the program 
    { 
        instance = this; // making an instance of this class 
        totalLaps = RaceInfo.instance.numberLaps; // getting the disred laps assigned by the user from RaceInfo instance 
    } 
 
    void Start()   // Start is called before the first frame update 
    { 
        for(int i = 0; i<allCP.Length; i++) // does a for loop through the checkpoints 
        { 
            allCP[i].cpNumber = i; // assigning a value for all the checkpoints 
        } 
        isStarting = true; // making the program know that the game is starting 
        startCounter = timeStartCountDiff; // making the time equal to 1 
        UIManager.instance.countDownText.text = currentCountDown + "!"; // displaying the current countdown number (in this case 3!) on the UIManager Text Mesh Pro Text 
        playerStartPos = Random.Range(0, aiNumbers + 1); // creates the player grid position by randomly choosing numbers between 0 and the amount of AI and user cars 
        playerCar.transform.position = startPoints[playerStartPos].position; // places the user car to the specific grid position which was randomly chosen 
        playerCar.RB.transform.position = startPoints[playerStartPos].position; // places the user rigid body to the specific grid position which was randomly chosen 
        for(int i = 0; i < aiNumbers + 1; i++) // does for loop for the AI 
        { 
            if(i != playerStartPos) // checking if i does not equal to the grid position of the user 
            { 
                int selectedCar = Random.Range(0, carSpawn.Count); // choses a random number from AI list and 0 
                carSpawn[selectedCar].transform.position = startPoints[i].position; // places the AI car to the grid position according to the random number chosen 
                carSpawn[selectedCar].RB.transform.position = startPoints[i].position; // places the AI Rigid Body to the grid position according to the random number chosen 
                carSpawn.RemoveAt(selectedCar); // removes the AI car which was placed on the grid from the list of AI Cars available 
            } 
        } 
    } 
 
    void Update()    // Update is called once per frame 
    { 
        if(isStarting) // checking if it is the countdown 
        { 
            startCounter -= Time.deltaTime; // subtracting 1 every second from the startCounter 
            if(startCounter <= 0) // checking if the start counter is below or equal to zero 
            { 
                currentCountDown--; // decreasing the count down as number has been shown 
                startCounter = timeStartCountDiff; //changes the counter to the starting counter difference 
                UIManager.instance.countDownText.text = currentCountDown + "!"; // changes the Text Mesh Pro Text to the current count down with an exclamatiun mark 
                if(currentCountDown == 0) // checking if the coutdown equals to zero 
                { 
                    isStarting = false; // changing isStarting to false to let the program know that the game may start 
                    UIManager.instance.countDownText.gameObject.SetActive(false); // disabling the coundown Text Mesh Pro Text 
                    UIManager.instance.goText.gameObject.SetActive(true); // displayes the GO text on the screen for the user 
                } 
            } 
        }else{ // if it is not in the countdown 
            playerPos = 1; // player position is changed to 1 
            foreach(CarControl aiCar in aiCars) // for each AI car 
            { 
                if(aiCar.currentLap > playerCar.currentLap) // checking if AI Car has a greated lap number than the user car 
                { 
                    playerPos ++; // if it does, then the player moves up the leaderboard by one 
                }else if(aiCar.currentLap == playerCar.currentLap) // checking if they are on the same lap 
                { 
                    if(aiCar.nextCP > playerCar.nextCP) // checking if the AI car checkpoint is behind the player checkpoint 
                    { 
                        playerPos ++; // if it is, the player moves up the leaderboard by one 
                    }else if (aiCar.nextCP == playerCar.nextCP){ // checking if AI and the user are both at the same checkpoint 
                        if(Vector3.Distance(aiCar.transform.position, allCP[aiCar.nextCP].transform.position) < Vector3.Distance(playerCar.transform.position,allCP[playerCar.nextCP].transform.position)) // checking if the players distance from the checkpoints is shorter than the AI distance from the checkpoint 
                        { 
                            playerPos ++; // if it is, the player moves up the leaderboard by one 
                        } 
                    }   
                } 
            } 
            UIManager.instance.posText.text = playerPos + "/" + (aiCars.Count + 1); // writes on the instance of the UIManager Text Mesh Pro Text the Player position seperated by / and the total amount of cars on the track 
            if (playerPos == 1) // checking if player is in first place 
            { 
                foreach(CarControl aiCar in aiCars) // cycling through each of hte AI cars 
                { 
                    aiCar.maxSpeed = Mathf.MoveTowards(aiCar.maxSpeed, aiDefSpeed + rubberBandSpeedMod, rubberBandAccel * Time.deltaTime); // increasing the AI max speed depeding on the rubberband/slipstream modifier and acceleration 
                } 
                playerCar.maxSpeed = Mathf.MoveTowards(playerCar.maxSpeed, playerDefSpeed - rubberBandSpeedMod, rubberBandAccel * Time.deltaTime); // decreases the user max speed depeding on the rubberband/slipstream modifier and acceleration 
            }  else{ // if user is not in 1st place 
                foreach(CarControl aiCar in aiCars) // cycles through the AI bots 
                { 
                    aiCar.maxSpeed = Mathf.MoveTowards(aiCar.maxSpeed, aiDefSpeed - (rubberBandSpeedMod*((float)playerPos/((float)aiCars.Count + 1))), rubberBandAccel * Time.deltaTime); // decreases AI max speed depending on their position on track 
                } 
                playerCar.maxSpeed = Mathf.MoveTowards(playerCar.maxSpeed, playerDefSpeed + (rubberBandSpeedMod*((float)playerPos/((float)aiCars.Count + 1))), rubberBandAccel * Time.deltaTime); // increases the user max speed depeding on the position on track 
            } 
        } 
    } 
 
    public void finishRace(float bestLap) 
    { 
        raceComplete = true; // letting the program know that the race is complete 
        var ts = System.TimeSpan.FromSeconds(bestLap); // changing the best lap to a formatable time version and storing it in ts 
        UIManager.instance.bestLapTimeText.text = string.Format("Your best time was {0:0}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds); // formating and outputing the best time lap message to the user in the format of 0:00.000 
        switch(playerPos) // switching the statement depeding on the player position at the end of the race 
        { 
            case 1:// if the player finished 1st 
                UIManager.instance.raceResultText.text = "You finished 1st"; // changes the UIManager raceResult Text Mesh Pro Text to You Finished 1st 
                break; // breaks from these case statements 
            case 2:// if the player finished 2nd 
                UIManager.instance.raceResultText.text = "You finished 2nd"; // changes the UIManager raceResult Text Mesh Pro Text to You Finished 2nd 
                break; // breaks from these case statements 
            case 3:// if the player finished 3rd 
                UIManager.instance.raceResultText.text = "You finished 3rd"; //changes the UIManager raceResult Text Mesh Pro Text to You Finished 3rd 
                break; // breaks from these case statements 
            default:// if the player finished anything but top three 
                UIManager.instance.raceResultText.text = "You finished " +playerPos + "th"; //changes the UIManager raceResult Text Mesh Pro Text to You Finished player postion  and the th at the end 
                break; // breaks from these case statements 
        } 
        UIManager.instance.resultsPanel.SetActive(true); // making the result panel true for the user to see the results 
    } 
 
    public void QuitRace() // method to return to main screen 
    { 
        SceneManager.LoadScene(raceCompleteScene); // starts up the main menu screen scene 
    } 
} 
