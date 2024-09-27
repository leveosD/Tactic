using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;

//using Pathfinding;

public class Skill : MonoBehaviour
{
    int cost;
    public int Cost
    {
        get { return cost; }
    }

    //const string imageAddr = "Assets/Sprites/UI/Skills/;

    string skillname;
    public string Name
    {
        get { return name; }
        protected set { skillname = value; }
    }

    int damage;
    public int Damage
    {
        get { return damage; }
        protected set { damage = value; }
    }

    protected CharacterController controller;

    /*(int, int) distance;
    public (int, int) Distance
    {
        get { return distance; }
    }

    string[] targetTypes;
    public string[] TargetTypes
    {
        get { return targetTypes; }
        protected set { targetTypes = value; }
    }
    Color color;
    public Color ColorP
    {
        get { return color; }
    }*/

    protected List<GameObject> targets;
    //Seeker seeker;

    //Vector3 checkDirection;

    public virtual void Prepare()
    {
        controller.CurrentSkill = this;
    }

    protected virtual IEnumerator ExtraAction()
    {
        yield return null;
    }

    public virtual IEnumerator Active(CharacterController contr)
    {
        yield return null;
    }

    public virtual IEnumerator DoSkill(Vector3 t)
    {
        yield return null;
    }

    public Skill SetController(CharacterController c)
    {
        controller = c;
        return this;
    }
}
