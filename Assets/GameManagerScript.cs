using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewGameManagerScript : MonoBehaviour
{ 
    
    public GameObject playerPrefab;
    public GameObject boxPrefab;

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

        //if (map[moveTo.y, moveTo.x] == 2)
        //{
        //    //どの方向へ移動するかを算出
        //    Vector2Int velocity = moveTo - moveFrom;
        //    //プレイヤーの移動先から、さらに先へ２（箱）を移動させる
        //    //箱の移動処理。MoveNumberメソッド内でMoveNumberメソッドを呼び処理が再起している。移動不可をBoolで記録
        //    bool success = MoveNumber("Box", moveTo, moveTo + velocity);
        //    //もし箱が移動失敗したら、プレイヤーの移動も失敗
        //    if (!success) { return false; }
        //}


        //プレイヤー・箱関わらずの移動処理
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        //GameObjectの座標（Position）を移動させてからインデックスの入れ替え
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveFrom.x, map.GetLength(0) - moveFrom.y, 0);

        return true;
    }

    //二次元配列で宣言

    Vector3 IndexPosition(Vector2Int index)
    {
        return new Vector3(index.x - map.GetLength(1) / 2 + 0.5f, index.y - map.GetLength(0) / 2, 0);
    }


    // Start is called before the first frame update
    void Start()
    {
        // GameObject instance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        

        //１をプレイヤー、２を箱とする
        map = new int[,] {
           { 0, 0, 0, 0, 0 },
           { 0, 2, 1, 0, 0 },
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
                        Quaternion.identity);
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

        if (Input.GetKeyDown(KeyCode.RightArrow))
        { 
            Vector2Int playerIndex = GetPlayerIndex();
            //移動処理を関数化
            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
        }

        ////左キーを押した瞬間

        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
           
        //    MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));
        //}
        
        ////上キーを押した瞬間

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
           
        //    MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));
        //}
        
        ////下キーを押した瞬間

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
            
        //    MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
        //}
    }
}
