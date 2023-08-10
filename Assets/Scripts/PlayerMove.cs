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

        //Ŭ���ؼ� selectable Ÿ�� �� �޾ƿ���
        if (Input.GetMouseButtonUp(0))
        {
            //��Ŭ���� Ŭ���� �������� ���� �߻�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    //�ε��� �ݶ��̴��� Ÿ���̸�
                    Tile t = hit.collider.GetComponent<Tile>();
                    if (t.selectable)
                    {
                        //target�� �̵�
                        //MoveToTile(t);
                        //�ش� Ÿ���� üũ�ؼ� Ÿ�� ���� ���� �ִٸ� �����ϴ� ����
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
