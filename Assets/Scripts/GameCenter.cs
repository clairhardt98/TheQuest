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
        //스테이지가 바뀌면 mapdata의 stageinfo 리스트의 해당 요소 값을 읽어와서 스테이지 세팅
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
                
                //플레이어 턴 코루틴
                break;
            case State.ENEMYTURN:
                
                 StartCoroutine(EnemyTurn());
                
                //적 턴 코루틴
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
        //mapData에 있는 값을 읽어와서 몬스터 instantiate
        //플레이어의 위치 초기화
        //플레이어의 체력 초기화
        //로직 종료 시 플레이어 턴으로
        state = State.PLAYERTURN;
        Debug.Log("state : PLAYERTURN");

    }
    IEnumerator PlayerTurn()
    {
        int moveCnt = 2;
        int attackCnt = 1;
        //Debug.Log("Now in Player's Turn");

        //조이스틱 움직임에 따라 플레이어 이동
        //플레이어 공격
        yield return null;
    }

    void PlayerTurnEnd()
    {
        //플레이어 턴 종료 시 호출
        state = State.ENEMYTURN;
    }
    IEnumerator EnemyTurn()
    {
        //적 하나마다 길 찾기, 이동
        //적 하나마다 공격
        yield return null;
    }

    void EnemyTurnEnd()
    {
        //적 턴 종료 시 호출
        state = State.PLAYERTURN;
    }
    void StageClear()
    {
        //클리어했으면 다음 스테이지로
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
