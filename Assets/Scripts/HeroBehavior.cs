using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroBehavior : MonoBehaviour {

    public EggStatSystem mEggStat = null;

    // Player Speeds
    public float mHeroSpeed = 20f;
    public float kHeroRotateSpeed = 90f/2f; // 90-degrees in 2 seconds
                                            // Use this for initialization

    // Boolean to determine if object should follow player's mouse
    private bool followUserMouse;

    // Reference to Game Manager
    private GlobalBehavior globalBehavior;


    void Start () {
        // Initialize object to not follow mouse
        followUserMouse = false;
        Debug.Assert(mEggStat != null);
        globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
    }
	
	// Update is called once per frame
	void Update () {

        // Have mouse follow the user if 'M' button pressed
        if (Input.GetKeyUp(KeyCode.M))
        {
            followUserMouse = true;
        }

        // Toggle flights if 'J' button pressed
        if (Input.GetKeyUp(KeyCode.J))
        {
            globalBehavior.toggleSequential();
        }

        //Toggle waypoint visibility if 'H' button pressed
        if (Input.GetKeyUp(KeyCode.H))
        {
            globalBehavior.toggleWaypointVisibility();
        }

        UpdateMotion();
        BoundPosition();
        ProcessEggSpwan();
    }

    private void UpdateMotion()
    {
        // Determine how mouse should move
        if (!followUserMouse)
        {
            mHeroSpeed += Input.GetAxis("Vertical");
            transform.position += transform.up * (mHeroSpeed * Time.smoothDeltaTime);
        }
        else
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }

        // Handles player rotation
        transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") *
                                        (kHeroRotateSpeed * Time.smoothDeltaTime));
    }

    private void BoundPosition()
    {
        GlobalBehavior.WorldBoundStatus status = GlobalBehavior.sTheGlobalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
        switch (status)
        {
            case GlobalBehavior.WorldBoundStatus.CollideBottom:
            case GlobalBehavior.WorldBoundStatus.CollideTop:
                transform.up = new Vector3(transform.up.x, -transform.up.y, 0.0f);
                break;
            case GlobalBehavior.WorldBoundStatus.CollideLeft:
            case GlobalBehavior.WorldBoundStatus.CollideRight:
                transform.up = new Vector3(-transform.up.x, transform.up.y, 0.0f);
                break;
        }
    }

    private void ProcessEggSpwan()
    {
        if (mEggStat.CanSpawn()) {
            if (Input.GetKey("space"))
            {
                mEggStat.SpawnAnEgg(transform.position, transform.up);
            }
        }
    }
}
