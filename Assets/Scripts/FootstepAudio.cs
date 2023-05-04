using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public List<AudioClip> footsteps;
    public AudioClip jumpUp;
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

    public void PlayJumpUp()
    {
        selectedAudio = jumpUp;
        source.PlayOneShot(jumpUp);
    }
}
