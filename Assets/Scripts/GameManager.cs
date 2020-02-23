using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Person[] people;
    public List<GameObject> People { get; set; }
    
    //quick n dirty put this on corner outside the camera's view and use position for where to spawn people
    [SerializeField] GameObject ExtentsMarker;
    [SerializeField] GameObject PersonPrefab;

    //[SerializeField] GameObject yExtentsMarker;

    float xExtents;
    float yExtents;

    float timer = 0.0f;
    float timeDelay = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        People = new List<GameObject>();
        xExtents = ExtentsMarker.transform.position.x;
        yExtents = ExtentsMarker.transform.position.y;
        //float vertExtent = Camera.main.orthographicSize;
        //float horzExtent = vertExtent * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0)
        {
            SpawnPerson();
            timer = timeDelay;
        }

        timer -= Time.deltaTime;
    }

    void SpawnPerson()
    {
        float ySpawn = yExtents;
        float xSpawn = xExtents;
        
        if(Random.value > 0.5f) //sides
        {
            ySpawn = Random.Range(yExtents, -yExtents);

            if (Random.value > 0.5f) //left
            {
                xSpawn = -xExtents;
            }
            else if(Random.value < 0.5f) //right
            {
                xSpawn = xExtents;
            }
        }
        else if(Random.value < 0.5f) //top or bottom
        {
            xSpawn = Random.Range(xExtents, -xExtents);

            if (Random.value > 0.5f) //left
            {
                ySpawn = -yExtents;
            }
            else if (Random.value < 0.5f) //right
            {
                ySpawn = yExtents;
            }
        }

        People.Add(Instantiate(PersonPrefab, new Vector2(xSpawn, ySpawn), Quaternion.identity));
    }
}
