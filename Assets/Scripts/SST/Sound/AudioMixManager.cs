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
        // ����� �ͼ��� ���� -80 ~ 0 �����̱� ������ 0.0001 ~ 1�� ���� Log10 * 20�� ���ش�.
        audioMixer.SetFloat(audioMixType.ToString(), Mathf.Log10(volume) * 20);
    }
}
