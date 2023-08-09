using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //타일의 속성
    public bool Walkable = true;
    public bool Current = false;
    public bool Target = false;
    public bool Selectable = false;

    //인접 타일의 리스트
    public List<Tile> adjacencyList = new List<Tile>();

    //bfs 속성
    public bool visited = false;
    public Tile Parent = null;
    public int Distance = 0;

    void Start()
    {

    }

    void Update()
    {
        if (Current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (Target)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (Selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void Reset()
    {
        adjacencyList.Clear();

        Current = false;
        Target = false;
        Selectable = false;

        visited = false;
        Parent = null;
        Distance = 0;
    }
    public void FindNeighbors()
    {
        Reset();

        CheckTile(Vector3.forward);
        CheckTile(-Vector3.forward);
        CheckTile(Vector3.right);
        CheckTile(-Vector3.right);
    }

    public void CheckTile(Vector3 _direction)
    {
        Vector3 halfExtents = new Vector3(0.25f,0, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + _direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.Walkable)
            {
                RaycastHit hit;
                //위쪽 방향으로 오브젝트가 없다면 인접 리스트에 입력
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }
}
