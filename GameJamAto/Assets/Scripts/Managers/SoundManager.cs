using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] AudioClip[] nerdTakesDamageClips;
    [SerializeField] AudioClip[] nerdDiesClips;
    public const int MAX_NUMBER_OF_NERD_CLIP= 5;
    private AudioSource[] nerdSoundInstances = new AudioSource[MAX_NUMBER_OF_NERD_CLIP];

    [SerializeField] AudioClip insertCoinClip;
    [SerializeField] AudioClip pickUpCoinClip;
    [SerializeField] AudioClip towerActivatedClip;
    [SerializeField] AudioClip towerDeactivatedClip;

    [SerializeField] AudioClip WaveStartSound;
    [SerializeField] AudioClip WaveFinishSound;
    [SerializeField] AudioClip GameOverSound;

    [SerializeField] AudioClip music;

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    //Return false if we can't play the clip because of too many instances
    public bool TryPlayNerdTakesDamageClip(AudioSource source)
    {
        for(int i=0;i<nerdSoundInstances.Length;++i)
        {
            if(nerdSoundInstances[i]==null || nerdSoundInstances[i].isPlaying==false)
            {
                source.clip = nerdTakesDamageClips[Random.Range(0, nerdTakesDamageClips.Length)];
                source.pitch = Random.Range(.8f, 1.2f);
                source.Play();
                nerdSoundInstances[i] = source;
                return true;
            }
        }
        return false;
    }

    //Return false if we can't play the clip because of too many instances
    public bool TryPlayNerdDiesClip(AudioSource source)
    {
        for (int i = 0; i < nerdSoundInstances.Length; ++i)
        {
            if (nerdSoundInstances[i] == null || nerdSoundInstances[i].isPlaying == false)
            {
                source.clip = nerdDiesClips[Random.Range(0, nerdTakesDamageClips.Length)];
                source.pitch = Random.Range(.8f, 1.2f);
                source.Play();
                nerdSoundInstances[i] = source;
                return true;
            }
        }
        //If we could not find an empty source, we try to find a take damage instance to replace
        //Note: this code is pretty shit
        for (int i = 0; i < nerdSoundInstances.Length; ++i)
        {
            for(int j=0;j<nerdTakesDamageClips.Length;j++)
            {
                if (nerdTakesDamageClips[j] == nerdSoundInstances[i].clip)
                {
                    nerdSoundInstances[i].Stop();

                    source.clip = nerdDiesClips[Random.Range(0, nerdTakesDamageClips.Length)];
                    source.pitch = Random.Range(.8f, 1.2f);
                    source.Play();
                    nerdSoundInstances[i] = source;
                    return true;
                }
            }
        }
        return false;
    }

    public void PlayInsertCoin(AudioSource source)
    {
        source.clip = insertCoinClip;
        source.pitch = Random.Range(.8f, 1.2f);
        source.Play();
    }

    public void PlayPickupCoin(AudioSource source)
    {
        source.clip = pickUpCoinClip;
        source.pitch = Random.Range(.8f, 1.2f);
        source.Play();
    }

    public void PlayTowerActivated(AudioSource source)
    {
        source.clip = towerActivatedClip;
        source.pitch = Random.Range(.8f, 1.2f);
        source.Play();
    }

    public void PlayTowerDeactivated(AudioSource source)
    {
        source.clip = towerDeactivatedClip;
        source.pitch = Random.Range(.8f, 1.2f);
        source.Play();
    }

    public void PlayWaveStart()
    {
        ambientSource.clip = WaveStartSound;
        ambientSource.Play();

        PlayMusic();
    }

    public void PlayWaveFinish()
    {
        StopMusic();

        ambientSource.clip = WaveFinishSound;
        ambientSource.Play();
    }

    public void PlayGameOver()
    {
        StopMusic();

        ambientSource.clip = GameOverSound;
        ambientSource.Play();
    }


    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
