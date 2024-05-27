using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewGameManagerScript : MonoBehaviour
{ 
    
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject clearText;
    public GameObject particlePrefab;

    //レベルデザイン用の配列
    int[,] map;

    //ゲーム管理用の配列
    GameObject[,] field;

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                //nullだったらタグを調べず次の要素へ移る
                if (field[y, x] == null)
                {
                    continue;
                    //nullだったらcontinueしているのでこの行にたどり着く場合はnull出ないことが確定
                    //タグの確認を行う
                }
                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    /// <summary>
    /// 配列の数字を移動させる
    /// </summary>
    /// <param name="number"></param>
    /// <param name="moveFrom"></param>
    /// <param name="moveTo"></param>
    /// <returns></returns>
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            //動けない条件を先に書き、早期リターンする
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            return false;
        }

        //2を動かす処理
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            //配列外参照防止
            //boxタグを持っていたら再帰処理
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber("Box", moveTo, moveTo + velocity);
            if (!success) { return false; }
        }


        //プレイヤー・箱関わらずの移動処理
        //GameObjectの座標（Position）を移動させてからインデックスの入れ替え
        //field[moveFrom.y, moveFrom.x].transform.position = new Vector3(x, map.GetLength(0) - y, 0);
        Vector3 moveToPosition = new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
        field[moveFrom.y,moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
       
        return true;
    }

    //二次元配列で宣言

    Vector3 IndexPosition(Vector2Int index)
    {
        return new Vector3(index.x - map.GetLength(1) / 2 + 0.5f, index.y - map.GetLength(0) / 2, 0);
    }
    /// <summary>
    /// クリア判定
    /// </summary>
    /// <returns></returns>
    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int>goals=new List<Vector2Int>();

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x=0; x < map.GetLength(1); x++)
            {
                //格納場所か否か判断
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for(int i=0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y,goals[i].x];
            if(f == null || f.tag != "Box")
            {
                //一つでも箱がなかったら条件未達成
                return false;
            }
        }
        //条件未達成でなければ条件達成
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //解像度とウィドウモード設定
        Screen.SetResolution(1280, 720, false);


        //１をプレイヤー、２を箱とする
        map = new int[,] {
           { 0, 0, 0, 0, 0 },
           { 0, 3, 1, 3, 0 },
           { 0, 0, 2, 0, 0,},
           { 0, 2, 3, 2, 0,},
           { 0, 0, 0, 0, 0,},
        };

        field = new GameObject
        [
           map.GetLength(0),
           map.GetLength(1)
        ];

        string debugText = "";

        //二連for文で二次元配列の情報を出力

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";

                //プレイヤー生成
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);//パーティクルを生成する
                   
                };

                //box生成
                if (map[y,x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                }

            }
            //改行
            debugText += "\n";
        }

        

        Debug.Log(debugText);
    }


    // Update is called once per frame
    void Update()
    {
        //右キーを押した瞬間
        //Vector2Int playerIndex = GetPlayerIndex();
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //移動処理を関数化
            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
            if (IsCleard())
            {
                Debug.Log("Clear");
                clearText.SetActive(true);
            }
        }

        //左キーを押した瞬間

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));
            if (IsCleard())
            {
                Debug.Log("Clear");
                clearText.SetActive(true);
            }
        }

        //上キーを押した瞬間

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
            if (IsCleard())
            {
                Debug.Log("Clear");
                clearText.SetActive(true);
            }
        }

        //下キーを押した瞬間

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));
            if (IsCleard())
            {
                Debug.Log("Clear");
                clearText.SetActive(true);
            }
        }
    }
}
