using UnityEngine;

public class music : MonoBehaviour
{
    private AudioSource targetAudioSource;
    private void Update()
    {
        targetAudioSource = GetComponent<AudioSource>();
        if (targetAudioSource == null)
        {
            Debug.LogError("AudioSource not found on this GameObject with 'music' script.");
            return; // ��ش��÷ӧҹ�������� AudioSource
        }
        targetAudioSource.volume = PlayerDataManager.getmusic();
    }

}
