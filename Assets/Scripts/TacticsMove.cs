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
    //Ž������
    public int move = 6;
    public float moveSpeed = 1.0f;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;
    protected void Init()
    {
        //�����ϸ� ���� ���� tile���� �� �о�ͼ� �迭�� ������
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    public void GetCurrentTile()
    {
        //���� Ÿ���� ���� Current�� true��
        currentTile = GetTargetTile(gameObject);
        currentTile.Current = true;
    }
    public Tile GetTargetTile(GameObject _target)
    {
        //�Ʒ��� ����ĳ��Ʈ�� ���� �� ���� Ÿ���� ����
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
            //���ÿ� ��θ� �Է�
            path.Push(next);
            next = next.Parent;
        }
    }

    public void Move()
    {
        if(path.Count > 0)
        {
            //�н��� ������ ���
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
                //Ÿ�ٰ� ����� ���������
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            //�н��� ������ selectableTiles�� remove
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
        //���� �ִ� ���� ����
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }
}
