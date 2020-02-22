using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerState //person state
{
    Default, //starting state of person. person will behave according to their specific type (kid, police, ice cream guy, etc).
    Attracted, //attracted to something, either player seducing them or at a world detail (ice cream truck, dance party, etc). will move towards source of attraction.
    Afraid, //scared of something, either player petitioning or at world detail (scary picture, loud noise, etc). will move away from source of scary.
    Suspicious //has started to see alien. timer begins and a visual indicator ("!?") appear above them. if timer runs out before distracted, GAME OVER.
}

public class Person : MonoBehaviour
{
    //configuration attributes: these attributes are meant to be fiddled with to make the person act differently, such as moving faster or spotting the alien quicker.
    [SerializeField] private float speedDefault = 1.0f;
    //[SerializeField] private float speedDefault = 1.0f;

    //state variables: these variables are 
    private PerState state = PerState.Default; //state of the person. see enum declaration for more.
    private Vector2 targetDefault = new Vector2(0, 0); //default target that the person will seek towards unless distracted, either by player, world detail, or alien.
    private Vector2 targetAttract = new Vector2(0, 0); //target which is 

    //bookkeeping attributes: holder attributes and such. used mostly for convience.

    // Start is called before the first frame update
    void Start()
    {
        Vector2 test = new Vector2(10, 1);
        Debug.Log(test.normalized);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //property for state
    public PerState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }
}
