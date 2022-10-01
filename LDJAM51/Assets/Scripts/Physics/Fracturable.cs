using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ImpactInfo {
    public Vector3 position;
    public float   radius;
    public Vector3 impulse;
}

public class Fracturable : MonoBehaviour {
    public  bool              debug             = false;
    [HideInInspector] public  FracturableRoot   root;
    private int               recursionLevel;

    public void CauseFracture(ImpactInfo impactInfo) {
        Debug.Assert(root != null);
        
        if (!IsWithinRadius(impactInfo.position, impactInfo.radius)) {
            return;
        }
        
        if (recursionLevel >= root.levelsOfRecursion) {
            BecomePhysicsFragmenst(impactInfo.impulse);
            return;
        }

        var mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null) {
            return;
        }

        GameObject fragmentRoot = BecomeFragments();
        UpdateChildren(fragmentRoot.transform, impactInfo);

        Destroy(this);
        GameObject.Destroy(gameObject);
    }

    private GameObject CreateFragmentTemplate() {
        // If pre-fracturing, make the fragments children of this object so they can easily be unfrozen later.
        // Otherwise, parent to this object's parent
        GameObject obj = new GameObject();
        obj.name = "Fragment";
        obj.tag  = tag;

        foreach (var component in GetComponents(typeof(Component))) {
            if (component is Transform or MeshFilter or MeshRenderer or Rigidbody or Collider or Fracturable) {
                continue;
            }
            // Debug.Log(component.GetType());
            CopyComponent(component, obj);
        }
        
        // Update mesh to the new sliced mesh
        obj.AddComponent<MeshFilter>();

        // Add materials. Normal material goes in slot 1, cut material in slot 2
        var meshRenderer = obj.AddComponent<MeshRenderer>();

        meshRenderer.sharedMaterials = new Material[2] {
            GetComponent<MeshRenderer>().sharedMaterial,
            root.fractureOptions.insideMaterial
        };

        // Copy collider properties to fragment
        var thisCollider     = GetComponent<Collider>();
        var fragmentCollider = obj.AddComponent<MeshCollider>();
        fragmentCollider.convex         = true;
        fragmentCollider.sharedMaterial = thisCollider.sharedMaterial;
        fragmentCollider.isTrigger      = thisCollider.isTrigger;

        // Copy rigid body properties to fragment
        var thisRigidBody = GetComponent<Rigidbody>();
        if (thisRigidBody) {
            var fragmentRigidBody = obj.AddComponent<Rigidbody>();
            fragmentRigidBody.velocity         = thisRigidBody.velocity;
            fragmentRigidBody.angularVelocity  = thisRigidBody.angularVelocity;
            fragmentRigidBody.drag             = thisRigidBody.drag;
            fragmentRigidBody.angularDrag      = thisRigidBody.angularDrag;
            fragmentRigidBody.useGravity       = thisRigidBody.useGravity;
            fragmentRigidBody.isKinematic      = thisRigidBody.isKinematic;
            fragmentRigidBody.solverIterations = 1;
        }

        CopyFractureComponent(obj);
        return obj;
    }
    
    public void BecomePhysicsFragmenst(Vector3 impulse) {
        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null) {
            rigidbody = this.AddComponent<Rigidbody>();
        }
        rigidbody.isKinematic = false;
        rigidbody.AddForce(impulse, ForceMode.Impulse);
        transform.SetParent(root.physicsFragmentRoot, true);
        root.ChildBecamePhysicsFragment(rigidbody);
        Destroy(this);
        return;
    }

    private GameObject BecomeFragments() {
        GameObject fragmentRoot = new GameObject(name);
        fragmentRoot.transform.SetParent(transform.parent);
        fragmentRoot.transform.position   = transform.position;
        fragmentRoot.transform.rotation   = transform.rotation;
        fragmentRoot.transform.localScale = Vector3.one;
        var fragmentTemplate = CreateFragmentTemplate();
        Fragmenter.Fracture(
            gameObject,
            root.fractureOptions,
            fragmentTemplate,
            fragmentRoot.transform
        );
        Destroy(fragmentTemplate);
        return fragmentRoot;
    }
 
    private void UpdateChildren(Transform fragmentRoot, ImpactInfo impactInfo) {
        // Save children as they may change during iteration
        var childTransforms = new Transform[fragmentRoot.childCount];
        int i               = 0;
        foreach (Transform child in fragmentRoot) {
            childTransforms[i++] = child;
        }
        foreach (Transform child in childTransforms) {
            var childFracturable = child.GetComponent<Fracturable>();
            // childFracturable.GetComponent<MeshCollider>().convex = false;
            if (childFracturable == null) {
                continue;
            }
            childFracturable.recursionLevel      = recursionLevel + 1;
            childFracturable.root                = root;
            if (debug) {
                var material = new Material(GetComponent<MeshRenderer>().sharedMaterial);
                material.color = new Color(
                    recursionLevel == 0 ? Random.Range(0f, 1f) : 0,
                    recursionLevel == 1 ? Random.Range(0f, 1f) : 0,
                    recursionLevel >= 2 ? Random.Range(0f, 1f) : 0
                );
                childFracturable.GetComponent<MeshRenderer>().material = material;
                childFracturable.debug                                 = true;
            }
            childFracturable.CauseFracture(impactInfo);
        }
    }
    
    private bool IsWithinRadius(Vector3 point, float radius) {
        var   fragmentCollider     = GetComponent<Collider>();
        var   closestPointOnBounds = fragmentCollider.ClosestPointOnBounds(point);
        float distanceOnBounds     = Vector3.Distance(closestPointOnBounds, point);

        if (distanceOnBounds > radius) {
            // return;
        }

        var   closestPoint = fragmentCollider.ClosestPoint(point);
        float distance     = Vector3.Distance(closestPoint, point);

        // Debug.LogFormat("Recursion {0} Distance: {1}", recursionLevel, distance);
        if (distanceOnBounds > radius) { // distance may not be accurate, collider out of date??
            // Debug.DrawLine(impactInfo.position, closestPoint, Color.red, 9999.0f);
            return false;
        } else {
            // Debug.DrawLine(impactInfo.position, closestPoint, Color.green, 9999.0f);
            // Debug.DrawLine(impactInfo.position, impactInfo.position - Vector3.forward, Color.green, 9999.0f);
            return true;
        }
    }

    private void CopyFractureComponent(GameObject obj) {
        var fractureComponent = obj.AddComponent<Fracturable>();
    }

    private static Component CopyComponent(Component original, GameObject destination) {
        if (original == null) {
            return null;
        }
        
        System.Type type = original.GetType();
        Component   copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields) {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}