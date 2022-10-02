using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    public override void Update()
    {
        float dt = Shop.shopOpen ? Time.unscaledDeltaTime : Time.deltaTime;
        Vector3 nextPosition = transform.position + transform.forward * speed * dt;

        if (Physics.Linecast(transform.position, nextPosition, out hit))
        {
            Debug.Log("Bullet hit: " + hit.transform.name, gameObject);

            if (hit.transform.tag == "Button")
            {
                if (hit.transform != null)
                {
                    hit.transform.SendMessage("Press", shooterId);
                }
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

            targetsHit++;

            if (targetsHit >= penetration)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.forward = Vector3.Reflect(transform.forward, hit.normal);
            }
        }
        else
        {
            Debug.DrawLine(transform.position, nextPosition, Color.green);
        }

        transform.position = nextPosition;
    }
}
