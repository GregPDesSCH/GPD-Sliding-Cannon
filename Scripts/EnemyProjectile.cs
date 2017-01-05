using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

    // Speed
    public float speed;

	// Use this for initialization
	void Start ()
    {
        gameObject.SetActive(false);
    }
	
	void FixedUpdate ()
    {
        // Move the projectile straight down
        if (!GameManager.gameManager.NoMoreEnemies() && !GameManager.gameManager.playerCannon.isDestroyed())
            transform.Translate(-Vector3.up * Time.deltaTime * speed);
        else
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannon"))
        {
            gameObject.SetActive(false);
            GameManager.gameManager.playerCannon.deductHitpoints(1);
            GameManager.gameManager.UpdateHealthBar();
        }
        else if (other.CompareTag("Board Limit"))
            gameObject.SetActive(false);
    }
}
