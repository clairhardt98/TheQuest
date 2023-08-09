using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public struct EnemyInfo
{
    public int i;
    public int j;
    public string enemyType;
    public EnemyInfo(int i, int j, string enemyType)
    {
        this.i = i;
        this.j = j;
        this.enemyType = enemyType;
    }
}
[Serializable]
public struct ItemInfo
{
    public int i;
    public int j;
    public string itemType;
    public ItemInfo(int i, int j, string itemType)
    {
        this.i = i;
        this.j = j;
        this.itemType = itemType;
    }
}
[Serializable]
public struct StageInfo
{
    public int EnemyCount;
    public List<EnemyInfo> enemyInfoList;
    public List<ItemInfo> itemInfoList;
}
public class MapData
{
    //타일의 어느 인덱스에 어떤 적이 있는지에 대한 정보
    public int StageCnt;
    public List<StageInfo> stageInfoList;
}
public class DataManager : MonoBehaviour
{
    //싱글톤 구현
    public static DataManager instance;

    MapData StageData = new MapData();
    public MapData nowData = new MapData();
    
    string path;
    string filename = "save";
    private void Awake()
    {
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion
        path = Application.persistentDataPath + "/";
    }
    void Start()
    {
        SetStageInfo();
        SaveData();
    }
    void SetStageInfo()
    {
        //map data set, 3개의 스테이지 정보
        StageData.StageCnt = 3;
        StageData.stageInfoList = new List<StageInfo>();

        EnemyInfo enemyInfo1;
        EnemyInfo enemyInfo2;
        EnemyInfo enemyInfo3;

        ItemInfo item1Info;
        ItemInfo item2Info;

        //stage1 적 1, 아이템 0
        StageInfo stage1Info = new StageInfo();
        stage1Info.EnemyCount = 1;
        stage1Info.enemyInfoList = new List<EnemyInfo>();
        enemyInfo1 = new EnemyInfo(1, 2, "bat");
        stage1Info.enemyInfoList.Add(enemyInfo1);

        StageData.stageInfoList.Add(stage1Info);

        //stage2 적 2, 아이템 1
        StageInfo stage2Info = new StageInfo();
        stage2Info.EnemyCount = 2;
        stage2Info.enemyInfoList = new List<EnemyInfo>();
        stage2Info.itemInfoList = new List<ItemInfo>();
        enemyInfo1 = new EnemyInfo(2, 3, "bat");
        enemyInfo2 = new EnemyInfo(5, 8, "ghost");
        stage2Info.enemyInfoList.Add(enemyInfo1);
        stage2Info.enemyInfoList.Add(enemyInfo2);
        item1Info = new ItemInfo(3,3,"bow");
        stage2Info.itemInfoList.Add(item1Info);
        
        StageData.stageInfoList.Add(stage2Info);

        //stage3 적 3, 아이템 2
        StageInfo stage3Info = new StageInfo();
        stage3Info.EnemyCount = 3;
        stage3Info.enemyInfoList = new List<EnemyInfo>();
        stage3Info.itemInfoList = new List<ItemInfo>();
        enemyInfo1 = new EnemyInfo(1, 9, "bat");
        enemyInfo2 = new EnemyInfo(8, 7, "ghost");
        enemyInfo3 = new EnemyInfo(7, 5, "ghoul");
        stage3Info.enemyInfoList.Add(enemyInfo1);
        stage3Info.enemyInfoList.Add(enemyInfo2);
        stage3Info.enemyInfoList.Add(enemyInfo3);
        item1Info = new ItemInfo(4, 4, "mace");
        item2Info = new ItemInfo(10, 9, "potion");
        stage3Info.itemInfoList.Add(item1Info);
        stage3Info.itemInfoList.Add(item2Info);

        StageData.stageInfoList.Add(stage3Info);
    }
    

    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(StageData);
        File.WriteAllText(path + filename, jsonData);
    }
    public void LoadData()
    {
        string data = File.ReadAllText(path + filename);
        nowData = JsonUtility.FromJson<MapData>(data);
    }
}
