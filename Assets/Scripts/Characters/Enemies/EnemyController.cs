using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : CharacterController
{
    GameObject player;
    List<Skill> skillist;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        skillist = new List<Skill>(GetComponents<Skill>());
        foreach(Skill skill in skillist)
        {
            skill.SetController(this);
        }
        CurrentSkill = skillist[0];
        points = 3;
        steps_per_action = 5;
        steps = points * steps_per_action;
        player = GameObject.Find("Player");
    }

    public override void StartOfTurn()
    {
        base.StartOfTurn();
        StartCoroutine(Move((Vector2)player.transform.position));
        //StartCoroutine(TurnLogic());
    }

    /*protected IEnumerator TurnLogic()
    {
        yield return StartCoroutine(this.CurrentSkill.DoSkill(player));
    }*/
}
