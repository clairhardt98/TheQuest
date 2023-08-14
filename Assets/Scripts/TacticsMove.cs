using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;
    public Tile actualTargetTile;

    public bool moving = false;
    //탐지범위
    public int move = 6;
    public float moveSpeed = 1.0f;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;
    protected void Init()
    {
        //시작하면 게임 내의 tile들을 다 읽어와서 배열에 저장함
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    public void GetCurrentTile()
    {
        //현재 타일을 얻어내서 Current를 true로
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }
    public Tile GetTargetTile(GameObject _target)
    {
        //아래로 레이캐스트를 쏴서 내 밑의 타일을 얻어낸다
        RaycastHit hit;
        Tile tile = null;

        if(Physics.Raycast(_target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyList(Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(target);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyList(null);
        GetCurrentTile();

        //bfs
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while(process.Count>0)
        {
            Tile t = process.Dequeue();
            selectableTiles.Add(t);
            t.selectable = true;

            if(t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while(next != null)
        {
            //스택에 경로를 입력
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if(path.Count > 0)
        {
            //패스가 존재할 경우
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //Calculate the unit's position on top of the target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;
            if(Vector3.Distance(transform.position,target)>=0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //타겟과 충분히 가까워지면
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            //패스가 없으면 selectableTiles를 remove
            RemoveSelectableTiles();
            moving = false;
            if(this.CompareTag("Enemy"))
                EventManager.instance.TriggerEvent(EventType.EnemyTurnEnd);
        }
    }

    protected void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach(Tile tile in selectableTiles)
        {
            tile.Reset();
        }
        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        //가고 있는 방향 벡터
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach(Tile t in list)
        {
            if (t.f < lowest.f)
                lowest = t;
        }
        list.Remove(lowest);
        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        //경로 생성
        Tile next = t.parent;
        while(next!=null)
        {
            tempPath.Push(next);
            next = next.parent;
        }
        //움직일 수 있는 거리보다 경로 길이가 더 짧다면
        if(tempPath.Count<=move)
        {
            return t.parent;
        }
        Tile endTile = null;
        //더 길다면 그만큼 pop해서 이동
        for(int i = 0; i<=move;i++)
        {
            endTile = tempPath.Pop();
        }
        return endTile;
    }
    protected void FindPath(Tile target)
    {
        //타겟 주변의 인접 리스트를 갱신
        ComputeAdjacencyList(target);
        //현재 타일 갱신
        GetCurrentTile();

        //A*
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;
        while(openList.Count>0)
        {
            //pq에서 가장 작은 f뽑기
            Tile t = FindLowestF(openList);
            closedList.Add(t);

            if(t == target)
            {
                //target을 찾음
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }
            foreach (Tile tile in t.adjacencyList)
            {
                //인접 노드에 대해서
                if(closedList.Contains(tile))
                {
                    //아무것도 하지 않는다
                }
                else if(openList.Contains(tile))
                {
                    //openlist에 있을 때
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    if(tempG < tile.g)
                    {
                        tile.parent = t;
                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }
        //target까지 path가 없다면?
        Debug.Log("Path not Found");
    }
}
