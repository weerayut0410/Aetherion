using UnityEngine;

public class SetPositionAndRotation : MonoBehaviour // เปลี่ยนชื่อ Script ให้สื่อความหมายมากขึ้น
{
    private Vector3 initialPosition;    // สำหรับเก็บตำแหน่งเริ่มต้น
    private Quaternion initialRotation; // สำหรับเก็บค่าการหมุนเริ่มต้น

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // เก็บตำแหน่งปัจจุบันของ GameObject นี้เมื่อเริ่มต้นเกม
        initialPosition = transform.position;

        // เก็บค่าการหมุนปัจจุบันของ GameObject นี้เมื่อเริ่มต้นเกม
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // บังคับให้ตำแหน่งของ GameObject กลับไปอยู่ที่ตำแหน่งเริ่มต้นตลอดเวลา
        transform.position = initialPosition;

        // บังคับให้การหมุนของ GameObject กลับไปอยู่ที่ค่าเริ่มต้นตลอดเวลา
        transform.rotation = initialRotation;
    }
}