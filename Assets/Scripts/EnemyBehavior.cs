using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	
	public float mSpeed = 20f;

    private GameObject[] waypoints = null;
    private int curDestination;

    // Use this for initialization
    void Start () {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        Debug.Log("Size is: " + waypoints.Length);
		NewDirection();
        curDestination = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 target = waypoints[curDestination].transform.position;
        Vector3 targetDirection = waypoints[curDestination].transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, target, mSpeed * Time.smoothDeltaTime);
        transform.up = Vector3.RotateTowards(transform.up, targetDirection, mSpeed * Time.smoothDeltaTime, 0.0f);
        GlobalBehavior globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior>();
		
		GlobalBehavior.WorldBoundStatus status =
			globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
			
		if (status != GlobalBehavior.WorldBoundStatus.Inside) {
			Debug.Log("collided position: " + this.transform.position);
			NewDirection();
		}	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (waypoints == null)
        {
            return;
        }

        if (col.gameObject.tag == "Waypoint")
        {
            if (col.gameObject.name == waypoints[curDestination].name)
            {
                Debug.Log("Choosing next waypoint");
                NewDirection();
            }
        }

        if (col.gameObject.tag == "Egg" || col.gameObject.tag == "Player")
        {
            GlobalBehavior.sTheGlobalBehavior.CreateEnemy();
            Destroy(gameObject);
        }
    }

    // New direction will be something completely random!
    private void NewDirection() {
        if (curDestination < waypoints.Length - 1)
        {
            Debug.Log("Updating curdestination to " + curDestination);
            curDestination += 1;
            Debug.Log(waypoints[curDestination].name);
        }
        else
        {
            curDestination = 0;
        }
	}
}
