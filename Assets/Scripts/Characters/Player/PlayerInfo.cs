using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : CharacterInfo
{
    protected string[] skill_names;
    public int amount_of_skills;

    // Start is called before the first frame update
    void Start()
    {
        skill_names = new string[] { "BaseAttack", "BigJump" };
        amount_of_skills = skill_names.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
