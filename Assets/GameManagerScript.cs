using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewGameManagerScript : MonoBehaviour
{ 
    
    public GameObject playerPrefab;
    public GameObject boxPrefab;

    //���x���f�U�C���p�̔z��
    int[,] map;

    //�Q�[���Ǘ��p�̔z��
    GameObject[,] field;

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                //null��������^�O�𒲂ׂ����̗v�f�ֈڂ�
                if (field[y, x] == null)
                {
                    continue;
                    //null��������continue���Ă���̂ł��̍s�ɂ��ǂ蒅���ꍇ��null�o�Ȃ����Ƃ��m��
                    //�^�O�̊m�F���s��
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
    /// �z��̐������ړ�������
    /// </summary>
    /// <param name="number"></param>
    /// <param name="moveFrom"></param>
    /// <param name="moveTo"></param>
    /// <returns></returns>
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            //�����Ȃ��������ɏ����A�������^�[������
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            return false;
        }

        //2�𓮂�������
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            //�z��O�Q�Ɩh�~
            //box�^�O�������Ă�����ċA����
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber("Box", moveTo, moveTo + velocity);
            if (!success) { return false; }
        }

        //if (map[moveTo.y, moveTo.x] == 2)
        //{
        //    //�ǂ̕����ֈړ����邩���Z�o
        //    Vector2Int velocity = moveTo - moveFrom;
        //    //�v���C���[�̈ړ��悩��A����ɐ�ւQ�i���j���ړ�������
        //    //���̈ړ������BMoveNumber���\�b�h����MoveNumber���\�b�h���Ăя������ċN���Ă���B�ړ��s��Bool�ŋL�^
        //    bool success = MoveNumber("Box", moveTo, moveTo + velocity);
        //    //���������ړ����s������A�v���C���[�̈ړ������s
        //    if (!success) { return false; }
        //}


        //�v���C���[�E���ւ�炸�̈ړ�����
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        //GameObject�̍��W�iPosition�j���ړ������Ă���C���f�b�N�X�̓���ւ�
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveFrom.x, map.GetLength(0) - moveFrom.y, 0);

        return true;
    }

    //�񎟌��z��Ő錾

    Vector3 IndexPosition(Vector2Int index)
    {
        return new Vector3(index.x - map.GetLength(1) / 2 + 0.5f, index.y - map.GetLength(0) / 2, 0);
    }


    // Start is called before the first frame update
    void Start()
    {
        // GameObject instance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        

        //�P���v���C���[�A�Q�𔠂Ƃ���
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

        //��Afor���œ񎟌��z��̏����o��

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";

                //�v���C���[����
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                };

                //box����
                if (map[y,x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity);
                }

            }
            //���s
            debugText += "\n";
        }

        Debug.Log(debugText);
    }


    // Update is called once per frame
    void Update()
    {
        //�E�L�[���������u��

        if (Input.GetKeyDown(KeyCode.RightArrow))
        { 
            Vector2Int playerIndex = GetPlayerIndex();
            //�ړ��������֐���
            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
        }

        ////���L�[���������u��

        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
           
        //    MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));
        //}
        
        ////��L�[���������u��

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
           
        //    MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));
        //}
        
        ////���L�[���������u��

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
            
        //    MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
        //}
    }
}
