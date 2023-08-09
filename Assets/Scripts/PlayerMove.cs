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
                    if (t.Selectable)
                    {
                        //target을 이동
                        MoveToTile(t);
                    }
                }
            }
        }
    }

    

    
}
