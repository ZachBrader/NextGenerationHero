using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehavior : MonoBehaviour
{
    // Holds the number of times hit
    private int timeHit;

    // Holds inital x-position
    private float initX;
    
    // Holds initial y-position
    private float initY;

    // Determines whether object is visible
    private bool visibility = true;

    // Reference to game manage
    private GlobalBehavior globalBehavior;

    // Start is called before the first frame update
    void Start()
    {
        initX = transform.position.x;
        initY = transform.position.y;
        timeHit = 0;
        globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check to see if visibility status needs to be updated
        if (globalBehavior.getWaypointVisibilityStatus() != visibility)
        {
            if (visibility) // Turn invisible
            {
                visibility = false;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
            }
            else // Show updated waypoint
            {
                visibility = true;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (1f - timeHit * .25f));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // If an egg hits the waypoint
        if (col.gameObject.tag == "Egg")
        {
            // If the waypoint has been hit 4 times
            if (timeHit >= 3)
            {
                // Reset the count down and move the object
                timeHit = 0;
                float newX = initX + Random.Range(0f, 15f);
                float newY = initY + Random.Range(0f, 15f);
                transform.position = new Vector3(newX, newY, transform.position.z);
            }
            else
            { timeHit += 1; } // Record the hit
            if (visibility) // Update object's transparency
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (1f - timeHit * .25f));
            }
            else // Turn invisible
            {
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
            }
        }
    }
}