using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioDB _audioDB;

    public string Id;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Play()
    {
        if (_audioDB.UiAudioSource != null)
        {
            _audioDB.UiAudioSource.PlayOneShot(_audioDB.GetAudio(Id));
        }
    }

    public void Play(string id)
    {
        if (_audioDB.UiAudioSource != null)
        {
            _audioDB.UiAudioSource.PlayOneShot(_audioDB.GetAudio(id));
        }
    }
}