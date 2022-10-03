using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song : MonoBehaviour
{

    // Song setup
    public int index; // for current book-keeping/future merchandising
    public int bpm; // when we add gfx strobing to the beat and make game unplayable


    // Base audio stems. These always play
    public AudioSource fullsong;

    public Song(int index, int bpm, AudioSource fullsong) {
        this.index = index;
        this.bpm = bpm;
        this.fullsong = fullsong;
    }
    public void Properties() {
        Debug.Log("Song: " + index);
        Debug.Log("BPM: " + bpm);
    }

}