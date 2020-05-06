using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform root;
    public Transform tip;
    public GameObject impactVFX;
    public float speed = 1;
    public float maxLifeTime = 5f;
    public float gravityDownAcceleration = 0;
    public float radius = 0.01f;
    public float impactVFXLifetime = 5f;


    public GameObject owner { get; private set; }
    public Vector3 initialPosition { get; private set; }
    public Vector3 initialDirection { get; private set; }
    public float initialCharge { get; private set; }

    float shootTime;
    Vector3 lastRootPosition;
    Vector3 velocity;



    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    public void Shoot(WeaponController controller)
    {
        owner = controller.owner;
        initialPosition = transform.position;
        initialDirection = transform.forward;
        initialCharge = controller.currentCharge;

        OnShoot();

    }
    

    void OnShoot()
    {
        shootTime = Time.time;
        lastRootPosition = root.position;
        velocity = transform.forward * speed;

    }

    void Update()
    {
        // Move
        transform.position += velocity * Time.deltaTime;

        //gravity
        if (gravityDownAcceleration > 0)
        {
            // add gravity to the projectile velocity for ballistic effect
            velocity += Vector3.down * gravityDownAcceleration * Time.deltaTime;
        }

        //Physics
        {
            RaycastHit closestHit = new RaycastHit();
            closestHit.distance = Mathf.Infinity;
            bool foundHit = false;

            // Sphere cast
            Vector3 displacementSinceLastFrame = tip.position - lastRootPosition;
            RaycastHit[] hits = Physics.SphereCastAll(lastRootPosition, radius, displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude);
            foreach (var hit in hits)
            {
                if (hit.distance < closestHit.distance)
                {
                    foundHit = true;
                    closestHit = hit;
                }
            }

            if (foundHit)
            {
                // Handle case of casting while already inside a collider
                if (closestHit.distance <= 0f)
                {
                    closestHit.point = root.position;
                    closestHit.normal = -transform.forward;
                }

                OnHit(closestHit.point, closestHit.normal, closestHit.collider);
            }
        }

        lastRootPosition = root.position;
    }
    void OnHit(Vector3 point, Vector3 normal, Collider collider)
    {
        GameObject impactVFXInstance = Instantiate(impactVFX, point, Quaternion.LookRotation(normal));
        if (impactVFXLifetime > 0)
        {
            Destroy(impactVFXInstance.gameObject, impactVFXLifetime);
        }

        Destroy(this.gameObject);
    }
}
