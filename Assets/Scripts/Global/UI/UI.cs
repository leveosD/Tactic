using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private Text label;

    void Awake()
    {
        Messenger<GameObject>.AddListener(GameEvents.CHANGE_HERO, ChangeHero);
    }

    void OnDestroy()
    {
        Messenger<GameObject>.RemoveListener(GameEvents.CHANGE_HERO, ChangeHero);
    }

    void ChangeHero(GameObject hero)
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void currentCharacterLabel(string name)
    {
        label.text = name;
    }

    public void nextTurnButton()
    {
        Messenger.Broadcast(TurnEvent.NEXT_TURN);
    }

    public void checkButton()
    {
        label.text = name;
    }
}
