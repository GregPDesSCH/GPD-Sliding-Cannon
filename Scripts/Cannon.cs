using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    // Singleton Instance
    public static Cannon cannon;


    // Projectile Prefab and Shooting Spot
    public GameObject projectile;
    public GameObject projectileShootingSpot;

    // Barrel Pivot
    public GameObject barrelPivot;

    // Movement Vector
    private Vector3 movementVector;
    private Vector3 rotationVector;
    private Vector3 currentRotation;

    // Cooldown Timing
    private float timeSinceProjectileFired;

    // Tweak Variables
    public float initialBarrelAngle = 0.0f;
    public float slideTiltSensitivityFactor = 0.15f;
    public float referenceAngle = 0.0f;
    public float currentBarrelPivotAngle = 0.0f;

    // Limits
    public const float maxDisplacement = 8.0f;
    public const float maxAngle = 60.0f;
    public const float projectileCooldownPeriod = 0.25f;


    // Hitpoints
    private int numberOfHitpoints = 10;
    private const int maxNumberOfHitpoints = 10;


    // Pool of Projectiles (Object Pooling)
    [HideInInspector]
    public readonly GameObject[] projectilePool = new GameObject[20];

	void Start ()
    {
        // Singleton Assignment
        if (cannon == null)
            cannon = this;
        else
            Destroy(gameObject);


        // Initialize components
        Input.gyro.enabled = true;
        movementVector = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < projectilePool.Length; i++)
            projectilePool[i] = Instantiate(projectile, Vector3.zero, Quaternion.identity) as GameObject;

        timeSinceProjectileFired = -projectileCooldownPeriod;
	}
	

    void FixedUpdate()
    {

        if (!isDestroyed() && !GameManager.gameManager.NoMoreEnemies())
        {
            // Get movement from the player's phone and set the component's position.
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + Input.gyro.rotationRate.y * slideTiltSensitivityFactor,
                -maxDisplacement, maxDisplacement),
                transform.position.y, transform.position.z);

            // Get rotation from the player's phone
            // This is where I ran into some trouble.
            // Input.gyro.rotationRateUnbiased.y
            // Input.gyro.attitude.
            // (initialOrientationZ - Input.gyro.rotationRate.z)
            // Input.acceleration.x
            // -Input.gyro.attitude.eulerAngles.z
            // 

            // Get rotation from the player's phone and set it to the cannon's barrel.
            //currentRotation = barrelPivot.transform.eulerAngles;
            //currentBarrelPivotAngle = currentRotation.z;

            rotationVector = new Vector3(180.0f, 0.0f, Mathf.Clamp(initialBarrelAngle - Input.gyro.rotationRateUnbiased.z,
                -maxAngle, maxAngle));

            initialBarrelAngle = Mathf.Clamp(initialBarrelAngle - Input.gyro.rotationRateUnbiased.z, -60.0f, 60.0f);

            referenceAngle = referenceAngle - Input.gyro.rotationRateUnbiased.z;

            barrelPivot.transform.rotation = Quaternion.Euler(rotationVector);




            // Tapping on the phone fires the projectiles.
            if (Input.touchCount >= 1 && Time.time - timeSinceProjectileFired > projectileCooldownPeriod)
            {
                for (int i = 0; i < projectilePool.Length; i++)
                    if (!projectilePool[i].activeInHierarchy)
                    {
                        projectilePool[i].transform.position = projectileShootingSpot.transform.position;
                        projectilePool[i].transform.rotation = barrelPivot.transform.rotation;
                        projectilePool[i].SetActive(true);
                        i = projectilePool.Length;
                    }


                timeSinceProjectileFired = Time.time;
            }
        }

    }

    public void deductHitpoints(int damage)
    {
        if (numberOfHitpoints - damage > 0)
            numberOfHitpoints -= damage;
        else
        {
            numberOfHitpoints = 0;
            gameObject.SetActive(false);
            GameManager.gameManager.DisableGame();
        }
    }

    public void addingBonusPoints()
    {
        numberOfHitpoints--;
    }

    public int numberOfHitpointsLeft()
    {
        return numberOfHitpoints;
    }

    public int maximumNumberOfHitpoints()
    {
        return maxNumberOfHitpoints;
    }

    public bool isDestroyed()
    {
        return numberOfHitpoints == 0;
    }
}
