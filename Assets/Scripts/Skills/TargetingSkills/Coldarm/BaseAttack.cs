using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAttack : TargetingSkill
{
    //protected override void Prepare()
    
    public override IEnumerator Active(CharacterController contr)
    {
        //contr.GetComponent<Collider2D>().isTrigger = true;
        contr.health -= Damage;
        controller.animator.SetBool("attack", true);
        yield return new WaitForSeconds(1);
        controller.animator.SetBool("attack", false);
        //Debug.Log("Attack has done");
    }

    // Start is called before the first frame update
    void Start()
    {
        Name = "BaseAttack";
        Damage = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}