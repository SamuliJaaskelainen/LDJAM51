using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FracturableRoot))]
public class ShatterOnLowHealth : MonoBehaviour {
    public float minHealth = 0.5f;
    void Update() {
        var fracturableRoot = GetComponent<FracturableRoot>();
        if (fracturableRoot.GetHealth() < minHealth) {
            fracturableRoot.Shatter();    
        }
    }
}
