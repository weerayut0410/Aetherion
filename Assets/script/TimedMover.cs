using UnityEngine;
using System;

public class TimedMover : MonoBehaviour
{
    private Transform target;
    private float speed;
    private Action onHit;

    // ������͡�����
    private bool lockY = false;
    private float fixedY;        // ��ҷ�����͡᡹ Y ���
    private bool destroyIfNoTarget = true;

    /// <summary>
    /// target: ������·���������
    /// speed: �������� (˹���/�Թҷ�)
    /// onHit: ���¡�͹�֧�������
    /// lockY: true ���������˹� Y �����
    /// fixedY: ��ҷ�����͡᡹ Y (�� Y �ͧ������µ͹�ԧ)
    /// destroyIfNoTarget: ��� target ��� ������µ���ͧ
    /// </summary>
    public void Init(Transform target, float speed, Action onHit, bool lockY = false, float fixedY = 0f, bool destroyIfNoTarget = true)
    {
        this.target = target;
        this.speed = speed;
        this.onHit = onHit;
        this.lockY = lockY;
        this.fixedY = fixedY;
        this.destroyIfNoTarget = destroyIfNoTarget;
    }

    void Update()
    {
        if (target == null)
        {
            if (destroyIfNoTarget) Destroy(gameObject);
            return;
        }

        // �ӹǳ���˹觻��·ҧ����͡᡹ Y (��ͧ�ѹ��/���)
        Vector3 targetPos = target.position;
        if (lockY) targetPos.y = fixedY;

        // ��Ѻ�����
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // �ѧ�Ѻ��� Y ����������ҧ�ҧ���� (�ѹ�����觨ҡ���ԡ��/͹���)
        if (lockY)
        {
            var p = transform.position;
            p.y = fixedY;
            transform.position = p;
        }

        // �֧�������?
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            onHit?.Invoke();
            Destroy(gameObject);
        }
    }
}
