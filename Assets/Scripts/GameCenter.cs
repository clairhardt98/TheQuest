using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCenter : MonoBehaviour
{
    enum State { INIT, START, PLAYERTURN, ENEMYTURN, CLEAR}
    GameObject[] tiles;
    MapData mapData;
    State state;
    int curStage;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject Enemy;
    //private List<GameObject> EnemyList;
    [SerializeField]
    private Button ButtonTurnEnd;

    int playerMoveCnt;
    int playerAttackCnt;

    
    // Start is called before the first frame update
    void Start()
    {
        //스테이지가 바뀌면 mapdata의 stageinfo 리스트의 해당 요소 값을 읽어와서 스테이지 세팅
        LoadMapData();
        InitTiles();
        curStage = 1;
        state = State.START;
        Debug.Log("state : START");
        playerMoveCnt = 2;
        playerAttackCnt = 1;

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
                Player.GetComponent<PlayerMove>().turn = true;
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
        
        
        //조이스틱 움직임에 따라 플레이어 이동
        //플레이어 공격
        yield return null;
    }

    void PlayerTurnEnd()
    {
        //플레이어 턴 종료 시 호출
        state = State.ENEMYTURN;
        ButtonTurnEnd.GetComponent<Button>().interactable = false;
        Player.GetComponent<PlayerMove>().turn = false;
        Enemy.GetComponent<EnemyMove>().turn = true;
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
        Debug.Log("State : PLAYERTURN");
        state = State.PLAYERTURN;
        ButtonTurnEnd.GetComponent<Button>().interactable = true;
        Player.GetComponent<PlayerMove>().turn = true;
        playerMoveCnt = 2;
        playerAttackCnt = 1;
        Enemy.GetComponent<EnemyMove>().turn = false;
    }
    void StageClear()
    {
        //클리어했으면 다음 스테이지로
        curStage++;
        state = State.START;
    }
    public void TurnEndButtonClick()
    {
        PlayerTurnEnd();
    }
    private void HandleEvent(EventType eventType)
    {
        if(state == State.PLAYERTURN && playerMoveCnt>0)
        {
            if (eventType == EventType.UIJoystickUp)
            {
                if (Player.GetComponent<PlayerMove>().CheckCanMove(Vector3.forward))
                    playerMoveCnt--;
            }
            else if (eventType == EventType.UIJoystickDown)
            {
                if (Player.GetComponent<PlayerMove>().CheckCanMove(-Vector3.forward))
                    playerMoveCnt--;
            }
            else if (eventType == EventType.UIJoystickLeft)
            {
                if (Player.GetComponent<PlayerMove>().CheckCanMove(-Vector3.right))
                    playerMoveCnt--;
            }
            else if (eventType == EventType.UIJoystickRight)
            {
                if (Player.GetComponent<PlayerMove>().CheckCanMove(Vector3.right))
                    playerMoveCnt--;
            }
        }
        if (eventType == EventType.EnemyTurnEnd)
        {
            EnemyTurnEnd();
        }
    }
}
