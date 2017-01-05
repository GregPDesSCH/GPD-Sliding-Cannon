using UnityEngine;
using System.Collections;

public class CubeOrb : MonoBehaviour
{

    // Score from destroying the orb
    public int score = 60;

    // Rotation Step (what to rotate by each frame)
    private Vector3 rotationStep;

	// Use this for initialization
	void Start ()
    {
        rotationStep = new Vector3(45.0f * Random.value, 45.0f * Random.value, 45.0f * Random.value);
        transform.rotation = Quaternion.Euler(180.0f * Random.value, 180.0f * Random.value, 180.0f * Random.value);
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(rotationStep * Time.deltaTime);
    }

    void OnDisable()
    {
        GameManager.gameManager.addScore(score);
    }
}
