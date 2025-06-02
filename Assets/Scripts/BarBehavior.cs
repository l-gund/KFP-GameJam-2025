using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarBehavior : MonoBehaviour
{
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float speed = 5f;

    public GameObject[] chickenApperances;
    public GameTimer station2Timer;

    private bool movingToEnd = true;

    void Start()
    {
        startPosition = transform.position;
        station2Timer.StartPrepTime(10f);
    }

    // Start is called before the first frame update
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            Vector2 target = movingToEnd ? endPosition : startPosition;

            // Move toward the target position
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Check if the object has reached the target
            if (Vector2.Distance(transform.position, target) < 0.01f)
            {
                movingToEnd = !movingToEnd; // Reverse direction
            }
        }
        else
        {
            //transform.position = startPosition;
            speed = Random.Range(5, 15);
            // Check for trigger colliders at the current position
            Collider2D[] overlapping = Physics2D.OverlapPointAll(transform.position);

            foreach (var col in overlapping)
            {
                if (col.gameObject != this.gameObject)
                {
                    Debug.Log("Stopped inside: " + col.name);
                    UpdateChicken(col.name);
                }
            }
        }
    }

    void UpdateChicken(string hitRate)
    {
        int index = hitRate switch
        {
            "Perfect" => 0, "Okay" => 1, "Bad" => 2, _ => -1
        };

        for (int i = 0; i < chickenApperances.Length; i++)
        {
            chickenApperances[i].SetActive(i == index);

        }

        station2Timer.RecordHit(hitRate);
    }
}
