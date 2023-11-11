using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CarControl : MonoBehaviour 
{ 
    public float maxSpeed; // storing the max speed of the car 
    public Rigidbody RB; // storing the rigid body of the car 
    private float speedInput; // storing the user speed input of the car 
    public float forwardAccel = 8f, reverseAccel = 4f; // presetting hte forward acceleration and the reverse acceleration of the car 
    private float turnInput; // storing the user turn input of the car 
    public float turnStength = 180f; // making hte turn strenght equal to 180 
    public Transform leftFrontWheel, rightFrontWheel; // storing the mesh of the front left and front right tyre 
    public float maxWheelTurn = 25f; // making so that at max the car can turn 25 degree 
    public ParticleSystem[] dustTrail; // adding hte particles for the dust trail to the array 
    public float maxEmission = 25f, emissionFadeSpeed = 20f; // setting hte max emmision rate of the dust particle and the fade away of the particle 
    private float emissionRate = 0f; // setting the intial emission rate of dust particle to zero 
    public AudioSource engineSound; // adding the engine sound 
    public int nextCP; // storing the next checkpoint 
    public int currentLap = 1; // storing the current lap the car is at 
    public float lapTime, bestLap; // storing the current lap timing and the best lap timing 
    public bool isAI; // boolean to check if the car is AI or not 
    public int currentTarget; // assigning a target for the AI to follow 
    private Vector3 targetPoint; // assigning the position of the target for the AI to follow 
    public float aiAcceSpeed = 1f, aiTurnSpeed = 0.8f, aiReachPointRange = 5f, aiPointVariance = 3f, aiMaxTurn = 15f; // presetting the AI acceleration speed, turn speed, max turn agnle, checkpoint variance and the reach of the checkpoint 
    private float aiSpeedInput, aiSpeedMod; // adding the AI speed input and the speed modification 
 
    void Start() // Start is called before the first frame update 
    { 
        maxSpeed = RaceInfo.instance.carMaxSpeed; // assigning the max speed depeding on what the user chose to be the AI difficulty using the information carried from RaceInfo from the main menu 
        RB.transform.parent = null; // setting hte rigid body to not have a parent object 
        emissionRate = 25f; // setting the default emission rate of the dust particles to 25 
        if (isAI) // checking if the car is AI 
        { 
            targetPoint = RaceManager.instance.allCP[currentTarget].transform.position; // changing the target position to the next checpoint position 
            RandomAITarget(); // calling the random AITarget to get a new random path to follow with new checkpoint variance and range 
            aiSpeedMod = Random.Range(0.8f, 1.1f); // changing hte speed modifies to a random number between 0.8 and 1.1 to have different AI speeds 
        } 
        UIManager.instance.lapCounterText.text = currentLap + "/" + RaceManager.instance.totalLaps; // changing the lap count message at the bottom left of the screen to be the current lap number out of the total lap number 
    } 
 
    void Update()  // Update is called once per frame 
    { 
        if(RaceManager.instance.isStarting == false){ // checking if the race is not in the countdown section 
            lapTime += Time.deltaTime; // starts incrementing the lap time 
 
            if (isAI == false) // checking if the car is AI 
            { 
                var ts = System.TimeSpan.FromSeconds(lapTime); // changing the lap time to a formatable variable 
                UIManager.instance.currentLapText.text = string.Format("{0:0}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds); // formating the lap time to be displayed at the top left of the screen as 0:00.000 
 
                speedInput = 0f; // changes the speed input to 0 
                if(Input.GetAxis("VerticalP2") > 0) // checks if the user pressed 'w' 
                { 
                    speedInput = Input.GetAxis("VerticalP2") * forwardAccel; // increases the forward torque by the product of w pressed and forwards acceleration number 
                }else if (Input.GetAxis("VerticalP2") < 0) // checks if the user pressed 's' 
                { 
                    speedInput = Input.GetAxis("VerticalP2") * reverseAccel; // increases the backwards torque by the product of s pressed and the reverse acceleration number 
                } 
                 
                turnInput = Input.GetAxis("HorizontalP2"); // changes the turn input depending on if the user pressed 'a' or 'd' 
                if(Input.GetKeyDown(KeyCode.R)) // checks if the user pressed 'r' 
                { 
                    ResetTrack(); // calls the ResetTrack function to reset the car back onto the track 
                } 
            }else{ // if the car is a bot 
                targetPoint.y = transform.position.y; // makes sure that the AI car is always on the same y axis as the checkpoint 
                if(Vector3.Distance(transform.position, targetPoint)<aiReachPointRange) // checks if the distance between the checkpoint and the AI car is not within its Checkpoint range 
                { 
                    currentTarget ++; //increases the checkpoint by one 
                    if (currentTarget >= RaceManager.instance.allCP.Length) // checks if there is a checkpoint when increasing it by one 
                    { 
                        currentTarget = 0; // if there is not, then it resets to zero as new lap has started 
                    } 
                    targetPoint = RaceManager.instance.allCP[currentTarget].transform.position; // changes the checkpoint location to follow to the new checkpoint 
                    RandomAITarget(); // receives a new random checkpoint variance and range 
                } 
                Vector3 targetDire = targetPoint - transform.position; //changes the car direction to the checkpoint 
                float angle = Vector3.Angle(targetDire, transform.forward); // changes the angle of the car to the checkpoint while driving forwards 
                Vector3 localPos = transform.InverseTransformPoint(targetPoint); // changes the position of the car 
                if(localPos.x < 0f) // checks if the car has a negative x coordinate meaning it is going opposite turn 
                { 
                    angle = -angle; // changes the angle to negative of itself 
                } 
                turnInput = Mathf.Clamp(angle/aiMaxTurn, -1f, 1f); // changes the AI turn input to the angle needed divided by AI max turn angle 
                if(Mathf.Abs(angle)<aiMaxTurn) // checking if the angle is smaller than the AI max turn angle 
                { 
                    aiSpeedInput = Mathf.MoveTowards(aiSpeedInput, 1f, aiAcceSpeed); // if it is, then it accelerates the car in that angle 
                }else{ 
                    aiSpeedInput = Mathf.MoveTowards(aiSpeedInput, aiTurnSpeed, aiAcceSpeed); // else decreases the turn speed and still accelerates the car in that angle 
                } 
                speedInput = aiSpeedInput * forwardAccel * aiSpeedMod; // changes the AI speedInput to the product of itself, forward acceleration and the speed modifier 
            } 
 
            leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput*maxWheelTurn)-180, leftFrontWheel.localRotation.eulerAngles.z); //adding the rotation animation to the front left wheel of the car 
            rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, (turnInput*maxWheelTurn), rightFrontWheel.localRotation.eulerAngles.z); // adding the rotation animation to the fron left wheel of the car 
             
            if(Mathf.Abs(turnInput) > 0.5f || (RB.velocity.magnitude < maxSpeed * .5f && RB.velocity.magnitude != 0 )) // checking if the turn angle or velocity is 0 or too high to release the particle dust effect 
            { 
                emissionRate = maxEmission; // dust particle will be released at max rate possible 
            } 
            if (RB.velocity.magnitude < 0.5f) // checking if car velocity is smaller than 0.5 
            { 
                emissionRate = 0; // changes the emission of dust particles to 0 
            } 
 
            emissionRate = Mathf.MoveTowards(emissionRate, 0f, emissionFadeSpeed*Time.deltaTime); //makes the emission of dust particles fade off at the rate of emission rate over time span of emission rate times by the time passed 
            for(int i = 0;i<dustTrail.Length;i++) // looping through each dust trail array 
            { 
                var emissionModule = dustTrail[i].emission; // adding specific dust trail to emission module 
                emissionModule.rateOverTime = emissionRate; // making the dust particle start big then get smaller in size 
            } 
 
            if(engineSound != null) // checking if there is engine sound 
            { 
                engineSound.pitch = 1f + ((RB.velocity.magnitude /maxSpeed) * 2); // increases the pitch of the engine as the car increases its speed 
            } 
        } 
    } 
 
    private void FixedUpdate() //updates 50 times per second (mainly used for physics) 
    { 
        RB.AddForce(transform.forward*speedInput); // adding a forward force to the car relative to its speed input 
        if (RB.velocity.magnitude > maxSpeed) // checking if the cars speed is faster than the max speed 
        { 
            RB.velocity = RB.velocity.normalized * maxSpeed; // decreasing the speed 
        } 
        transform.position = RB.position; // moving the car model forward 
        if(speedInput != 0) // checking if the user is moving 
        { 
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput*turnStength*Time.deltaTime*Mathf.Sign(speedInput)*(RB.velocity.magnitude/maxSpeed),0f)); // assigning a rotation angle depending on the velocity, max speed, turn input, turn stength and its location 
        } 
    } 
 
    public void CPHit(int CPNumber) // called when the car collides with an invisible checkpoint 
    { 
        if(CPNumber == nextCP) // checking if the checkpoint is the next checkpoint 
        { 
            nextCP ++; // increases the next checkpoint by oen 
            if(nextCP == RaceManager.instance.allCP.Length) // checking if the next checkpoint is greater than the amount of checkpoints 
            { 
                nextCP = 0; // resets the checkpoint to zero meaning to lap is completed 
                LapComplete(); // calling hte lap completed function to complete the lap 
            } 
        } 
        if(isAI) // checking if it is AI 
        { 
            if(CPNumber == currentTarget) // checking if the checkpoint number is the current checkpoint 
            { 
                currentTarget ++; // increases the checkpoint by one 
                if (currentTarget >= RaceManager.instance.allCP.Length) // checks if the checkpoint exists in the checkpoints array 
                { 
                    currentTarget = 0; // if not, then it is set to zero as new lap has started 
                } 
                targetPoint = RaceManager.instance.allCP[currentTarget].transform.position; // changes the AI to drive to the next checkpoint 
                RandomAITarget(); // receives a new checkpoint variance and range 
            } 
        } 
    } 
 
    public void LapComplete() // lap completed function 
    { 
        currentLap ++; // increases the current lap by one 
        if(lapTime < bestLap || bestLap == 0) // checking if it is the best lap the user has done 
        { 
            bestLap = lapTime; // assigning the lap time as the users best lap 
        } 
        if(currentLap<=RaceManager.instance.totalLaps) // checking if this was the last lap 
            { 
 
            lapTime = 0; // resets the lap time 
            if(isAI == false) // checks if it is AI 
            { 
                UIManager.instance.lapCounterText.text = currentLap + "/" + RaceManager.instance.totalLaps; // displayes the amount of laps driven by the amount of laps in total 
                var ts = System.TimeSpan.FromSeconds(bestLap); // conversts best lap to formatable time 
                UIManager.instance.bestLapText.text = string.Format("{0:0}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds); // displayes hte formatable time for the user in Best Lap Text Mesh Pro Text 
            } 
        } 
        else{ // if it was the last lap 
            if(!isAI) // checks if it is AI 
            { 
                isAI = true; // makes the car be controlled by AI 
                aiSpeedMod = 1f; // makes AI speed to one 
                targetPoint = RaceManager.instance.allCP[currentTarget].transform.position; // assigns a checkpoint to follow 
                RandomAITarget(); // receives a new checkpoint variance and range 
                var ts = System.TimeSpan.FromSeconds(bestLap); // conversts best lap to formatable time 
                UIManager.instance.bestLapText.text = string.Format("{0:0}:{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds); // displayes hte formatable time for the user in Best Lap Text Mesh Pro Text 
                RaceManager.instance.finishRace(bestLap); //calls the finishRace function and sends the best lap timings 
            } 
        } 
    } 
 
    public void RandomAITarget() // assigns a random checkpoint variance and range for the AI 
    { 
        targetPoint += new Vector3(Random.Range(-aiPointVariance,aiPointVariance),0f,Random.Range(-aiPointVariance,aiPointVariance)); // assigns a random checkpoint variance and range for the AI 
    } 
 
    void ResetTrack() // resetting to track 
    { 
        int pointGo = nextCP -1; // checking the last checkpoint 
        if(pointGo < 0) // checking if the user just started the lap 
        { 
            pointGo = RaceManager.instance.allCP.Length -1; // resets the user to the start 
        } 
        transform.position = RaceManager.instance.allCP[pointGo].transform.position; // stores the position of the checkpoint 
        RB.transform.position = transform.position; // moves the user car model to the position of the checkpoint 
        RB.velocity = Vector3.zero; // makes the user temporarly have 0 velocity 
        speedInput = 0f; // makes the user temporarly have 0 speed input 
        turnInput = 0f; // makes the user temporarly have 0 turn input 
    } 
} 
