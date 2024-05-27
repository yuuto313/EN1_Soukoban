using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewGameManagerScript : MonoBehaviour
{ 
    
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject clearText;
    public GameObject resetText;
    public GameObject particlePrefab;
    public GameObject goalPrefab;
    public GameObject clearParticlePrefab;
    public GameObject fieldPrefab;
    public GameObject wallPrefab;

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
        
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Wall")
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
        
        



        //�v���C���[�E���ւ�炸�̈ړ�����
        //GameObject�̍��W�iPosition�j���ړ������Ă���C���f�b�N�X�̓���ւ�
        Vector3 moveToPosition = IndexPosition(moveTo);
        field[moveFrom.y,moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
       
        return true;
    }

    //�񎟌��z��Ő錾

    Vector3 IndexPosition(Vector2Int index)
    {
        return new Vector3(-(map.GetLength(1) / 2 - index.x), -(map.GetLength(0) / 2 - index.y), 0);
    }
    /// <summary>
    /// �N���A����
    /// </summary>
    /// <returns></returns>
    bool IsCleard()
    {
        //Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int>goals=new List<Vector2Int>();

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x=0; x < map.GetLength(1); x++)
            {
                //�i�[�ꏊ���ۂ����f
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
                //��ł������Ȃ�������������B��
                return false;
            }
        }
        //�������B���łȂ���Ώ����B��
        return true;
    }
    /// <summary>
    /// �v���C���[�̈ړ��p�̃p�[�e�B�N��
    /// </summary>
    /// <param name="playerIndex"></param>
    void CleatePlayerParticle(Vector2Int playerIndex)
    {
        //�p�[�e�B�N���𐶐�����
        for (int i = 0; i < 7; i++)
        {
            Instantiate(particlePrefab,
            IndexPosition(playerIndex),
            Quaternion.identity
            );
        }
    }

    void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // �t�B�[���h��̃I�u�W�F�N�g��j��
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (field[y, x] != null)
                    {
                        Destroy(field[y, x]);
                    }
                }
            }

            // �t�B�[���h�̏�����
            Start();
            clearText.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //�𑜓x�ƃE�B�h�E���[�h�ݒ�
        Screen.SetResolution(1280, 720, false);

        //�P���v���C���[�A�Q�𔠁A�R���S�[���Ƃ���
        map = new int[,] {
            {4,4,4,4,4,4,4,4,4,4,4,4},
            {4,0,0,0,0,0,0,0,0,0,0,4},
            {4,0,0,3,0,0,0,0,3,0,0,4},
            {4,0,4,4,0,0,0,0,4,4,0,4},
            {4,0,0,0,0,0,1,0,0,0,0,4},
            {4,0,0,0,0,0,0,0,0,0,0,4},
            {4,0,2,0,0,0,0,0,0,2,0,4},
            {4,0,0,0,0,0,0,0,0,0,0,4},
            {4,4,4,4,4,4,4,4,4,4,4,4},
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
                        IndexPosition(new Vector2Int(x,y)),
                        Quaternion.identity
                        );                  
                };

                //box����
                if (map[y,x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        IndexPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                        );
                }

                //�S�[������
                if (map[y, x] == 3)
                {
                    Instantiate(
                        goalPrefab,
                        IndexPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                        );
                
                }

                //�ǐ���
                if (map[y, x] == 4)
                {
                    field[y,x] = Instantiate(
                        wallPrefab,
                        IndexPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                        );

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
        Reset();
        resetText.SetActive(true);

        //�E�L�[���������u��
        //Vector2Int playerIndex = GetPlayerIndex();
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            //�ړ��������֐���
            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));

            //�p�[�e�B�N���𐶐�����
            CleatePlayerParticle(playerIndex);
            //�N���A����
            if (IsCleard())
            {
                Debug.Log("Clear");
                clearText.SetActive(true);
       
            }
        }

        //���L�[���������u��

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));

            //�p�[�e�B�N���𐶐�����
            CleatePlayerParticle(playerIndex);
            //�N���A����
            if (IsCleard())
            {
                Debug.Log("Clear");
                clearText.SetActive(true);
               
            }

        }

        //��L�[���������u��

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));

            //�p�[�e�B�N���𐶐�����
            CleatePlayerParticle(playerIndex);
            //�N���A����
            if (IsCleard())
            {
                Debug.Log("Clear");
                clearText.SetActive(true);
               
            }

        }

        //���L�[���������u��

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));

            //�p�[�e�B�N���𐶐�����
            CleatePlayerParticle(playerIndex);
            //�N���A����
            if (IsCleard())
            {
                Debug.Log("Clear");
                clearText.SetActive(true);
              
            }

        }
    }
}
