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
        

        Invoke("Test", 2f);
    }

    Node WorldPosToGridPos(Vector3 position)
    {
        int xPos = tileMap.WorldToCell(position).x + halfX;
        int yPos = tileMap.WorldToCell(position).y + halfY;
        return map[xPos,yPos];
    }

    List<Node> GetStartPoints()
    {
        List<Node> startpoints = new List<Node>();
        for (int i = 0; i < robots.Length; i++)
        {
            GameObject temp = robots[i];
            int randomIndex = Random.Range(i, robots.Length);
            robots[i] = robots[randomIndex];
            robots[randomIndex] = temp;
        }


        for (int i = 0; i < robots.Length; i++)
        {
            startpoints.Add(WorldPosToGridPos(robots[i].transform.position));
        }

        
        foreach (Node nod in startpoints)
        {
            nod.SetEndPoint(true);
        }
        
        return startpoints;

    }
    List<Node> GetEndPoints()
    {
        List<Node> endPoints = new List<Node>();
        endPoints.Add(map[36, 20]);
        endPoints.Add(map[37, 20]);
        endPoints.Add(map[38, 20]);
        endPoints.Add(map[39, 20]);
        endPoints.Add(map[40, 20]);

        endPoints.Add(map[36, 21]);
        endPoints.Add(map[37, 21]);
        endPoints.Add(map[38, 21]);
        endPoints.Add(map[39, 21]);
        endPoints.Add(map[40, 21]);

        endPoints.Add(map[36, 2]);
        endPoints.Add(map[37, 2]);
        endPoints.Add(map[38, 2]);
        endPoints.Add(map[39, 2]);
        endPoints.Add(map[40, 2]);

        endPoints.Add(map[36, 3]);
        endPoints.Add(map[37, 3]);
        endPoints.Add(map[38, 3]);
        endPoints.Add(map[39, 3]);
        endPoints.Add(map[40, 3]);
        return endPoints;
    }

    public struct ListOfPaths
    {
        public List<Node> path;
        public int count;
    }

    ListOfPaths[] paths;

    private void Test()
    {
        List<Node> startPoints = GetStartPoints();
        List<Node> endPoints = GetEndPoints();

        paths = new ListOfPaths[startPoints.Count];
        
        System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        st.Start();

        bool pathsFound = false;
        int j = 0;
        while (!pathsFound && j < 15)
        {
            pathsFound = true;
            for (int i = 0; i < paths.Length; i++)
            {
                if(paths[i].path == null)
                {
                    paths[i].path = FindPath(startPoints[i], endPoints[0], j);
                }
                if(paths[i].path == null)
                {
                    pathsFound = false;
                }
                else
                {
                    startPoints[i].SetEndPoint(false);
                }
            }
            j++;
        }

        for(int i = 0; i < paths.Length; i++)
        {
            paths[i].count = 0;
            ShowPath(paths[i].path);
        }
        

        st.Stop();
        Debug.Log(st.ElapsedMilliseconds);
        Debug.Log(longestPathCount);
        StartCoroutine(VisualizeMovement());
    }



    IEnumerator VisualizeMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            for(int i = 0; i < paths.Length; i++)
            {
                List<Node> route = paths[i].path;
                if (route != null && route.Count > paths[i].count)
                {
                    robots[i].transform.position = new Vector3(route[paths[i].count].position.x, route[paths[i].count].position.y, -1);
                    paths[i].count++;
                }
            }
        }
    }
    

    public Node[,] map;

    Tilemap tileMap;
    int halfX;
    int halfY;

    void InitMap()
    {
        GameObject grid = GameObject.Find("Grid");
        tileMap = grid.GetComponentInChildren<Tilemap>();
        GridLayout gridLayout = grid.GetComponent<GridLayout>();
        Vector3Int total = tileMap.size;
        halfX = total.x / 2;
        halfY = total.y / 2;
        

        map = new Node[total.x, total.y];
        foreach (Vector3Int position in tileMap.cellBounds.allPositionsWithin)
        {
            if (tileMap.HasTile(position))
            {
                TileBase current = tileMap.GetTile(position);
                if (current.name == "Tile")
                {
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
        if (path == null)
            return;
        GameObject gemObj = LinerendererPooling.SharedInstance.GetPooledVFX();
        LineRenderer lineR = gemObj.GetComponent<LineRenderer>();
        lineR.SetVertexCount(path.Count);
        
        lineR.material.color = colors[0];
        for (int i = 0; i < path.Count; i++)
        {
            
            lineR.SetPosition(i, new Vector3(path[i].position.x, path[i].position.y, -1));
        }
        gemObj.SetActive(true);
        colorCount++;
    }

    List<Node> FindPath(Node _start, Node _goal, int startTime)
    {

        List<Node> path = new List<Node>();
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        _start.currentTimeStep = startTime;
        open.Add(_start);
        int count = 0;

        while (open.Count > 0 && count < 4000)
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
                {
                    continue;
                }
                    
                
                if(neighbour.timesteps.Contains(current.currentTimeStep + 1) && _goal != neighbour)
                {

                    continue;
                }
                

                float newCost = current.gCost + GetDistance(current, neighbour);
                
                if (newCost < neighbour.gCost || !open.Contains(neighbour))
                {
                    if(neighbour.timesteps.Contains(current.currentTimeStep+1) && _goal != neighbour)
                    {
                        if (neighbour.timesteps.Contains(current.currentTimeStep + 2))
                        {
                            continue;
                        }
                        current.child = neighbour;

                        neighbour.parent = current;
                        neighbour.gCost = newCost;

                        neighbour.hCost = Heuristic(neighbour, _goal, current.currentTimeStep + 2);
                        neighbour.SetCurrentTimestep(current.currentTimeStep + 2);
                    }
                    else
                    {
                        current.child = neighbour;

                        neighbour.parent = current;
                        neighbour.gCost = newCost;

                        neighbour.hCost = Heuristic(neighbour, _goal, current.currentTimeStep + 1);
                        neighbour.SetCurrentTimestep(current.currentTimeStep + 1);

                    }
                    if (!open.Contains(neighbour))
                        open.Add(neighbour);
                    
                }
            }
        }
        Debug.Log("Found no path");
        return null;
    }

    float Heuristic(Node nod, Node goal, int timestep)
    {
        int defaultCost = GetDistance(nod, goal);
        if (nod.isEndPoint)
        {
            return 1000;
        }
        
        if (nod.timesteps != null && nod.safeTimesteps.Contains(timestep))
        {
            return nod.timesteps.Count * ((56 - defaultCost) * 2);
        }
        
        return defaultCost;
    }
    int longestPathCount = 0;
    List<Node> Path(Node start, Node goal, int startTime)
    {
        List<Node> newPath = new List<Node>();
        Node current = goal;
        
        int currenttime = goal.currentTimeStep;
        while (current.position != start.position && current != null)
        {
            newPath.Add(current);
            while (currenttime != current.currentTimeStep)
            {
                newPath.Add(current);
                currenttime--;
            }
            currenttime--;
            current = current.parent;
            if (current == null)

                break;
        }

        for (int i = 0; i < startTime; i++)
        {
            newPath.Add(start);
        }
        newPath.Reverse();
        
        for (int i = 0; i < newPath.Count; i++)
        {
            newPath[i].AddTimestep(i+1);
            newPath[i].AddSafeTimeStep(i + 2);
        }
        if(newPath.Count > longestPathCount)
        {
            longestPathCount = newPath.Count;
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
        neighbours.Add(map[node.xVal + 1, node.yVal]);
        neighbours.Add(map[node.xVal - 1, node.yVal]);
        neighbours.Add(map[node.xVal, node.yVal + 1]);
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
