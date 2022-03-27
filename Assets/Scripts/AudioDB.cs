using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "AudioDB", menuName = "Audio/AudioDB")]
public class AudioDB : ScriptableObject
{
    [SerializeField] private List<AudioData> _dB;
    private AudioSource _uiAudioSource;
    public List<AudioData> DB => _dB;
    public AudioSource UiAudioSource => _uiAudioSource;

    public void OnEnable()
    {
        GameObject cameraCanvas = GameObject.FindWithTag("CameraCanvas");
        _uiAudioSource = cameraCanvas != null ? cameraCanvas.GetComponent<AudioSource>() : null;
    }

    public AudioClip GetAudio(string id)
    {
        return _dB.Find(data => id == data.id)?.AudioClip;
    }
}

[Serializable]
public class AudioData
{
    public string id;
    public AudioClip AudioClip;
}