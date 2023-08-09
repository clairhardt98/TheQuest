using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

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
        currentTile.Current = true;
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

    public void ComputeAdjacencyList()
    {
        //tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors();
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjacencyList();
        GetCurrentTile();

        //bfs
        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.Parent = 

        while(process.Count>0)
        {
            Tile t = process.Dequeue();
            selectableTiles.Add(t);
            t.Selectable = true;

            if(t.Distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.Parent = t;
                        tile.visited = true;
                        tile.Distance = 1 + t.Distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.Target = true;
        moving = true;

        Tile next = tile;
        while(next != null)
        {
            //스택에 경로를 입력
            path.Push(next);
            next = next.Parent;
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
        }
    }

    protected void RemoveSelectableTiles()
    {
        if(currentTile != null)
        {
            currentTile.Current = false;
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
}
