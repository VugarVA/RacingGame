using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class CollisionSound : MonoBehaviour 
{ 
    public AudioSource sound; // adding the audio source which needs to be played 
 
    private void OnCollisionEnter(Collision other) // unities own method which is called when two objects collide with each other 
    { 
        if (other.gameObject.layer != 3) // as there are no jumpoing or flying, touching the ground at the start shouldnt be counted as a collision therefore put all the track to ground layer therefore it doesnt output a sound when colliding with the track 
        { 
            sound.Stop(); //if it collides, it will stop the sound in case it is playing 
            sound.pitch = Random.Range(0.8f, 1.2f); // it will create a random pitch between 0.8 and 1.2 of the sound which we have given 
            sound.Play(); // it will play the sound 
        } 
    } 
} 
