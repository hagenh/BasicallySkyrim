using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public List<AudioClip> footsteps;
    private AudioClip selectedAudio;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        selectedAudio = footsteps[Random.Range(0, footsteps.Count)];
        source.PlayOneShot(selectedAudio);
    }
}
