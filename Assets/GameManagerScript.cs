using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void PrintArray()
    {
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ",";
        }

        Debug.Log(debugText);
    }

    int GetPlayerIndex()
    {
        for(int i = 0; i < map.Length; i++)
        {
            if (map[i] == i)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// �z��̐������ړ�������
    /// </summary>
    /// <param name="number"></param>
    /// <param name="moveFrom"></param>
    /// <param name="moveTo"></param>
    /// <returns></returns>
    bool MoveNumber(int number,int moveFrom,int moveTo)
    {
        if(moveTo < 0 || moveTo >= map.Length)
        {
            //�����Ȃ��������ɏ����A�������^�[������
            return false;
        }
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }
    

    int[] map = { };

    string debugText="";

    // Start is called before the first frame update
    void Start()
    {  
        //�P���v���C���[�A�Q�𔠂Ƃ���
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 0, 0 };
        PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
        int playerIndex = GetPlayerIndex();
        //�E�L�[���������u��
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1)
                {
                    playerIndex = i;
                    break;
                }
            }
            MoveNumber(1, playerIndex, playerIndex + 1);

            PrintArray();
        }

        //���L�[���������u��
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1)
                {
                    playerIndex = i;
                    break;
                }
            }
            MoveNumber(1, playerIndex, playerIndex - 1);

            PrintArray();
        }

    }

}
