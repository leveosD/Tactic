using System;
using System.Collections;
using UnityEngine;
using Pathfinding;

public class MovementBehaviour : MonoBehaviour
{
    //public string mode = CharacterEvent.STAND;

    //CharacterController controller;
    Seeker seeker;
    AILerp ai;
    Rigidbody2D rb;

    public Vector2 target;
    public bool jumpup;
    Vector2 vJump;
    private float Speed = 10f;
    public bool isGround;
    const float FORCE = 5;
    public bool jumpDone;
    int jumpDirection;

    public bool keepMoving;
    //IEnumerator coroutine;

    public Vector2 startPos;

    public Vector2 StartPos
    {
        get
        {
            return startPos;
        }
        set
        {
            int k = value.x >= 0 ? 1 : -1;
            float x = (Math.Abs((int)value.x) + 0.5f) * k;
            k = value.y >= 0 ? 1 : -1;
            float y = (Math.Abs((int)value.y) + 0.5f) * k;
            startPos = new Vector2(x, y);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        target = (Vector2)transform.position;
        StartPos = transform.position;
        seeker = GetComponent<Seeker>();
        ai = GetComponent<AILerp>();
        isGround = false;
        rb = GetComponent<Rigidbody2D>();
        jumpDone = false;
        keepMoving = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Moving(Vector2 target)
    {
        //mode = CharacterEvent.MOVE;
        keepMoving = true;
        this.target = target;
        Messenger.Broadcast(CharacterEvent.REMOVE_NODE);
        Messenger<Vector2>.Broadcast(CharacterEvent.NEW_NODE, (Vector2)target);
        seeker.StartPath(this.transform.position, new Vector3(target.x, target.y, 0));
        ai.canMove = true;
        StartPos = transform.position;
        //Debug.Log("Moving is active");

        while (keepMoving && Math.Abs(transform.position.x - target.x) >= 0.01f)
        {
            yield return new WaitUntil(() => !keepMoving || Math.Abs(transform.position.y - StartPos.y) >= 0.1f || Math.Abs(transform.position.x - target.x) <= 0.01f);

            if (!keepMoving || Math.Abs(transform.position.x - target.x) <= 0.01f)
            {
                //Debug.Log(keepMoving);
                yield return null;
            }

            else if (Math.Abs(transform.position.y - StartPos.y) >= 0.1f)
            {
                //Debug.Log(transform.position + " " + StartPos);
                ai.canMove = false;
                jumpDone = false;
                jumpDirection = transform.position.x - StartPos.x >= 0 ? 1 : -1;
                float y;
                float x;
                if (transform.position.y - StartPos.y < 0)
                {
                    y = 1f;
                    x = 0.12f;
                    jumpup = false;
                }
                else
                {
                    y = 1.5f;
                    x = 0.05f;
                    jumpup = true;
                }
                StartPos = transform.position;
                //mode = CharacterEvent.JUMP;
                rb.bodyType = RigidbodyType2D.Dynamic;
                //Debug.Log("isGround " + isGround);
                rb.AddForce(new Vector2(jumpDirection * x, y) * FORCE, ForceMode2D.Impulse);

                while (!(isGround && jumpDone))
                {
                    if (!jumpDone && jumpup && transform.position.y - StartPos.y >= 2.1f)
                    {
                        rb.AddForce(new Vector2(jumpDirection, 0) * 0.65f, ForceMode2D.Impulse);
                        jumpDone = true;
                    }
                    else if (Math.Abs(StartPos.x - transform.position.x) >= 0.5f)
                    {
                        jumpDone = true;
                    }
                    yield return null;
                }
                //mode = CharacterEvent.MOVE;
                StartPos = transform.position;
                rb.bodyType = RigidbodyType2D.Kinematic;
                seeker.StartPath(this.transform.position, (Vector3)target, (p) =>
                {
                    ai.canMove = true;
                });
            }
            yield return null;
        }

        rb.bodyType = RigidbodyType2D.Kinematic;
        //mode = CharacterEvent.STAND;
        StartPos = transform.position;
        ai.canMove = false;
        keepMoving = false;
        //Messenger.Broadcast(CharacterEvent.STAND);
    }
}