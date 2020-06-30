using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarComponent : MonoBehaviour
{
    Unit parent;

    public GameObject turretObject;
    public GameObject bulletPrefab;

    private Collider collider;

    public float maxProjectileVelocity = 50;

    private float cooldownRemaining = 0;

    void Awake()
    {
        RoundManager.OnRoundTick += UnitUpdate;
    }

    private void Start()
    {
        parent = GetComponent<Unit>();
        collider = GetComponent<Collider>();
        cooldownRemaining = parent.reloadTime;
    }

    private SortedDictionary<float, Unit> nearbyUnits = new SortedDictionary<float, Unit>();

    void UnitUpdate()
    {
        Unit target = null;
        Vector3 projVelocity = Vector3.zero;
        if (UnitManager.INSTANCE.findUnitsNear(ref nearbyUnits, transform.position, parent.weaponRange*10))
        {
            
            foreach (Unit u in nearbyUnits.Values)
            {
                if (u != parent && u.team != parent.team)
                {
                    target = u;
                    break;//this is a really hacky way of getting the first element. I need to fix this later.
                }
            }
        }
        
        if (target != null)
        {
            projVelocity = minimumTrajectory(turretObject.transform.position, target.transform.position);
            if (!isNaN(projVelocity))
                turretObject.transform.LookAt(turretObject.transform.position + projVelocity.normalized, Vector3.up);
            else
                turretObject.transform.localEulerAngles = Vector3.right * -90;
        }
        else
        {
            turretObject.transform.localEulerAngles = Vector3.right * -90;
        }

        if (cooldownRemaining < 0 && target != null)
        {
            cooldownRemaining = parent.reloadTime;

            GameObject projectile = Instantiate(bulletPrefab, turretObject.transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = projVelocity;
            projectile.GetComponent<Explosive>().damage = parent.projectileDmg;
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), collider);
            Destroy(projectile, 30);
        }

        cooldownRemaining -= RoundManager.INSTANCE.TICK_LENGTH;
    }

    //checks if any value is NaN
    bool isNaN(Vector3 vect)
    {
        return float.IsNaN(vect.x) || float.IsNaN(vect.y) || float.IsNaN(vect.z);
    }

    Vector3 maximumTrajectory(Vector3 v1, Vector3 v2, Vector3 targetVelocity)
    {
        float grav = 9.8f;
        float projVelo = 100.0f;
        Vector3 diff = v2 - v1;
        float h = -diff.y;
        diff.y = 0;
        float d = diff.magnitude;
        float a = (grav * Mathf.Pow(d, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta = angleOfFire(h, d, a);
        float time = d / (projVelo * Mathf.Cos(theta));
        Vector3 v3 = v2 + (targetVelocity * time);
        Vector3 diff1 = v3 - v1;
        float h1 = -diff1.y;
        diff1.y = 0;
        float d1 = diff1.magnitude;
        float a1 = (grav * Mathf.Pow(d1, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta1 = angleOfFire(h1, d1, a1);
        diff1 = diff1.normalized;
        diff1 = diff1 * projVelo * Mathf.Cos(theta1);
        diff1.y = projVelo * Mathf.Sin(theta1);
        return diff1;
    }

    Vector3 maximumTrajectory(Vector3 v1, Vector3 v2)
    {
        return maximumTrajectory(v1, v2, Vector3.zero);
    }

    float angleOfFire(float heightdiff, float distance, float aggregateVal)
    {
        return (Mathf.Acos(((2 * aggregateVal) - heightdiff) / Mathf.Sqrt(Mathf.Pow(heightdiff, 2) + Mathf.Pow(distance, 2))) + Mathf.Atan2(distance, heightdiff)) / 2;
    }

    Vector3 minimumTrajectory(Vector3 v1, Vector3 v2, Vector3 targetVelocity)
    {
        float grav = 9.8f;
        float projVelo = maxProjectileVelocity;
        Vector3 diff = v2 + (targetVelocity * ((v2 - v1).magnitude / projVelo)) - v1;
        float h = -diff.y;
        diff.y = 0;
        float d = diff.magnitude;
        float a = (grav * Mathf.Pow(d, 2)) / (2 * Mathf.Pow(projVelo, 2));
        float theta = angleOfFire(h, d, a);
        theta = (Mathf.PI / 2) - theta - Mathf.Sin(h / d);
        diff = diff.normalized;
        diff = diff * projVelo * Mathf.Cos(theta);
        diff.y = projVelo * Mathf.Sin(theta);
        return diff;
    }

    Vector3 minimumTrajectory(Vector3 v1, Vector3 v2)
    {
        return minimumTrajectory(v1, v2, Vector3.zero);
    }

    private void OnDestroy()
    {
        RoundManager.OnRoundTick -= UnitUpdate;
    }

}
