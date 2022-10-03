using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FracturableRoot : MonoBehaviour
{
    public Transform physicsFragmentRoot;
    public FractureOptions fractureOptions;
    public int levelsOfRecursion = 2;
    private float massLost = 0.0f;
    private int childrenLost = 0;
    private bool shattered = false;
    
    public void Start()
    {
        var fracturable = gameObject.GetComponentInChildren<Fracturable>();
        fracturable.mass = GetComponent<Rigidbody>().mass;
        fracturable.root = this;

        if (physicsFragmentRoot == null) {
            physicsFragmentRoot = transform.parent.Find("PhysicsFragments");
        }
        if (physicsFragmentRoot == null) {
            physicsFragmentRoot = new GameObject("PhysicsFragments").transform;
            physicsFragmentRoot.parent = transform;
        }
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
        switch (Random.Range(0, 7))
        {
            case 0:
                AudioManager.Instance.PlaySound("glassbreak01", transform.position);
                break;
            case 1:
                AudioManager.Instance.PlaySound("glassbreak02", transform.position);
                break;
            case 2:
                AudioManager.Instance.PlaySound("glassbreak03", transform.position);
                break;
            case 3:
                AudioManager.Instance.PlaySound("glassbreak04", transform.position);
                break;
            case 4:
                AudioManager.Instance.PlaySound("glassbreak05", transform.position);
                break;
            case 5:
                AudioManager.Instance.PlaySound("glassbreak06", transform.position);
                break;
            case 6:
                AudioManager.Instance.PlaySound("glassbreak07", transform.position);
                break;
        }
        Destroy(gameObject);
    }

    public void ChildBecamePhysicsFragment(Rigidbody childRigidbody)
    {
        massLost += childRigidbody.mass;
        ++childrenLost;
        //Debug.Log("Health left: " + GetHealth());
    }

}
