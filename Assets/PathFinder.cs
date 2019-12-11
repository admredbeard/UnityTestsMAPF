using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PathFinder : MonoBehaviour
{
    public GameObject[] robots;

    bool second = false;
    Color[] colors;
    // Start is called before the first frame update
    void Start()
    {

        colors = new Color[5];
        colors[0] = Color.white;
        colors[1] = Color.red;
        colors[2] = Color.blue;
        colors[3] = Color.black;
        colors[4] = Color.cyan;

        InitMap();
        List<Node> endPoints = new List<Node>();
        endPoints.Add(map[19, 8]);
        endPoints.Add(map[18, 9]);
        endPoints.Add(map[18, 14]);
        endPoints.Add(map[18, 15]);
        endPoints.Add(map[12, 5]);

        endPoints.Add(map[12, 8]);
        endPoints.Add(map[13, 6]);
        endPoints.Add(map[15, 14]);
        endPoints.Add(map[16, 14]);
        endPoints.Add(map[15, 6]);


        int test = 0;
        foreach(Node nod in endPoints)
        {
            robots[test].transform.position = new Vector3(nod.position.x, nod.position.y, -1);
            nod.SetEndPoint(true);
            test++;
        }
        System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        st.Start();
        path = FindPath(map[2, 1], endPoints[0], 0);
        ShowPath(path.nodes);

        path2 = FindPath(map[1, 1], endPoints[1], 0);
        ShowPath(path2.nodes);

        path3 = FindPath(map[3, 1], endPoints[2], 0);
        ShowPath(path3.nodes);

        path4 = FindPath(map[8, 3], endPoints[3], 0);
        ShowPath(path4.nodes);

        path5 = FindPath(map[5, 5], endPoints[4], 0);
        ShowPath(path5.nodes);

        path6 = FindPath(map[22, 5], endPoints[5], 0);
        ShowPath(path6.nodes);

        path7 = FindPath(map[24, 20], endPoints[6], 0);
        ShowPath(path7.nodes);

        path8 = FindPath(map[21, 4], endPoints[7], 0);
        ShowPath(path8.nodes);

        path9 = FindPath(map[28, 15], endPoints[8], 0);
        ShowPath(path9.nodes);

        path10 = FindPath(map[5, 20], endPoints[9], 0);
        ShowPath(path10.nodes);
        st.Stop();
        Debug.Log(st.ElapsedMilliseconds);

        Debug.Log(map[15, 10].timesteps[0]);
        Debug.Log(map[15, 10].timesteps[1]);
        StartCoroutine(VisualizeMovement());
    }

    Pathstruct path;
    Pathstruct path2;
    Pathstruct path3;
    Pathstruct path4;
    Pathstruct path5;
    Pathstruct path6;
    Pathstruct path7;
    Pathstruct path8;
    Pathstruct path9;
    Pathstruct path10;

    int path1Count = 0;
    int path2Count = 0;
    int path3Count = 0;
    int path4Count = 0;
    int path5Count = 0;
    int path6Count = 0;
    int path7Count = 0;
    int path8Count = 0;
    int path9Count = 0;
    int path10Count = 0;



    IEnumerator VisualizeMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if(path.nodes.Count > path1Count)
            {
                robots[0].transform.position = new Vector3(path.nodes[path1Count].position.x, path.nodes[path1Count].position.y, -1);
                path1Count++;
            }
            if (path2.nodes.Count > path2Count)
            {
                robots[1].transform.position = new Vector3(path2.nodes[path2Count].position.x, path2.nodes[path2Count].position.y, -1);
                path2Count++;
            }
            if (path3.nodes.Count > path3Count)
            {
                robots[2].transform.position = new Vector3(path3.nodes[path3Count].position.x, path3.nodes[path3Count].position.y, -1);
                path3Count++;
            }
            if (path4.nodes.Count > path4Count)
            {
                robots[3].transform.position = new Vector3(path4.nodes[path4Count].position.x, path4.nodes[path4Count].position.y, -1);
                path4Count++;
            }
            if (path5.nodes.Count > path5Count)
            {
                robots[4].transform.position = new Vector3(path5.nodes[path5Count].position.x, path5.nodes[path5Count].position.y, -1);
                path5Count++;
            }
            if (path6.nodes.Count > path6Count)
            {
                robots[5].transform.position = new Vector3(path6.nodes[path6Count].position.x, path6.nodes[path6Count].position.y, -1);
                path6Count++;
            }
            if (path7.nodes.Count > path7Count)
            {
                robots[6].transform.position = new Vector3(path7.nodes[path7Count].position.x, path7.nodes[path7Count].position.y, -1);
                path7Count++;
            }
            if (path8.nodes.Count > path8Count)
            {
                robots[7].transform.position = new Vector3(path8.nodes[path8Count].position.x, path8.nodes[path8Count].position.y, -1);
                path8Count++;
            }
            if (path9.nodes.Count > path9Count)
            {
                robots[8].transform.position = new Vector3(path9.nodes[path9Count].position.x, path9.nodes[path9Count].position.y, -1);
                path9Count++;
            }
            if (path10.nodes.Count > path10Count)
            {
                robots[9].transform.position = new Vector3(path10.nodes[path10Count].position.x, path10.nodes[path10Count].position.y, -1);
                path10Count++;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Node[,] map;

    void InitMap()
    {
        GameObject grid = GameObject.Find("Grid");
        Tilemap tileMap = grid.GetComponentInChildren<Tilemap>();
        GridLayout gridLayout = grid.GetComponent<GridLayout>();
        Vector3Int total = tileMap.size;
        int halfX = total.x / 2;
        int halfY = total.y / 2;
        

        map = new Node[total.x, total.y];
        foreach (Vector3Int position in tileMap.cellBounds.allPositionsWithin)
        {
            if (tileMap.HasTile(position))
            {
                TileBase current = tileMap.GetTile(position);
                if (current.name == "Tile")
                {
                    if(tileMap.GetCellCenterWorld(position).x == -5.5f && tileMap.GetCellCenterWorld(position).y == -1.5f)
                    {
                        Debug.Log(position.x + halfX);
                        Debug.Log(position.y + halfY);
                    }
                    map[position.x+halfX, position.y+halfY] = new Node(true, tileMap.GetCellCenterWorld(position), position.x+halfX, position.y+halfY);
                }
                else
                {
                    map[position.x+halfX, position.y+halfY] = new Node(false, tileMap.GetCellCenterWorld(position), position.x+halfX, position.y+halfY);
                }
            }
        }
    }

    
    int colorCount = 0;
    void ShowPath(List<Node> path)
    {
        GameObject gemObj = LinerendererPooling.SharedInstance.GetPooledVFX();
        LineRenderer lineR = gemObj.GetComponent<LineRenderer>();
        lineR.SetVertexCount(path.Count);
        
        lineR.material.color = colors[0];
        //lineR.SetColors(colors[colorCount], colors[colorCount]);
        //lineR.endColor = colors[colorCount];
        for (int i = 0; i < path.Count; i++)
        {
            
            lineR.SetPosition(i, new Vector3(path[i].position.x, path[i].position.y, -1));
        }
        gemObj.SetActive(true);
        colorCount++;
    }

    Pathstruct FindPath(Node _start, Node _goal, int startTime)
    {

        List<Node> path = new List<Node>();
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        _start.currentTimeStep = 0;
        open.Add(_start);
        int count = 0;

        while (open.Count > 0 && count < 10000)
        {

            count++;
            Node current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].Cost < current.Cost || (open[i].Cost == current.Cost && open[i].hCost < current.hCost))
                    current = open[i];
            }

            open.Remove(current);
            closed.Add(current);

            if (current.position == _goal.position)
            {
                return Path(_start, _goal, startTime);
            }
            

            foreach (Node neighbour in GetNeighbours(current))
            {
                if (!neighbour.traversable || closed.Contains(neighbour) || (neighbour.isEndPoint && _goal != neighbour))
                    continue;

                neighbour.SetCurrentTimestep(current.currentTimeStep + 1);

                float newCost = current.gCost + GetDistance(neighbour, _goal);
                if (newCost < neighbour.gCost || !open.Contains(neighbour))
                {
                    current.child = neighbour;
                    neighbour.parent = current;
                    neighbour.gCost = newCost;
                    neighbour.hCost = Heuristic(neighbour, neighbour.currentTimeStep);


                    if (!open.Contains(neighbour))
                        open.Add(neighbour);

                }
            }
        }
        Debug.Log("Found no path");
        return new Pathstruct();
    }

    float Heuristic(Node nod, int timestep)
    {
        if (nod.timesteps.Contains(timestep))
        {
            return 2000;
        }else if (nod.safeTimesteps.Contains(timestep))
        {
            return 1000;
        }
        /*
        foreach(Node nud in GetNeighbours(nod))
        {
            if (nud.safeTimesteps.Contains(timestep) || nud.timesteps.Contains(timestep))
            {
                return 1000;
            }
        }
        */
        return 5;
    }

    public struct Pathstruct
    {
        public List<Node> nodes;
        public List<int> timestep;
    }

    Pathstruct Path(Node start, Node goal, int startTime)
    {
        Pathstruct newPath = new Pathstruct();
        newPath.nodes = new List<Node>();
        newPath.timestep = new List<int>();
        start.AddTimestep(0);
        start.AddSafeTimeStep(1);
        Node current = goal;
        int count = startTime + 1;
        int currenttime = goal.currentTimeStep;
        while (current.position != start.position && current != null)
        {
            while(currenttime != current.currentTimeStep)
            {
                newPath.nodes.Add(current);
                newPath.timestep.Add(currenttime);
                currenttime--;
            }
            currenttime--;
            newPath.nodes.Add(current);
            newPath.timestep.Add(current.currentTimeStep);
            current = current.parent;

            if (current == null)
                break;
        }
        newPath.nodes.Reverse();
        newPath.timestep.Reverse();

        for (int i = 0; i < newPath.nodes.Count; i++)
        {
            Debug.Log(newPath.nodes[i].currentTimeStep);
            newPath.nodes[i].AddTimestep(newPath.nodes[i].currentTimeStep);
            if(i-1 >= 0)
            {
                newPath.nodes[i - 1].AddSafeTimeStep(newPath.nodes[i].currentTimeStep);
            }
            newPath.nodes[i].SetCurrentTimestep(-1);
        }
        
        return newPath;
    }
    int GetDistance(Node nodeA, Node nodeB)
    {
        int x = (int)Mathf.Abs(nodeA.position.x - nodeB.position.x);
        int z = (int)Mathf.Abs(nodeA.position.y - nodeB.position.y);
        return x + z;
    }

    List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        if(node.parent != map[node.xVal + 1, node.yVal])
            neighbours.Add(map[node.xVal + 1, node.yVal]);
        if (node.parent != map[node.xVal - 1, node.yVal])
            neighbours.Add(map[node.xVal - 1, node.yVal]);
        if(node.parent != map[node.xVal, node.yVal + 1])
            neighbours.Add(map[node.xVal, node.yVal + 1]);
        if(node.parent != map[node.xVal, node.yVal - 1])
            neighbours.Add(map[node.xVal, node.yVal - 1]);
        return neighbours;
    }

    public class Node
    {
        public bool traversable = true;
        public Vector2 position;
        public float gCost = 0;
        public float hCost = 0;

        public int xVal = 0;
        public int yVal = 0;

        public int currentTimeStep = 0;

        public Node parent;
        public Node child;

        public List<int> timesteps;
        public List<int> safeTimesteps;

        public bool isEndPoint = false;

        public Node(bool _traversable, Vector2 _position, int _xVal, int _yVal)
        {
            timesteps = new List<int>();
            safeTimesteps = new List<int>();
            traversable = _traversable;
            position = _position;
            xVal = _xVal;
            yVal = _yVal;
        }

        public void SetEndPoint(bool value)
        {
            isEndPoint = value;
        }

        public void SetCurrentTimestep(int timestep)
        {
            currentTimeStep = timestep;
        }

        public void AddSafeTimeStep(int time)
        {
            safeTimesteps.Add(time);
        }

        public void AddTimestep(int time)
        {

            timesteps.Add(time);
        }

        public void RemoveTimestep(int time)
        {
            timesteps.Remove(time);
        }

        public float Cost
        {
            get { return gCost + hCost; }
        }
    }
}
