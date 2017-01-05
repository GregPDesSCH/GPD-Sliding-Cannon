using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    // Speed Factor
    public float speed = 2.5f;

	// Use this for initialization
	void Start ()
    {
        gameObject.SetActive(false);

        // Destroy after 2.5 seconds.
        //Destroy(gameObject, 2.5f);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // Move the projectile in a direction based on the cannon's direction.
        if (!GameManager.gameManager.NoMoreEnemies() && !GameManager.gameManager.playerCannon.isDestroyed())
            transform.Translate(-Vector3.up * Time.deltaTime * speed);
        else
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // The projectile has collided with an orb.
        if (other.CompareTag("Target Orb"))
        {
            other.gameObject.SetActive(false);
            GameManager.gameManager.UpdateScoreText();
            GameManager.gameManager.DeductEnemy();
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Board Limit"))
        {
            gameObject.SetActive(false);
        }
    }
}
