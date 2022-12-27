using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds
{
    Bomb,
    Ice,
    PlaceTower,
    Pop,
    Selltower,
    LevelUp
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    List<AudioClip> sounds = new List<AudioClip>();
    [SerializeField] AudioSource SFXSource;
    AudioSource[] PopSources;
    [SerializeField] AudioClip[] popSounds = new AudioClip[4];
    [SerializeField] AudioClip levelUp;
    [SerializeField] AudioClip iceTower;
    [SerializeField] AudioClip placeTower;
    [SerializeField] AudioClip bomb;
    [SerializeField] AudioClip sellTower;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        PopSources = new AudioSource[8];
        for (int i = 0; i < PopSources.Length; i++)
            PopSources[i] = gameObject.AddComponent<AudioSource>();
    }

    private void OnApplicationQuit()
    {
        Instance = null;
    }

    public void PlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.Bomb:
                SFXSource.PlayOneShot(bomb);
                break;
            case Sounds.Ice:
                SFXSource.PlayOneShot(iceTower);
                break;
            case Sounds.PlaceTower:
                SFXSource.PlayOneShot(placeTower);
                break;
            case Sounds.Pop:
                foreach (var source in PopSources)
                    if (!source.isPlaying)
                    {
                        source.clip = popSounds.GetRandom();
                        source.Play();
                        break;
                    }
                break;
            case Sounds.Selltower:
                SFXSource.PlayOneShot(sellTower);
                break;
            case Sounds.LevelUp:
                SFXSource.PlayOneShot(levelUp);
                break;
        }
    }
}
