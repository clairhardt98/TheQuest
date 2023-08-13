using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //타일의 속성
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    //인접 타일의 리스트
    public List<Tile> adjacencyList = new List<Tile>();

    //bfs 속성
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    //a* 속성
    public float f = 0;
    public float g = 0;
    public float h = 0;
    void Start()
    {
    }

    void Update()
    {
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (selectable)
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

        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;
    }
    public void FindNeighbors(Tile target)
    {
        Reset();

        CheckTile(Vector3.forward, target);
        CheckTile(-Vector3.forward, target);
        CheckTile(Vector3.right, target);
        CheckTile(-Vector3.right, target);
    }

    public void CheckTile(Vector3 direction, Tile Target)
    {
        Vector3 halfExtents = new Vector3(0.25f,0, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;
                //위쪽 방향으로 오브젝트가 없다면 인접 리스트에 입력
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == Target))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }
}
