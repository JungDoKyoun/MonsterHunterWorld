using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixManager : MonoBehaviour
{
    public enum AudioMixType
    {
        Master,
        BGM,
        SFX
    }

    public static AudioMixManager Instance;

    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetAudioVolume(AudioMixType audioMixType, float volume)
    {
        // 오디오 믹서의 값은 -80 ~ 0 까지이기 때문에 0.0001 ~ 1의 값을 Log10 * 20을 해준다.
        audioMixer.SetFloat(audioMixType.ToString(), Mathf.Log10(volume) * 20);
    }
}
