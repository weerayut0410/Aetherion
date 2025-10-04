using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class monsterFollow : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;
    Animator animator;
    bool isAttacking = false; // อันนี้คือสถานะว่า Animator กำลังเล่นท่าโจมตีอยู่หรือไม่
    public GameObject weapon;
    Collider childCollider;

    public float attack = 0; // นี่คือ 'ระยะโจมตี' ตามที่คุณใช้เดิม
    public float initialAttackDelay = 2f; // เพิ่มตัวแปรสำหรับหน่วงเวลาโจมตีเริ่มต้น (2 วินาที)
    private float attackDelayTimer; // ตัวนับเวลาถอยหลังสำหรับการหน่วงเวลา
    private bool isInAttackRangeAndWaiting = false; // สถานะเพื่อบอกว่าอยู่ในระยะโจมตีและกำลังรอการหน่วงเวลา

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // ตรวจสอบว่า weapon และ Collider ของ weapon มีค่าหรือไม่
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

        // กำหนดค่าเริ่มต้นของ Timer ให้พร้อมใช้งานทันทีเมื่อเข้าสู่ระยะโจมตีครั้งแรก
        attackDelayTimer = initialAttackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // ตรวจสอบว่า player มีค่าหรือไม่ ก่อนที่จะคำนวณ distance
        if (player == null)
        {
            Debug.LogWarning("Player GameObject is not assigned to monsterFollow script on " + gameObject.name);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);
        animator.SetFloat("Distance", distance);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // ตรวจสอบว่า State ปัจจุบันคือ "attack" หรือไม่ (ใช้ชื่อ State ใน Animator ของคุณ)
        // ตรวจสอบชื่อ State ให้ตรงกับ Animator ของคุณ (ปกติจะเป็นตัวใหญ่หรือตัวเล็กตามที่คุณตั้ง)
        isAttacking = stateInfo.IsName("attack"); // ตัวอย่าง: ถ้าชื่อ State คือ "Attack"

        // ตรวจสอบว่า childCollider มีค่าหรือไม่ ก่อนที่จะใช้งาน
        if (childCollider == null)
        {
            if (weapon != null)
            {
                childCollider = weapon.GetComponent<Collider>();
            }
            if (childCollider == null)
            {
                Debug.LogError("Child Collider for weapon is null. Cannot enable/disable it.");
                return; // ออกจาก Update เพื่อป้องกัน NullReferenceException
            }
        }

        // หันหน้าหา player ตลอดเวลาเมื่ออยู่ในระยะติดตามหรือโจมตี
        // ควรทำก่อนเงื่อนไขระยะ เพื่อให้หันหน้าได้แม้กำลังรอหรือเดิน
        transform.LookAt(player.transform);


        // --- ตรรกะการโจมตีและการหน่วงเวลา ---
        if (distance <= attack) // ถ้าอยู่ในระยะโจมตี (ใช้ 'attack' ตามเดิม)
        {
            agent.speed = 0; // หยุดการเคลื่อนที่

            // ถ้าเพิ่งเข้าสู่ระยะโจมตี หรือยังอยู่ในระยะและยังไม่ได้เริ่มนับถอยหลัง
            if (!isInAttackRangeAndWaiting)
            {
                isInAttackRangeAndWaiting = true; // ตั้งค่าว่ากำลังอยู่ในระยะโจมตีและรอ
                attackDelayTimer = initialAttackDelay; // เริ่มนับถอยหลังใหม่
                animator.SetBool("attack", false); // ตรวจสอบให้แน่ใจว่าไม่ได้เล่นแอนิเมชันโจมตีในขณะที่รอ
                childCollider.enabled = false; // ปิด Collider อาวุธในขณะที่รอ
            }

            // นับถอยหลัง
            if (attackDelayTimer > 0)
            {
                attackDelayTimer -= Time.deltaTime; // ลดเวลา
                // Debug.Log($"Waiting to attack... {attackDelayTimer:F2} seconds left."); // เอาไว้ดีบั๊ก
            }
            else // ถ้าเวลาหน่วงหมดแล้ว (พร้อมโจมตี)
            {
                animator.SetBool("attack", true); // เริ่มเล่นแอนิเมชันโจมตี
                if(stateInfo.IsName("attack") && stateInfo.normalizedTime >= 0.5f)
                {
                    childCollider.enabled = true;
                }
            }
        }
        // ถ้าอยู่ในระยะติดตาม (แต่ไม่อยู่ในระยะโจมตี)
        // และมอนสเตอร์ไม่ได้กำลังเล่นแอนิเมชันโจมตีอยู่
        else if (distance < 20f && !isAttacking)
        {
            // รีเซ็ตสถานะการรอโจมตีเมื่อออกจากระยะ
            isInAttackRangeAndWaiting = false;
            attackDelayTimer = initialAttackDelay; // รีเซ็ต timer กลับไปค่าเริ่มต้น

            childCollider.enabled = false;
            animator.SetBool("attack", false); // ไม่เล่นแอนิเมชันโจมตี
            agent.speed = 5;
            agent.SetDestination(player.transform.position);
        }
        // ถ้าอยู่นอกระยะติดตามและระยะโจมตี
        else if (distance > 20f)
        {
            // รีเซ็ตสถานะการรอโจมตีเมื่อออกจากระยะ
            isInAttackRangeAndWaiting = false;
            attackDelayTimer = initialAttackDelay; // รีเซ็ต timer กลับไปค่าเริ่มต้น

            childCollider.enabled = false;
            animator.SetBool("attack", false); // ไม่เล่นแอนิเมชันโจมตี
            agent.speed = 0;
        }
    }
}