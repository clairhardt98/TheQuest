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
            FindNearestTarget();//�÷��̾� ã��
            CalculatePath();//��� ���
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
        //�÷��̾ Ÿ������ �ؼ� ��� Ž��
        Tile targetTile = GetTargetTile(Target);
        
        FindPath(targetTile);
    }
    void FindNearestTarget()
    {
        //�÷��̾� �±��� Ÿ���� ã�´�
        Target = GameObject.FindGameObjectWithTag("Player");
        //float distance = Vector3.Distance(transform.position, Target.transform.position);
    }
}
