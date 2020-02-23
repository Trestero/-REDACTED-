using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Person[] people;
    public Person[] People { get; set; }
    
    //quick n dirty put this on corner outside the camera's view and use position for where to spawn people
    [SerializeField] GameObject ExtentsMarker;
    [SerializeField] GameObject PersonPrefab;

    //[SerializeField] GameObject yExtentsMarker;

    float xExtents;
    float yExtents;

    // Start is called before the first frame update
    void Start()
    {
        xExtents = ExtentsMarker.transform.position.x;
        yExtents = ExtentsMarker.transform.position.y;
        //float vertExtent = Camera.main.orthographicSize;
        //float horzExtent = vertExtent * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        
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

        Instantiate(PersonPrefab, new Vector2(xSpawn, ySpawn), Quaternion.identity);
    }
}
