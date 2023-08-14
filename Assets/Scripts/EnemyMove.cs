using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : TacticsMove
{
    GameObject Target;
    public bool turn = false;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!turn)
        {
            return;
        }
        if (!moving)
        {
            FindNearestTarget();//플레이어 찾기
            CalculatePath();//경로 계산
            FindSelectableTiles();
            //actualTargetTile.target = true;
        }
        else
        {
            Move();
        }
    }
    void CalculatePath()
    {
        //플레이어를 타겟으로 해서 경로 탐색
        Tile targetTile = GetTargetTile(Target);
        
        FindPath(targetTile);
    }
    void FindNearestTarget()
    {
        //플레이어 태그의 타겟을 찾는다
        Target = GameObject.FindGameObjectWithTag("Player");
        //float distance = Vector3.Distance(transform.position, Target.transform.position);
    }
}
