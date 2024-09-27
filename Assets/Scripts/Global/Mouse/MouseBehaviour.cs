//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using Pathfinding;

public class MouseBehaviour : MonoBehaviour
{
    public const int HEIGHT = 10;
    public const int WIDTH = 26;

    public Vector3 currentCell;
    public Vector2 tempMousePos;

    //[SerializeField] private Graph graph;
    SpriteRenderer sprite;
    //Seeker seeker;
    public CharacterController controller;
    bool activeSkill;

    //string currentTag;
    //Color targetColor;
    //const int COLOR = 4;
    //string[][] colorTriggers;

    bool inCell(Vector2 v)
    {
        if (Math.Abs(v.x - currentCell.x) >= 0.5 && Math.Abs(v.y - currentCell.y) >= 0.5)
        {
            return false;
        }
        return true;
    }

    void Awake()
    {
        //Messenger.AddListener(CharacterEvent.STAND, WayIsOver);
        //Messenger<Skill>.AddListener(CharacterEvent.CHANGE_SKILL, ChangeSkill);
        //Messenger.AddListener(TurnEvent.ENOUGHSTEPS, ChangeColor);
    }

    void OnDestroy()
    {
        //Messenger.RemoveListener(CharacterEvent.STAND, WayIsOver);
        //Messenger<Skill>.RemoveListener(CharacterEvent.CHANGE_SKILL, ChangeSkill);
        //Messenger.RemoveListener(TurnEvent.ENOUGHSTEPS, ChangeColor);
    }

    public void SetController(CharacterController c)
    {
        controller = c;
    }

    /*void ChangeColor(string target)
    {
        if(currentTag != target && !activeSkill)
        {
            if (Array.IndexOf(colorTriggers[0], target) != -1)
            {
                sprite.color = targetColor;
            }
            currentTag = target;
        }
    }

    void ChangeSkill(Skill s)
    {
        //colorTriggers = s.TargetTypes;
        //targetColor = s.ColorP;
        sprite.color = s.ColorP;
        activeSkill = true;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        currentCell = Vector3.zero;
        tempMousePos = Input.mousePosition;
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.blue;
        //colorTriggers = new string[COLOR][];
        //seeker = GetComponent<Seeker>();
        activeSkill= false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.busy && (Math.Abs(tempMousePos.x - Input.mousePosition.x) >= 20 || Math.Abs(tempMousePos.y - Input.mousePosition.y) >= 20))
        {
            tempMousePos = Input.mousePosition;
            if (!inCell(tempMousePos))
            {
                currentCell = new Vector3((float)((int)(tempMousePos.x * WIDTH / Screen.width)) - WIDTH / 2 + 0.5f, (float)((int)(tempMousePos.y * HEIGHT / Screen.height)) - HEIGHT / 2, 0);
                transform.position = currentCell;
                currentCell += new Vector3(0, 0.5f, 0);

                bool canMove = controller.canMoveTo(currentCell); // graph.canMoveTo(controller.transform.position, currentCell);
                bool amountOfSteps = true;// controller.enoughSteps(currentCell, (0, 0));
                if (canMove && amountOfSteps)
                {
                    Collider2D col = Physics2D.OverlapPoint(currentCell);
                    if (col == null)
                        sprite.color = Color.blue;
                    else
                    {
                        string tag = col.gameObject.tag;
                        Debug.Log(tag);
                        switch (tag)
                        {
                            case MouseTargetTypes.ENEMY:
                            case MouseTargetTypes.INTERACTIVE_OBJECT:
                                sprite.color = Color.red;
                                break;
                            case MouseTargetTypes.TEAMMATE:
                            case MouseTargetTypes.PLAYER:
                                sprite.color = Color.green;
                                break;
                            default:
                                sprite.color = Color.white;
                                break;
                        }
                    }
                }
                else
                    sprite.color = Color.white;
            }
        }
        
        //else if (EventSystem.current.IsPointerOverGameObject())
        
        else if (Input.GetMouseButtonDown(1) && !controller.busy && controller.ItsTurn && sprite.color == Color.blue)
        {
            //controller.busy = true;
            StartCoroutine(controller.Move((Vector2)currentCell));
        }
        else if (Input.GetMouseButtonDown(0) && !controller.busy && controller.ItsTurn && controller.CurrentSkill && !EventSystem.current.IsPointerOverGameObject())// && walkable)
        {
            //controller.busy = true;
            Collider2D col = Physics2D.OverlapPoint((Vector2)currentCell);
            if (col != null)
            {
                StartCoroutine(controller.CurrentSkill.DoSkill(col.transform.position));
            }
        }
        else if(Input.GetMouseButtonDown(0) && !controller.busy && sprite.color == Color.green)
        {
            GameObject hero = Physics2D.OverlapPoint(currentCell).gameObject;
            Messenger<GameObject>.Broadcast(GameEvents.CHANGE_HERO, hero);
        }
    }
}