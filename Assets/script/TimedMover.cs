using UnityEngine;
using System;

public class TimedMover : MonoBehaviour
{
    private Transform target;
    private float speed;
    private Action onHit;

    // ตัวเลือกเสริม
    private bool lockY = false;
    private float fixedY;        // ค่าที่จะล็อกแกน Y ไว้
    private bool destroyIfNoTarget = true;

    /// <summary>
    /// target: เป้าหมายที่จะวิ่งไปหา
    /// speed: ความเร็ว (หน่วย/วินาที)
    /// onHit: เรียกตอนถึงเป้าหมาย
    /// lockY: true เพื่อให้ตำแหน่ง Y คงที่
    /// fixedY: ค่าที่จะล็อกแกน Y (เช่น Y ของผู้ร่ายตอนยิง)
    /// destroyIfNoTarget: ถ้า target หาย ให้ทำลายตัวเอง
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

        // คำนวณตำแหน่งปลายทางโดยล็อกแกน Y (ป้องกันจม/ลอย)
        Vector3 targetPos = target.position;
        if (lockY) targetPos.y = fixedY;

        // ขยับเข้าหา
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // บังคับให้ Y คงที่ระหว่างทางด้วย (กันการแกว่งจากฟิสิกส์/อนิเม)
        if (lockY)
        {
            var p = transform.position;
            p.y = fixedY;
            transform.position = p;
        }

        // ถึงเป้าหมาย?
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            onHit?.Invoke();
            Destroy(gameObject);
        }
    }
}
