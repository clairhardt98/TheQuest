using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
    public enum Direction { UP,DOWN,LEFT,RIGHT}
    public bool turn;
    
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!turn)
            return;
        if(!moving)
        {
            FindSelectableTiles();
            //Debug.Log(selectableTiles.Count);
            //CheckMouse();
        }
        else
        {
            Move();
        }
    }
    void CheckMouse()
    {

        //클릭해서 selectable 타일 값 받아오기
        if (Input.GetMouseButtonUp(0))
        {
            //좌클릭시 클릭한 시점에서 레이 발생
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    //부딛힌 콜라이더가 타일이면
                    Tile t = hit.collider.GetComponent<Tile>();
                    if (t.selectable)
                    {
                        //target을 이동
                        //MoveToTile(t);
                        //해당 타일을 체크해서 타일 위에 적이 있다면 공격하는 로직
                    }
                }
            }
        }
    }

    public bool CheckCanMove(Vector3 dir)
    {
        Tile playerTile = GetTargetTile(this.gameObject);
        Vector3 halfExtents = new Vector3(0.25f, 0, 0.25f);
        Collider[] colliders = Physics.OverlapBox(playerTile.transform.position + dir, halfExtents);
        //Debug.Log(colliders.Length);
        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                Debug.Log(colliders.Length);
                MoveToTile(tile);
                return true;
            }
        }
        return false;
    }

    public void Attack()
    {

    }
    IEnumerator Attack(Tile tile)
    {

        yield return new WaitForSeconds(0.1f);
    }
}
