using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(FracturableRoot))]
public class EnemyBullet : MonoBehaviour {
    public  float radius   = 0.3f;
    public  float speed    = 5.0f;
    private float lifetime = 0.0f;
    void Update() {
        lifetime += Time.deltaTime;
        if (lifetime > 10.0f) {
            Destroy(gameObject);
        }
        
        Transform  target    = HitPoints.Instance.transform;
        Vector3    direction = target.position - transform.position;
        Vector3    delta     = speed * Time.deltaTime * direction.normalized;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, delta, out hit, delta.magnitude)) {
            // Collisions with player handled by separate script
            if (hit.collider.gameObject == target.gameObject) {
                return;
            }
            Fracturable.CauseFractures(hit.point, 2 * radius, 0.1f * speed * hit.normal);
        }
        
        transform.position += delta;
    }
}
