using UnityEngine;

public class ClickToFracture : MonoBehaviour
{
    public float radius = 0.3f;
    public float impulse = -0.1f;
    private Fracturable fracturable;

    private void Start()
    {
        fracturable = gameObject.GetComponent<Fracturable>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 1000f))
            {
                return;
            }
            Fracturable.CauseFractures(hit.point, radius, ray.direction * impulse);
        }
    }
}