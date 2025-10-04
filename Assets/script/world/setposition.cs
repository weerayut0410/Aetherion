using UnityEngine;

public class SetPositionAndRotation : MonoBehaviour // ����¹���� Script ������ͤ��������ҡ���
{
    private Vector3 initialPosition;    // ����Ѻ�纵��˹��������
    private Quaternion initialRotation; // ����Ѻ�纤�ҡ����ع�������

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �纵��˹觻Ѩ�غѹ�ͧ GameObject �����������������
        initialPosition = transform.position;

        // �纤�ҡ����ع�Ѩ�غѹ�ͧ GameObject �����������������
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // �ѧ�Ѻ�����˹觢ͧ GameObject ��Ѻ���������˹�������鹵�ʹ����
        transform.position = initialPosition;

        // �ѧ�Ѻ�������ع�ͧ GameObject ��Ѻ���������������鹵�ʹ����
        transform.rotation = initialRotation;
    }
}