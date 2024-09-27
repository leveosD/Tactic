using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSkill : Skill
{
    protected GameObject target;

    protected (int, int) distance;
    public (int, int) Distance
    {
        get { return distance; }
    }

    public override void Prepare()
    {
        base.Prepare();
    }

    /*public virtual IEnumerator Active(CharacterController contr)
    {
        yield return null;
    }*/

    public override IEnumerator DoSkill(Vector3 t)
    {
        controller.busy = true;
        target = Physics2D.OverlapPoint(t).gameObject;
        if (Math.Abs(target.transform.position.y - controller.transform.position.y) >= 0.5f || Math.Abs(target.transform.position.x - controller.transform.position.x) >= 1.1f)
        {
            yield return StartCoroutine(controller.Move((Vector2)target.transform.position));
        }
        yield return StartCoroutine(Active(target.GetComponent<CharacterController>()));
        controller.busy = false;
    }
}
