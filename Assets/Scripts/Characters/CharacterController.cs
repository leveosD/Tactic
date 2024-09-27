using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class CharacterController : MonoBehaviour
{
    public AILerp ai;
    public Seeker seeker;
    public Animator animator;
    protected Rigidbody2D rb;
    protected BoxCollider2D col;

    public delegate void Actions(GameObject g);
    public Actions actions;
    protected int points;
    protected int steps_per_action;
    protected int steps;
    protected int triggerCount;
    public int triggers;

    public bool busy;

    public int health = 100;
    
    bool itsTurn;
    public bool ItsTurn
    {
        get { return itsTurn; }
        private set { itsTurn = value; }
    }
    protected MovementBehaviour movement;
    string opponentTag;
    protected Vector2 target;

    public List<Weapon> weapons;
    protected Weapon currentWeapon;
    public Weapon CurrentWeapon
    {
        get
        {
            return currentWeapon;
        }
        set 
        {
            currentWeapon = value;
        }
    }
    
    protected List<GameObject> skills;
    Skill currentSkill;
    public Skill CurrentSkill
    {
        get
        {
            return currentSkill;
        }
        set { currentSkill = value; }
    }

    IEnumerator OnTriggerEnter2D(Collider2D trigger)
    {
        //Debug.Log("From: " + this.gameObject.name + " To: " + trigger.gameObject.name);
        /*if (ItsTurn && trigger.gameObject.tag == opponentTag)
        {
            if(target == (Vector2)trigger.transform.position)
            {
                movement.keepMoving = false;
            }
            if (trigger.isTrigger && target != (Vector2)trigger.transform.position)
            {
                trigger.isTrigger = false;
                movement.keepMoving = false;
                //StartCoroutine(trigger.gameObject.GetComponent<CharacterController>().currentSkill.Active(this.gameObject));
                StartCoroutine(trigger.gameObject.GetComponent<CharacterController>().currentSkill.Active(this.gameObject));

            }
        }*/
        
        if(!ItsTurn && trigger.gameObject.tag == opponentTag)
        {
            CharacterController opp = trigger.gameObject.GetComponent<CharacterController>();
            if (triggers > 0 && opp.target != (Vector2)this.transform.position)
            {
                //opp.busy = true;
                triggers--;
                opp.movement.keepMoving = false;
                yield return StartCoroutine(currentSkill.Active(opp));
                opp.busy = false;
                //Messenger.Broadcast(CharacterEvent.STAND);
            }
            else if (opp.target == (Vector2)this.transform.position)
            {
                opp.movement.keepMoving = false;
            }
            yield return null;
        }
        //Debug.Log(trigger.gameObject.name + " " + ItsTurn + " " + (trigger.gameObject.tag == opponentTag) + " " + col.isTrigger + " " + (target != (Vector2)trigger.transform.position));
    }

    /*void OnTriggerStay2D(Collider2D trigger)
    {
        /*if(ItsTurn && trigger.gameObject.tag == opponentTag)
        {
            if (target == (Vector2)trigger.transform.position)
            {
                movement.keepMoving = false;
            }
            if (movement.keepMoving && trigger.isTrigger && target != (Vector2)trigger.transform.position)
            {
                //Debug.Log("From: " + this.gameObject.name + " " + movement.keepMoving + " " + trigger.isTrigger + " " + (target != (Vector2)trigger.transform.position));
                trigger.isTrigger = false;
                movement.keepMoving = false;
                StartCoroutine(trigger.gameObject.GetComponent<CharacterController>().currentSkill.Active(this.gameObject));
            }
        }
        if (!ItsTurn && trigger.gameObject.tag == opponentTag)
        {
            CharacterController opp = trigger.gameObject.GetComponent<CharacterController>();
            if (triggers > 0 && opp.target != (Vector2)this.transform.position && opp.movement.keepMoving)
            {
                triggers--;
                StartCoroutine(currentSkill.Active(trigger.gameObject));
                opp.movement.keepMoving = false;
            }
            else if (opp.target == (Vector2)this.transform.position)
                opp.movement.keepMoving = false;
        }
    }*/

    protected void GetMesseges()
    {
        //Messenger<Vector3>.AddListener(CharacterEvent.MOVE, Move);
        //Messenger.AddListener(CharacterEvent.STAND, Stand);
        Messenger.AddListener(TurnEvent.END, EndOfTurn);
    }

    protected void IgnoreMesseges()
    {
        //Messenger<Vector3>.RemoveListener(CharacterEvent.MOVE, Move);
        //Messenger.RemoveListener(CharacterEvent.STAND, Stand);
        Messenger.RemoveListener(TurnEvent.END, EndOfTurn);
    }

    public virtual void StartOfTurn()
    {
        col.edgeRadius = 0f;
        col.isTrigger = false;
        points = 3;
        steps_per_action = 5;
        steps = points * steps_per_action;
        ItsTurn = true;
        GetMesseges();
    }

    public void EndOfTurn()
    {
        col.edgeRadius = 0.4f;
        col.isTrigger = true;
        ItsTurn = false;
        triggers = triggerCount;
        IgnoreMesseges();
    }

    public IEnumerator Move(Vector2 v)
    {
        busy = true;
        bool enemies = false;
        Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)this.transform.position, new Vector2(2, 1), 0);
        foreach(Collider2D col in cols)
        {
            CharacterController opp = col.gameObject.GetComponent<CharacterController>();
            if (col.tag == opponentTag && opp.triggers > 0)
            {
                opp.triggers--;
                enemies = true;
                yield return StartCoroutine(opp.CurrentSkill.Active(this));
                busy = false;
            }
        }
        if (ItsTurn && !enemies)
        {
            target = v;
            yield return StartCoroutine(movement.Moving(v));
            if (Math.Abs(v.x - transform.position.x) <= 1)
                busy = false;
        }
    }

    public bool enoughSteps(Vector2 v, (int, int) distance)
    {
        Vector3 cntr = this.transform.position;
        ABPath p = (ABPath)seeker.StartPath(cntr, v);
        p.BlockUntilCalculated();
        int len = p.vectorPath.Count;
        int k = (p.vectorPath[len - 1].x - p.vectorPath[len - 2].x) > 0 ? 1 : -1;
        //Vector2 start = v + Vector2.down + new Vector2(k * distance.Item2, 0);
        //Vector2 end = v + Vector2.down + new Vector2(k * distance.Item1, 0);
        RaycastHit2D hit = Physics2D.Raycast(v + Vector2.down, new Vector2(k, 0));
        if (hit.rigidbody == null)
            return false;
        else if (p.GetTotalLength() <= steps + hit.distance)
            return true;
        else
            return false;
    }

    public bool canMoveTo(Vector3 target)
    {
        //Messenger.Broadcast(CharacterEvent.REMOVE_NODE);
        //Messenger<Vector2>.Broadcast(CharacterEvent.NEW_NODE, (Vector2)v);
        bool walkable = false;
        Path path = seeker.StartPath(transform.position, target);
        path.BlockUntilCalculated();
        int len = path.vectorPath.Count;
        //Debug.Log(path.vectorPath[len - 1] + " " + v);
        Vector3 lastNode = path.vectorPath[len - 1];
        if (lastNode == target)
            walkable = true;
        /*else
        {
            Vector2 direction = (lastNode.x >= target.x)  ? Vector2.left : Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(lastNode, direction);
            if (hit.rigidbody != null)
            {
                if ((int)lastNode.y == (int)target.y && hit.rigidbody.gameObject.tag == "Enemy")
                    walkable = true;
                //Debug.Log(hit.rigidbody.gameObject.tag + " " + hit.distance + " " + direction + " " + lastNode + " " + v);
            }
        }*/
        //Messenger.Broadcast(CharacterEvent.REMOVE_NODE);
        return walkable;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        seeker = GetComponent<Seeker>();
        ai = GetComponent<AILerp>();
        ai.canMove = false;
        opponentTag = gameObject.tag == "Player" ? "Enemy" : "Player";
        movement = GetComponent<MovementBehaviour>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        target = Vector2.zero;
        animator = GetComponent<Animator>();
        weapons = new List<Weapon>();
        triggerCount = 1;
        triggers = triggerCount;
        busy = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
