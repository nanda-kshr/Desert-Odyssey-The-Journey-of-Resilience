using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class npcMovement : MonoBehaviour
{
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>();
    public int current = 0;
    public float speed = 4f;
    public float WPradius = 1;
    private void Start(){
       GetRandom();
        
    }
    void Update()
    {
        StartCoroutine( MoveBetweenWaypoints());
    }
    private void GetRandom()
    {
        for (int i = 0; i < 30; i++)
        {
            waypoints.Add(new Vector3(Random.Range(transform.position.x, transform.position.x + 3f),
                Random.Range(transform.position.y, transform.position.y + 3f),
                transform.position.z));
        }
    }
    IEnumerator MoveBetweenWaypoints()
    {
        yield return new WaitForSeconds(2f);
        if (Vector3.Distance(waypoints[current], transform.position) < WPradius)
        {
            current = Random.Range(0, waypoints.Count);
            if (current >= waypoints.Count)
            {
                current = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[current], Time.deltaTime * speed);
        yield return new WaitForSeconds(1f);
        
    }
}
