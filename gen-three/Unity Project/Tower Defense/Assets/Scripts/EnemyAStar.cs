using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyAStar : MonoBehaviour
{
    public GameManager Game;
    public MyPathNode nextNode;
    bool gray = false;
    public MyPathNode[,] grid;
    public gridPosition currentGridPosition = new gridPosition();
    public gridPosition startGridPosition = new gridPosition();
    public gridPosition endGridPosition = new gridPosition();
    private Orientation gridOrientation = Orientation.Vertical;
    private bool allowDiagonals = false;
    private bool correctDiagonalSpeed = true;
    private Vector2 input;
    private bool isMoving = true;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float t;
    private float factor;
    private Color myColor;
    public float moveSpeed;

    public class MySolver<TPathNode, TUserContext> : SettlersEngine.SpatialAStar<TPathNode,
    TUserContext> where TPathNode : SettlersEngine.IPathNode<TUserContext>
    {
        protected override Double Heuristic(PathNode inStart, PathNode inEnd)
        {
            int formula = GameManager.distance;
            int dx = Math.Abs(inStart.X - inEnd.X);
            int dy = Math.Abs(inStart.Y - inEnd.Y);
            if (formula == 0)
                return Math.Sqrt(dx * dx + dy * dy); //Euclidean distance
            else if (formula == 1)
                return (dx * dx + dy * dy); //Euclidean distance squared
            else if (formula == 2)
                return Math.Min(dx, dy); //Diagonal distance
            else if (formula == 3)
                return (dx * dy) + (dx + dy); //Manhatten distance
            else
                return Math.Abs(inStart.X - inEnd.X) + Math.Abs(inStart.Y - inEnd.Y);
        }

        protected override Double NeighborDistance(PathNode inStart, PathNode inEnd)
        {
            return Heuristic(inStart, inEnd);
        }

        public MySolver(TPathNode[,] inGrid)
            : base(inGrid)
        {
        }
    }

    void Start()
    {
        myColor = getRandomColor();
        // Straight line pathfinding
        int endPos = UnityEngine.Random.Range(0, Game.gridHeight - 1);
        startGridPosition = new gridPosition(0, endPos);
        endGridPosition = new gridPosition(17, endPos);
        initializePosition();
        MySolver<MyPathNode, System.Object> aStar = new MySolver<MyPathNode, System.Object>(Game.grid);
        IEnumerable<MyPathNode> path = aStar.Search(new Vector2(startGridPosition.x, startGridPosition.y), new Vector2(endGridPosition.x, endGridPosition.y), null);
        updatePath();
        this.GetComponent<Renderer>().material.color = myColor;
    }

    public void findUpdatedPath(int currentX, int currentY)
    {
        MySolver<MyPathNode, System.Object> aStar = new MySolver<MyPathNode, System.Object>(Game.grid);
        IEnumerable<MyPathNode> path = aStar.Search(new Vector2(currentX, currentY), new Vector2(endGridPosition.x, endGridPosition.y), null);
        int x = 0;
        if (path != null)
        {
            foreach (MyPathNode node in path)
            {
                if (x == 1)
                {
                    nextNode = node;
                    break;
                }

                x++;
            }

            // Changes enemy's path color
            //foreach (GameObject g in GameObject.FindGameObjectsWithTag("GridBox"))
            //{
            //    if (g.GetComponent<Renderer>().material.color != Color.red && g.GetComponent<Renderer>().material.color == myColor)
            //        g.GetComponent<Renderer>().material.color = Color.white;
            //}

            //foreach (MyPathNode node in path)
            //{
            //    //GameObject.Find(node.X + "," + node.Y).GetComponent<Renderer>().material.color = myColor;
            //}
        }
    }

    Color getRandomColor()
    {
        Color tmpCol = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        return tmpCol;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            StartCoroutine(move());
        }
    }

    public class gridPosition
    {
        public int x = 0;
        public int y = 0;

        public gridPosition()
        {
        }

        public gridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    };

    private enum Orientation
    {
        Horizontal,
        Vertical
    };

    public IEnumerator move()
    {
        isMoving = true;
        startPosition = transform.position;
        t = 0;

        if (gridOrientation == Orientation.Horizontal)
        {
            endPosition = new Vector2(startPosition.x + System.Math.Sign(input.x) * Game.gridSize,
                                      startPosition.y);
            currentGridPosition.x += System.Math.Sign(input.x);
        }
        else
        {
            endPosition = new Vector2(startPosition.x + System.Math.Sign(input.x) * Game.gridSize,
                                      startPosition.y + System.Math.Sign(input.y) * Game.gridSize);
            currentGridPosition.x += System.Math.Sign(input.x);
            currentGridPosition.y += System.Math.Sign(input.y);
        }

        if (allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0)
        {
            factor = 0.9071f;
        }
        else
        {
            factor = 1f;
        }

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / Game.gridSize) * factor;
            transform.position = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        isMoving = false;
        getNextMovement();
        yield return 0;
    }

    void updatePath()
    {
        findUpdatedPath(currentGridPosition.x, currentGridPosition.y);
    }

    void getNextMovement()
    {
        updatePath();
        input.x = 0;
        input.y = 0;
        if (nextNode.X > currentGridPosition.x)
        {
            input.x = 1;
            this.GetComponent<SpriteRenderer>().sprite = Game.carFront;
        }

        if (nextNode.Y > currentGridPosition.y)
        {
            input.y = 1;
            this.GetComponent<SpriteRenderer>().sprite = Game.carUp;
        }

        if (nextNode.Y < currentGridPosition.y)
        {
            input.y = -1;
            this.GetComponent<SpriteRenderer>().sprite = Game.carDown;
        }

        if (nextNode.X < currentGridPosition.x)
        {
            input.x = -1;
            this.GetComponent<SpriteRenderer>().sprite = Game.carBack;
        }

        StartCoroutine(move());
    }

    public Vector2 getGridPosition(int x, int y)
    {
        float contingencyMargin = Game.gridSize * .10f;
        float posX = Game.gridBox.transform.position.x + (Game.gridSize * x) - contingencyMargin;
        float posY = Game.gridBox.transform.position.y + (Game.gridSize * y) + contingencyMargin;
        return new Vector2(posX, posY);
    }

    public Vector2 fetchGridPosition()
    {
        return new Vector2(this.currentGridPosition.x, this.currentGridPosition.y);
    }

    public void initializePosition()
    {
        this.gameObject.transform.position = getGridPosition(startGridPosition.x, startGridPosition.y);
        currentGridPosition.x = startGridPosition.x;
        currentGridPosition.y = startGridPosition.y;
        isMoving = false;
        // Changes enemy's start tile to black
        //GameObject.Find(startGridPosition.x + "," + startGridPosition.y).GetComponent<Renderer>().material.color = Color.black;
    }
}
