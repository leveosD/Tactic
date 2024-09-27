using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public int health;
    protected int actions;
    protected int steps_per_action;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        actions = 2;
        steps_per_action = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
