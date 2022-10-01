using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float radius = 0.4f;
    public float impulse = -0.5f;
    protected RaycastHit hit;
    protected int shooterId;

    public void Init(int id)
    {
        shooterId = id;
    }

    void Update()
    {
        Vector3 nextPosition = transform.position + transform.forward * speed * Time.deltaTime;

        if (Physics.Linecast(transform.position, nextPosition, out hit))
        {
            Debug.Log("Bullet hit: " + hit.transform.name, gameObject);

            if (hit.transform.tag == "Button")
            {
                hit.transform.SendMessage("Press");
            }

            Collider[] colliders = Physics.OverlapSphere(hit.point, radius);
            foreach (var collider in colliders)
            {
                var fracturable = collider.GetComponent<Fracturable>();
                if (fracturable == null)
                {
                    continue;
                }
                ImpactInfo impactInfo = new ImpactInfo();
                impactInfo.position = hit.point;
                impactInfo.radius = radius;
                impactInfo.impulse = (transform.position - nextPosition).normalized * impulse;
                fracturable.CauseFracture(impactInfo);
            }
            Debug.DrawLine(transform.position, nextPosition, Color.red);
            Destroy(gameObject);
        }
        else
        {
            Debug.DrawLine(transform.position, nextPosition, Color.green);
        }

        transform.position = nextPosition;
    }
}
