using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]
public class VolumeSettings
{
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;
}

public class VolumeSettingManager : MonoBehaviour
{
    public static VolumeSettingManager Instance;

    // 경로 설정하기 위한 객체
    private string filePath;

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

        DontDestroyOnLoad(Instance);

        // 파일 경로를 설정 -> persistentDataPah에 volumeSettings.json 파일로 저장한다.
        filePath = Path.Combine(Application.persistentDataPath, "volumeSettings.json");
    }

    // 새롭게 업데이트 되는 볼륨값을 저장해주기 위한 함수
    public void SaveVolumeSettings(VolumeSettings volumeSettings)
    {
        // VolumeSettings 객체를 JSON 문자열로 변환
        string json = JsonUtility.ToJson(volumeSettings, true);
        // 경로에 있는 JSON파일에 변환한 json 파일을 써서 저장한다.
        File.WriteAllText(filePath, json);
    }

    public VolumeSettings LoadVolumeSettings()
    {
        // 경로에 파일이 존재한다면?
        if (File.Exists(filePath))
        {
            // 경로에 있는 파일을 불러와서 담아준다.
            string json = File.ReadAllText(filePath);
            // 그 파일을 JSON -> VolumeSettings 객체로 변환해준다 
            VolumeSettings settings = JsonUtility.FromJson<VolumeSettings>(json);
            return settings;
        }
        // 경로에 파일이 존재하지 않는다면
        else
        {
            // 파일이 없으면 기본값을 모두 1로 설정해서 새 객체를 만들고 반환해준다
            return new VolumeSettings { masterVolume = 1f, bgmVolume = 1f, sfxVolume = 1f };
        }
    }
}
