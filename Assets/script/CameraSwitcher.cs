using UnityEngine;
using Unity.Cinemachine; // อย่าลืม using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    // อ้างอิงถึง Virtual Camera แต่ละตัว
    public CinemachineCamera warriorVcam;
    public CinemachineCamera mageVcam;
    public CinemachineCamera clericVcam;
    public CinemachineCamera enemyVcam;
    public CinemachineCamera playerVcam;

    private CinemachineCamera currentActiveVcam; // เก็บ VCam ที่กำลัง Active อยู่

    private void Start()
    {
        warriorVcam.Priority = 0;
        mageVcam.Priority = 0;
        clericVcam.Priority = 0;
        enemyVcam.Priority = 0;
        playerVcam.Priority = 0;
    }

    // ฟังก์ชันสำหรับสลับกล้อง
    public void SwitchCamera(CinemachineCamera targetVcam)
    {
        if (targetVcam == null)
        {
            Debug.LogWarning("CameraSwitcher: Target VCam is null!");
            return;
        }

        // ปิด VCam ที่กำลัง Active อยู่ (ถ้ามี)
        if (currentActiveVcam != null && currentActiveVcam != targetVcam)
        {
            currentActiveVcam.Priority = 0; // ตั้ง Priority เป็น 0 เพื่อปิด
        }

        // ทำให้ VCam เป้าหมาย Active
        targetVcam.Priority = 10; // ตั้ง Priority สูงกว่า VCam อื่นๆ เพื่อให้ Active

        currentActiveVcam = targetVcam; // อัปเดต VCam ที่กำลัง Active
    }
}