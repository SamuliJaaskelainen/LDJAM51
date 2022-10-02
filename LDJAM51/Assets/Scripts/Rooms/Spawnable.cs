using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawnable : MonoBehaviour {
    [FormerlySerializedAs("difficulty")] 
    [FormerlySerializedAs("addsToDifficulty")] 
    public float points  = 1.0f;
}
