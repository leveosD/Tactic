using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Graph : MonoBehaviour
{
    private PointGraph graph;
    [SerializeField] private GameObject nodePrefab;
    private List<GameObject> newNodes;
    GameObject newNode;

    PointNode a, b;

    void Awake()
    {
        Messenger<Vector2>.AddListener(CharacterEvent.NEW_NODE, NewNode);
        Messenger.AddListener(CharacterEvent.REMOVE_NODE, RemoveNode);
        Messenger.AddListener(TurnEvent.ENEMY_TURN, EnemyTurn);
        Messenger.AddListener(TurnEvent.TEAMMATES_TURN, TeammateTurn);
    }

    void OnDestroy()
    {
        Messenger<Vector2>.RemoveListener(CharacterEvent.NEW_NODE, NewNode);
        Messenger.AddListener(CharacterEvent.REMOVE_NODE, RemoveNode);
        Messenger.AddListener(TurnEvent.ENEMY_TURN, EnemyTurn);
        Messenger.AddListener(TurnEvent.TEAMMATES_TURN, TeammateTurn);
    }

    private void NewNode(Vector2 node)
    {
        Vector3 nearest = (Vector3)graph.GetNearest(node).node.position;
        float distance = Math.Abs(nearest.x - node.x);
        bool isNode =  distance > 0.1f ? false : true;
        /*AstarPath.active.AddWorkItem(new AstarWorkItem(() => {
            newNode = graph.AddNode((Int3)(Vector3)node);
        }));*/
        if (!isNode)
        {
            newNode = Instantiate(nodePrefab) as GameObject;
            newNode.transform.position = node;
            newNodes.Add(newNode);
        }
        //updtNode = false;
    }

    private void RemoveNode()
    {
        if(newNodes.Count != 0)
        {
            foreach(GameObject node in newNodes)
            {
                Destroy(node);
            }
        }
    }

    void EnemyTurn()
    {
        graph.mask += LayerMask.GetMask("Teammates");
        graph.Scan();
        graph.mask -= LayerMask.GetMask("Teammates");
    }

    void TeammateTurn()
    {
        graph.mask += LayerMask.GetMask("Enemies");
        graph.Scan();
        graph.mask -= LayerMask.GetMask("Enemies");
    }

    public bool canMoveTo(Vector3 f, Vector3 s)
    {
        a.position = (Int3)f;
        b.position = (Int3)s;
        //Debug.Log(a.position + " " + b.position);
        float dist = 0;
        bool connection = graph.IsValidConnection(a, b, out dist);
        //Debug.Log(connection + " " + dist);
        return connection;
    }

    // Start is called before the first frame update
    void Start()
    {
        newNodes = new List<GameObject>();
        graph = AstarPath.active.data.pointGraph;
        a = graph.nodes[graph.nodeCount - 1];
        b = graph.nodes[graph.nodeCount - 2];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
