using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SourcesManager : MonoSingle<SourcesManager>
{
    private AudioSource clipPlaySource;
    private void Awake()
    {
        instance = this;
        clipPlaySource = GetComponent<AudioSource>();
    }

    public void PlayOnShot(AudioClip clip,float volumn)
    {
        clipPlaySource.PlayOneShot(clip,volumn);
    }
}
