using UnityEngine;

[RequireComponent(typeof(Light))]
public class LanternFlicker : MonoBehaviour
{
    public float baseIntensity = 4.0f;   // ��ҡ�ҧ�ͧ�������ҧ
    public float flickerAmount = 0.4f;   // �дѺ������ (0.2�0.6 ���ѧ��)
    public float flickerSpeed = 2.0f;    // �������ǡ����� (1.5�3.0)

    public float baseRange = 2.8f;       // ��ҡ�ҧ�ͧ�����������ʧ
    public float rangeAmount = 0.15f;    // �дѺ�����蹢ͧ����
    public float rangeSpeed = 1.2f;

    private Light L;
    private float seedI, seedR;

    void Awake()
    {
        L = GetComponent<Light>();
        seedI = Random.value * 100f;
        seedR = Random.value * 100f;
        L.type = LightType.Point;
        L.shadows = LightShadows.Soft;
    }

    void Update()
    {
        float t = Time.time;
        // ��蹴��� Perlin ������ ����о�Ժ�Ѵ
        float i = Mathf.PerlinNoise(seedI, t * flickerSpeed);
        float r = Mathf.PerlinNoise(seedR, t * rangeSpeed);

        L.intensity = baseIntensity + (i - 0.5f) * 2f * flickerAmount;
        L.range = baseRange + (r - 0.5f) * 2f * rangeAmount;
    }
}
