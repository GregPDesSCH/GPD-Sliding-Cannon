using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour
{
    // Singleton Instance
    public static ApplicationManager applicationManager;

	// Use this for initialization
	void Start ()
    {
        if (applicationManager == null)
            applicationManager = this;
        else
            Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Closes the application when the user presses the Back button.
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
