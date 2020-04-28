using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehavior : MonoBehaviour
{
    private GameObject[] eggs;
    private int timeHit;
    private float initX;
    private float initY;

    // Start is called before the first frame update
    void Start()
    {
        initX = transform.position.x;
        initY = transform.position.y;
        timeHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        eggs = GameObject.FindGameObjectsWithTag("Egg");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Egg")
        {
            if (timeHit >= 3)
            {
                timeHit = 0;
                float newX = initX + Random.Range(0f, 15f);
                float newY = initY + Random.Range(0f, 15f);
                transform.position = new Vector3(newX, newY, transform.position.z);
            }
            else
            { timeHit += 1; }
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (1f - timeHit * .25f));
        }
    }
}