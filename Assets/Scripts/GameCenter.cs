using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenter : MonoBehaviour
{
    enum State { INIT, START, PLAYERTURN, ENEMYTURN, CLEAR}
    GameObject[] tiles;
    MapData mapData;
    State state;
    int curStage;

    
    // Start is called before the first frame update
    void Start()
    {
        //���������� �ٲ�� mapdata�� stageinfo ����Ʈ�� �ش� ��� ���� �о�ͼ� �������� ����
        LoadMapData();
        InitTiles();
        curStage = 1;
        state = State.START;
        Debug.Log("state : START");
        EventManager.instance.OnEventTriggered += HandleEvent;
    }
    private void OnDisable()
    {
        EventManager.instance.OnEventTriggered -= HandleEvent;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.START:
                InitStage();
                break;
            case State.PLAYERTURN:
                
                 StartCoroutine(PlayerTurn());
                
                //�÷��̾� �� �ڷ�ƾ
                break;
            case State.ENEMYTURN:
                
                 StartCoroutine(EnemyTurn());
                
                //�� �� �ڷ�ƾ
                break;
        }
    }
    void LoadMapData()
    {
        DataManager.instance.LoadData();
        mapData = DataManager.instance.nowData;
    }
    void InitTiles()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
    }

   
    void InitStage()
    {
        //mapData�� �ִ� ���� �о�ͼ� ���� instantiate
        //�÷��̾��� ��ġ �ʱ�ȭ
        //�÷��̾��� ü�� �ʱ�ȭ
        //���� ���� �� �÷��̾� ������
        state = State.PLAYERTURN;
        Debug.Log("state : PLAYERTURN");

    }
    IEnumerator PlayerTurn()
    {
        int moveCnt = 2;
        int attackCnt = 1;
        //Debug.Log("Now in Player's Turn");

        //���̽�ƽ �����ӿ� ���� �÷��̾� �̵�
        //�÷��̾� ����
        yield return null;
    }

    void PlayerTurnEnd()
    {
        //�÷��̾� �� ���� �� ȣ��
        state = State.ENEMYTURN;
    }
    IEnumerator EnemyTurn()
    {
        //�� �ϳ����� �� ã��, �̵�
        //�� �ϳ����� ����
        yield return null;
    }

    void EnemyTurnEnd()
    {
        //�� �� ���� �� ȣ��
        state = State.PLAYERTURN;
    }
    void StageClear()
    {
        //Ŭ���������� ���� ����������
        curStage++;
        state = State.START;
    }
    public void TurnEndButtonClick()
    {
        state = State.ENEMYTURN;
    }
    private void HandleEvent(EventType eventType)
    {
        if (eventType == EventType.UIJoystickUp)
        {
            Debug.Log("Up");
        }
        else if (eventType == EventType.UIJoystickDown)
        {
            Debug.Log("Down");
        }
        else if (eventType == EventType.UIJoystickLeft)
        {
            Debug.Log("Left");
        }
        else if (eventType == EventType.UIJoystickRight)
        {
            Debug.Log("Right");
        }
    }

}
