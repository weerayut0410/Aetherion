using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyboxExposureLerp : MonoBehaviour
{
    [Header("Exposure Settings")]
    public float startExposure = 0.2f;
    public float targetExposure = 1.2f;
    public float duration = 3f;

    private float timer = 0f;

    float newExposure;

    public GameObject Canvasend;

    void Start()
    {
        Time.timeScale = 1.0f;
        // ตั้งค่าเริ่มต้น
        RenderSettings.skybox.SetFloat("_Exposure", startExposure);
        Canvasend.SetActive(false);
    }

    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            newExposure = Mathf.Lerp(startExposure, targetExposure, t);

            RenderSettings.skybox.SetFloat("_Exposure", newExposure);
        }
        if (newExposure == 1.2f)
        {
            Canvasend.SetActive(true);
            StartCoroutine(slow());
        }
    }
    public IEnumerator slow()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("menu");
    }
}
