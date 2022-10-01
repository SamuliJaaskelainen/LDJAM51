using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturableRoot : MonoBehaviour
{
    public Transform physicsFragmentRoot;
    public FractureOptions fractureOptions;
    public int levelsOfRecursion = 2;
    private float startingMass = 1.0f;
    private float massLost = 0.0f;
    private int childrenLost = 0;
    private bool shattered = false;

    public void Start()
    {
        var fracturable = gameObject.GetComponentInChildren<Fracturable>();
        startingMass = fracturable.GetComponent<Rigidbody>().mass;
        fracturable.root = this;
    }

    public float GetHealth()
    {
        float maxChildren = Mathf.Pow(fractureOptions.fragmentCount, levelsOfRecursion);
        return (maxChildren - childrenLost) / maxChildren;
        // return 1.0f - (massLost / startingMass); 
    }

    public void Shatter()
    {
        if (shattered)
        {
            return;
        }
        shattered = true;
        var fracturables = gameObject.GetComponentsInChildren<Fracturable>();
        foreach (var fracturable in fracturables)
        {
            fracturable.BecomePhysicsFragmenst(0.5f * Random.onUnitSphere);
        }
    }

    public void ChildBecamePhysicsFragment(Rigidbody childRigidbody)
    {
        massLost += childRigidbody.mass;
        ++childrenLost;
        //Debug.Log("Health left: " + GetHealth());
    }
}
