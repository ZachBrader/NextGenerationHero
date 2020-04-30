using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	
    // Average speed of enemy class
	public float mSpeed = 20f;

    // Reference to waypoints
    private GameObject[] waypoints = null;

    // Current destination enemy is heading too
    private int curDestination;

    // Used to lock enemy when destroyed so that it can only create
    // a single new enemy
    private bool isDestroyed = false;

    // Reference to Game Manager
    private GlobalBehavior globalBehavior;

    // Use this for initialization
    void Start () {
        // Find all the waypoints in the game
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        // Make a reference to game manager
        globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();

        // Point the enemy towards an objective
        curDestination = 0;

    }
	
	// Update is called once per frame
	void Update () {
        // Get the position of the target waypoint
        Vector2 target = waypoints[curDestination].transform.position;

        // Get the direction the enemy needs to point
        Vector3 targetDirection = waypoints[curDestination].transform.position - transform.position;

        // Move towards the waypoint
        transform.position = Vector2.MoveTowards(transform.position, target, mSpeed * Time.smoothDeltaTime);

        // Rotate towards the waypoint
        transform.up = Vector3.RotateTowards(transform.up, targetDirection, mSpeed * Time.smoothDeltaTime, 0.0f);

        // Check if enemy hit world bounds
		GlobalBehavior.WorldBoundStatus status =
			globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
			
		if (status != GlobalBehavior.WorldBoundStatus.Inside) {
			Debug.Log("collided position: " + this.transform.position);
			NewDirection();
		}	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        // Only trigger once object has waypoints initialized
        if (waypoints == null)
        {
            return;
        }

        // If enemy collides with waypoint
        if (col.gameObject.tag == "Waypoint")
        {
            if (col.gameObject.name == waypoints[curDestination].name)
            {
                NewDirection();
            }
        }

        // If enemy collides with a player or an egg
        if (col.gameObject.tag == "Egg" || col.gameObject.tag == "Player")
        {
            if (!isDestroyed)
            {
                isDestroyed = true;

                // Record enemy defeat
                globalBehavior.UpdateEnemyDefeatedState();

                // Record when player object collides with enemy seperately
                if (col.gameObject.tag == "Player")
                {
                    globalBehavior.UpdateEnemyTouchedState();
                }

                // Create a single new enemy
                globalBehavior.CreateEnemy();

                // Remove this object from the game
                Destroy(gameObject);
            }
        }
    }

    // New direction will be something completely random!
    private void NewDirection() {
        // Ensure waypoints are within bounds of number of waypoints
        if (curDestination < waypoints.Length - 1)
        {
            // Determine if waypoints need to be sequential or random
            if (globalBehavior.getSequentialStatus())
            {
                curDestination += 1;
            }
            else
            {
                int randomNum; // Holds random number
                do
                {
                    // Selects a random waypoint
                    randomNum = Random.Range(0, waypoints.Length);
                } while (randomNum == curDestination); // Make sure we select a new location

                // Assign random destination
                curDestination = randomNum;
            }
        }
        else
        {
            // Default to first waypoint
            curDestination = 0;
        }
	}
}
