using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //public void MoveTo(Vector3 destination)
    //{
    //    transform.position = destination;
    //}

    //�ړ��A�j���[�V����
    //�����܂łɂ����鎞��
    private float timeTaken = 0.125f;
    //�o�ߎ���
    private float timeErapsed;
    //�ړI�n
    private Vector3 destination;
    //�o���n
    private Vector3 origin;


    // Start is called before the first frame update
    private void Start()
    {
        //�ړI�n�E�o���n�����ݒn�ŏ�����
        destination = transform.position;
        origin = destination;
    }

    public void MoveTo(Vector3 newDestination)
    {
        //�o�ߎ��Ԃ�������
        timeErapsed = 0;
        //�ړ����̉\��������̂ŁA���ݒn��podition�ɑO��ړ��̖ړI�n����
        origin = destination;
        transform.position = origin;
        //�V�����ړI�n����
        destination = newDestination;
    }

    // Update is called once per frame
    private void Update()
    {
        //�ړI�n�ɓ��������珈�����Ȃ�
        if (origin == destination)
        {
            return;
        }
        //���Ԍo�߂����Z
        timeErapsed += Time.deltaTime;
        //�o�ߎ��Ԃ��������Ԃ̉��������Z�o
        float timeRate = timeErapsed / timeTaken;
        //�������Ԃ𒴂���悤�ł���Ύ��s�������ԑ����Ɋۂ߂�
        if (timeRate > 1)
        {
            timeRate = 1;
        }
        //�C�[�W���O�p�v�Z�i���j�A�j
        float easing = timeRate;
        //���W���Z�o
        Vector3 currentPosition = Vector3.Lerp(origin, destination, easing);
        //�Z�o�������W��position�ɑ��
        transform.position = currentPosition;

    }
}
