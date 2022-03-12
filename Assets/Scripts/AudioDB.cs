using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "AudioDB", menuName = "Audio/AudioDB")]
public class AudioDB : ScriptableObject
{
    [SerializeField] private List<AudioData> _dB;
    public List<AudioData> DB => _dB;

    public AudioData GetAudio(string id)
    {
        return _dB.Find(data => id == data.id);
    }
}

[Serializable]
public struct AudioData
{
    public string id;
    public AudioClip AudioClip;
}