using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public Sound[] musicSounds;
    public Sound[] sfxSounds;
    public AudioSource musicSource, sfxSource, acelerationSource;
    public int currentMusic = 2;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        musicSource.volume = 0.1f;
        sfxSource.volume = 1f;
        PlayMusic(0);
    }

    public void PlayMusic(int name) {
        if (currentMusic == name) return;

        var s = musicSounds[name];
        currentMusic = name;
        musicSource.loop = true;
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlayIntroAndLoop(int intro, int loop) {
        if (currentMusic == intro && musicSource.isPlaying)
            return;

        var introSound = musicSounds[intro];
        currentMusic = intro;
        musicSource.loop = false;
        musicSource.clip = introSound.clip;
        musicSource.Play();

        StartCoroutine(PlayLoopAfterIntro(introSound.clip.length, loop));
    }

    private IEnumerator PlayLoopAfterIntro(float introLength, int loop) {
        yield return new WaitForSeconds(introLength);

        var loopSound = musicSounds[loop];
        currentMusic = loop;
        musicSource.loop = true;
        musicSource.clip = loopSound.clip;
        musicSource.Play();
    }

    public void PlaySFXOneShot(string name) {
        var s = Array.Find(sfxSounds, x => x.name == name);
        sfxSource.PlayOneShot(s.clip);
    }

    public void PlaySFXOneShot(AudioClip audio) {
        sfxSource.PlayOneShot(audio);
    }

    public void PlaySFXUntil(string name, Func<bool> condition) {
        var s = Array.Find(sfxSounds, x => x.name == name);
        StartCoroutine(PlayLoop(s, condition));
    }

    private IEnumerator PlayLoop(Sound s, Func<bool> condition) {
        acelerationSource.loop = true;
        acelerationSource.clip = s.clip;
        acelerationSource.Play();

        yield return new WaitUntil(condition);

        acelerationSource.Stop();
    }

    public void PlayAceleration(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        
        acelerationSource.loop = true;
        acelerationSource.clip = s.clip;
        acelerationSource.Play();
    }

    public void StopAceleration()
    {
        acelerationSource.loop = false;
    }
}