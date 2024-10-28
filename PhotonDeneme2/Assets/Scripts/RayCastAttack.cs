using Fusion;
using UnityEngine;

public class RayCastAttack : NetworkBehaviour
{
    public float Damage = 10;

    public PlayerMovement playerMovement;

    private void Update()
    {
        if(HasStateAuthority == false)
        {
            return;
        }

        Ray ray = playerMovement.Camera.ScreenPointToRay(Input.mousePosition);
        ray.origin += playerMovement.Camera.transform.forward;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);

            if(Physics.Raycast(ray.origin, ray.direction, out var hit))
            {
                if(hit.transform.TryGetComponent<Health>(out var health))
                {
                    health.DealDamageRpc(Damage);
                }
            }
        }
    }
}
