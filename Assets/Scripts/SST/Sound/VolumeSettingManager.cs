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

    // ��� �����ϱ� ���� ��ü
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

        // ���� ��θ� ���� -> persistentDataPah�� volumeSettings.json ���Ϸ� �����Ѵ�.
        filePath = Path.Combine(Application.persistentDataPath, "volumeSettings.json");
    }

    // ���Ӱ� ������Ʈ �Ǵ� �������� �������ֱ� ���� �Լ�
    public void SaveVolumeSettings(VolumeSettings volumeSettings)
    {
        // VolumeSettings ��ü�� JSON ���ڿ��� ��ȯ
        string json = JsonUtility.ToJson(volumeSettings, true);
        // ��ο� �ִ� JSON���Ͽ� ��ȯ�� json ������ �Ἥ �����Ѵ�.
        File.WriteAllText(filePath, json);
    }

    public VolumeSettings LoadVolumeSettings()
    {
        // ��ο� ������ �����Ѵٸ�?
        if (File.Exists(filePath))
        {
            // ��ο� �ִ� ������ �ҷ��ͼ� ����ش�.
            string json = File.ReadAllText(filePath);
            // �� ������ JSON -> VolumeSettings ��ü�� ��ȯ���ش� 
            VolumeSettings settings = JsonUtility.FromJson<VolumeSettings>(json);
            return settings;
        }
        // ��ο� ������ �������� �ʴ´ٸ�
        else
        {
            // ������ ������ �⺻���� ��� 1�� �����ؼ� �� ��ü�� ����� ��ȯ���ش�
            return new VolumeSettings { masterVolume = 1f, bgmVolume = 1f, sfxVolume = 1f };
        }
    }
}
