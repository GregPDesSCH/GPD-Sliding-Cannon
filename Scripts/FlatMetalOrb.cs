using UnityEngine;
using System.Collections;

public class FlatMetalOrb : MonoBehaviour {

    // Score from destroying the orb
    public int score = 50;
	
    // When the orb gets disabled by the shot, add the score to the player.
    void OnDisable()
    {
        GameManager.gameManager.addScore(score);
    }
}
