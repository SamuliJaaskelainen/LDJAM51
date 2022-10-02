using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FracturableRoot))]
public class DamageOnContact : MonoBehaviour {
    public float timeAlive = 0.0f; 
    // On contact with trigger

    private void Update() {
        timeAlive += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other) {
        if (timeAlive < 1.0f) {
            // Prevent wonkiness with spawn as player may not be in correct position yet
            return;
        }
        
        var hp          = other.GetComponent<HitPoints>();
        if (hp) {
            var rigidbody = GetComponent<Rigidbody>();

            float      radius    = 3.5f;
            float      impulse   = 0.5f;
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                var fracturable = collider.GetComponent<Fracturable>();
                if (fracturable == null)
                {
                    continue;
                }
                ImpactInfo impactInfo = new ImpactInfo();
                impactInfo.position = hp.transform.position;
                impactInfo.radius   = radius;
                impactInfo.impulse  = impulse * Random.onUnitSphere;
                fracturable.CauseFracture(impactInfo);
            }
            GetComponent<FracturableRoot>().Shatter();
            
            hp.TakeDamage();
        }
    }
}
