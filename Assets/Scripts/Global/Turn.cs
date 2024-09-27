using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    [SerializeField] private UI ui;
    [SerializeField] private MouseBehaviour mouse;

    CharacterController[] characters;
    CharacterController curChar;
    int index, maxIndex;
    public string NameOfCurChar
    {
        get
        {
            return curChar.gameObject.name;
        }
    }

    void Awake()
    {
        Messenger.AddListener(TurnEvent.NEXT_TURN, nextTurn);
    }

    void OnDestroy()
    {
        Messenger.AddListener(TurnEvent.NEXT_TURN, nextTurn);
    }

    void ChangeSide()
    {
        if (curChar.gameObject.tag == "Player")
        {
            mouse.SetController(curChar);
            //Messenger.Broadcast(TurnEvent.TEAMMATES_TURN);
        }
        else
        {
            //Messenger.Broadcast(TurnEvent.ENEMY_TURN);
        }
    }

    void nextTurn()
    {
        curChar.EndOfTurn();
        index++;
        if (index > maxIndex)
            index = 0;
        curChar = characters[index];
        ChangeSide();
        curChar.StartOfTurn();
        ui.currentCharacterLabel(NameOfCurChar);
    }

    // Start is called before the first frame update
    void Start()
    {
        characters = this.GetComponentsInChildren<CharacterController>();
        maxIndex = characters.Length - 1;
        index = 0;
        curChar = characters[index];
        ChangeSide();
        curChar.StartOfTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
