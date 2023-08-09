using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
    
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if(!moving)
        {
            FindSelectableTiles();
            CheckMouse();
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
                    if (t.Selectable)
                    {
                        //target�� �̵�
                        MoveToTile(t);
                    }
                }
            }
        }
    }

    

    
}
