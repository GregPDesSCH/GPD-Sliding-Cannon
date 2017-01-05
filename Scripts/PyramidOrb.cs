using UnityEngine;
using System.Collections;

public class PyramidOrb : MonoBehaviour {

    // Score from destroying the orb
    public int score = 100;

    // Rotation Step (what to rotate by each frame)
    private Vector3 rotationStep;
    public float rotationSpeedFactor = 1f;
    public const float rawSpeed = 3600.0f;

    // Enemy Projectile
    public GameObject enemyProjectile;
    public GameObject enemyProjectileSpawnPoint;
    public const float enemyProjectileCooldownPeriod = 0.5f;
    private float timeSinceEnemyProjectileFired = 0.0f;
    Vector3 relativePositionToCannon;

    // Object Pool
    private readonly GameObject[] enemyProjectilePool = new GameObject[15];

    // Use this for initialization
    void Start ()
    {
	    for (int i = 0; i < enemyProjectilePool.Length; i++)
            enemyProjectilePool[i] = Instantiate(enemyProjectile, Vector3.zero, Quaternion.identity) as GameObject;
	}
	
    // Rotate the orb at a fixed rate.
    void FixedUpdate()
    {
        rotationStep = new Vector3(0.0f, 0.0f, rawSpeed * rotationSpeedFactor * Time.deltaTime);
        transform.rotation *= Quaternion.Euler(rotationStep * Time.deltaTime);

        if (Time.time - timeSinceEnemyProjectileFired > (enemyProjectileCooldownPeriod + Random.Range(0.0f, 0.5f)) &&
            !GameManager.gameManager.NoMoreEnemies() && !GameManager.gameManager.playerCannon.isDestroyed())
        {
            for (int i = 0; i < enemyProjectilePool.Length; i++)
                if (!enemyProjectilePool[i].activeInHierarchy)
                {
                    enemyProjectilePool[i].transform.position = enemyProjectileSpawnPoint.transform.position;

                    //relativePositionToCannon = GameManager.gameManager.playerCannon.transform.position - transform.position;

                    enemyProjectilePool[i].transform.rotation = Quaternion.identity;
                    enemyProjectilePool[i].SetActive(true);
                    i = enemyProjectilePool.Length;
                }

            timeSinceEnemyProjectileFired = Time.time;
        }

    }

    // When the orb gets disabled by the shot, add the score to the player.
    void OnDisable()
    {
        GameManager.gameManager.addScore(score);
    }
}
