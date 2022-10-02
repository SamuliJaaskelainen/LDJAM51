using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float radius = 0.4f;
    public float impulse = -0.5f;
    public int penetration = 1;
    protected RaycastHit hit;
    protected int shooterId;
    protected int targetsHit = 0;

    public void Init(int id)
    {
        shooterId = id;
        Destroy(gameObject, 3.0f);
    }

    public virtual void Update()
    {
        float dt = Shop.shopOpen ? Time.unscaledDeltaTime : Time.deltaTime;
        Vector3 nextPosition = transform.position + transform.forward * speed * dt;
        float lenght = Mathf.Max(speed * dt, 1.0f);

        if (Physics.Raycast(transform.position, transform.forward, out hit, lenght))
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
        }
        else
        {
            Debug.DrawLine(transform.position, nextPosition, Color.green);
        }

        transform.position = nextPosition;
    }
}
