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
        //���������� �ٲ�� mapdata�� stageinfo ����Ʈ�� �ش� ��� ���� �о�ͼ� �������� ����
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
        
        
        //���̽�ƽ �����ӿ� ���� �÷��̾� �̵�
        //�÷��̾� ����
        yield return null;
    }

    void PlayerTurnEnd()
    {
        //�÷��̾� �� ���� �� ȣ��
        state = State.ENEMYTURN;
        ButtonTurnEnd.GetComponent<Button>().interactable = false;
        Player.GetComponent<PlayerMove>().turn = false;
        Enemy.GetComponent<EnemyMove>().turn = true;
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
        //Ŭ���������� ���� ����������
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
