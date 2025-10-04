using UnityEngine;
using Unity.Cinemachine; // ������� using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    // ��ҧ�ԧ�֧ Virtual Camera ���е��
    public CinemachineCamera warriorVcam;
    public CinemachineCamera mageVcam;
    public CinemachineCamera clericVcam;
    public CinemachineCamera enemyVcam;
    public CinemachineCamera playerVcam;

    private CinemachineCamera currentActiveVcam; // �� VCam �����ѧ Active ����

    private void Start()
    {
        warriorVcam.Priority = 0;
        mageVcam.Priority = 0;
        clericVcam.Priority = 0;
        enemyVcam.Priority = 0;
        playerVcam.Priority = 0;
    }

    // �ѧ��ѹ����Ѻ��Ѻ���ͧ
    public void SwitchCamera(CinemachineCamera targetVcam)
    {
        if (targetVcam == null)
        {
            Debug.LogWarning("CameraSwitcher: Target VCam is null!");
            return;
        }

        // �Դ VCam �����ѧ Active ���� (�����)
        if (currentActiveVcam != null && currentActiveVcam != targetVcam)
        {
            currentActiveVcam.Priority = 0; // ��� Priority �� 0 ���ͻԴ
        }

        // ����� VCam ������� Active
        targetVcam.Priority = 10; // ��� Priority �٧���� VCam ���� ������� Active

        currentActiveVcam = targetVcam; // �ѻവ VCam �����ѧ Active
    }
}