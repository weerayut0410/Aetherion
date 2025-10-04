using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class monsterFollow : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;
    Animator animator;
    bool isAttacking = false; // �ѹ�����ʶҹ���� Animator ���ѧ��蹷�����������������
    public GameObject weapon;
    Collider childCollider;

    public float attack = 0; // ����� '��������' ������س�����
    public float initialAttackDelay = 2f; // �������������Ѻ˹�ǧ��������������� (2 �Թҷ�)
    private float attackDelayTimer; // ��ǹѺ���Ҷ����ѧ����Ѻ���˹�ǧ����
    private bool isInAttackRangeAndWaiting = false; // ʶҹ����ͺ͡������������������С��ѧ�͡��˹�ǧ����

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // ��Ǩ�ͺ��� weapon ��� Collider �ͧ weapon �դ���������
        if (weapon != null)
        {
            childCollider = weapon.GetComponent<Collider>();
            if (childCollider != null)
            {
                childCollider.enabled = false;
            }
            else
            {
                Debug.LogWarning("Weapon GameObject does not have a Collider component assigned!", weapon);
            }
        }
        else
        {
            Debug.LogWarning("Weapon GameObject is not assigned in the Inspector for " + gameObject.name);
        }

        // ��˹����������鹢ͧ Timer ���������ҹ�ѹ��������������������դ����á
        attackDelayTimer = initialAttackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ǩ�ͺ��� player �դ��������� ��͹���Фӹǳ distance
        if (player == null)
        {
            Debug.LogWarning("Player GameObject is not assigned to monsterFollow script on " + gameObject.name);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);
        animator.SetFloat("Distance", distance);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // ��Ǩ�ͺ��� State �Ѩ�غѹ��� "attack" ������� (����� State � Animator �ͧ�س)
        // ��Ǩ�ͺ���� State ���ç�Ѻ Animator �ͧ�س (���Ԩ��繵���˭����͵����硵�����س���)
        isAttacking = stateInfo.IsName("attack"); // ������ҧ: ��Ҫ��� State ��� "Attack"

        // ��Ǩ�ͺ��� childCollider �դ��������� ��͹������ҹ
        if (childCollider == null)
        {
            if (weapon != null)
            {
                childCollider = weapon.GetComponent<Collider>();
            }
            if (childCollider == null)
            {
                Debug.LogError("Child Collider for weapon is null. Cannot enable/disable it.");
                return; // �͡�ҡ Update ���ͻ�ͧ�ѹ NullReferenceException
            }
        }

        // �ѹ˹���� player ��ʹ�����������������еԴ�����������
        // ��÷ӡ�͹���͹����� ��������ѹ˹���������ѧ�������Թ
        transform.LookAt(player.transform);


        // --- ��áС��������С��˹�ǧ���� ---
        if (distance <= attack) // ���������������� (�� 'attack' ������)
        {
            agent.speed = 0; // ��ش�������͹���

            // �������������������� �����ѧ�������������ѧ�����������Ѻ�����ѧ
            if (!isInAttackRangeAndWaiting)
            {
                isInAttackRangeAndWaiting = true; // ��駤����ҡ��ѧ������������������
                attackDelayTimer = initialAttackDelay; // ������Ѻ�����ѧ����
                animator.SetBool("attack", false); // ��Ǩ�ͺ������������������͹����ѹ����㹢�з����
                childCollider.enabled = false; // �Դ Collider ���ظ㹢�з����
            }

            // �Ѻ�����ѧ
            if (attackDelayTimer > 0)
            {
                attackDelayTimer -= Time.deltaTime; // Ŵ����
                // Debug.Log($"Waiting to attack... {attackDelayTimer:F2} seconds left."); // ������պ��
            }
            else // �������˹�ǧ������� (���������)
            {
                animator.SetBool("attack", true); // ���������͹����ѹ����
                if(stateInfo.IsName("attack") && stateInfo.normalizedTime >= 0.5f)
                {
                    childCollider.enabled = true;
                }
            }
        }
        // �����������еԴ��� (������������������)
        // ����͹������������ѧ����͹����ѹ��������
        else if (distance < 20f && !isAttacking)
        {
            // ����ʶҹС��������������͡�ҡ����
            isInAttackRangeAndWaiting = false;
            attackDelayTimer = initialAttackDelay; // ���� timer ��Ѻ令���������

            childCollider.enabled = false;
            animator.SetBool("attack", false); // �������͹����ѹ����
            agent.speed = 5;
            agent.SetDestination(player.transform.position);
        }
        // �������͡���еԴ��������������
        else if (distance > 20f)
        {
            // ����ʶҹС��������������͡�ҡ����
            isInAttackRangeAndWaiting = false;
            attackDelayTimer = initialAttackDelay; // ���� timer ��Ѻ令���������

            childCollider.enabled = false;
            animator.SetBool("attack", false); // �������͹����ѹ����
            agent.speed = 0;
        }
    }
}