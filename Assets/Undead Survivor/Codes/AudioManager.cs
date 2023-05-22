using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance;

  [Header("# BGM")]
  public AudioClip bgmClip;
  public float bgmVolume;
  AudioSource bgmPlayer;
  AudioHighPassFilter bgmEffect;

  // 효과음
  [Header("# SFX")]
  public AudioClip[] sfxClips;
  public float sfxVolume;
  public int channels;
  AudioSource[] sfxPlayers;
  // 가장 마지막에 플레이했던 플레이어의 인덱스
  int channelIndex;

  // 숫자를 대입하면 그 숫자로 인덱스가 지정되고 그 뒤 부터는 지정한 인덱스의 다음번호로 인덱스가 설정됨
  public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }
  void Awake()
  {
    instance = this;
    Init();
  }

  void Init()
  {
    // 배경음 플레이어 초기화
    GameObject bgmObject = new GameObject("BgmPlayer");
    // AudioManger의 자식으로 설정
    bgmObject.transform.parent = transform;
    bgmPlayer = bgmObject.AddComponent<AudioSource>();
    // 게임을 시작하자마자 재생하는지 여부
    bgmPlayer.playOnAwake = false;
    // 반복여부
    bgmPlayer.loop = true;
    bgmPlayer.volume = bgmVolume;
    bgmPlayer.clip = bgmClip;
    // 메인카메라로 접근하려면 Camera.main
    bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

    // 효과음 플레이어 초기화
    GameObject sfxObject = new GameObject("SfxPlayer");
    sfxObject.transform.parent = transform;
    sfxPlayers = new AudioSource[channels];

    for (int i = 0; i < sfxPlayers.Length; i++)
    {
      sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
      sfxPlayers[i].playOnAwake = false;
      // EffectBgm에서 적용하는 MainCamara > Audio High Pass Filter에 영향을 받지않게 하기위함
      sfxPlayers[i].bypassListenerEffects = true;
      sfxPlayers[i].volume = sfxVolume;

    }
  }
  // 배경음 재생
  public void PlayBgm(bool isPlay)
  {
    if (isPlay) bgmPlayer.Play();
    else bgmPlayer.Stop();
  }
  // 아이템창 UI가 보일때 배경을에 필터를 적용
  public void EffectBgm(bool isPlay)
  {
    bgmEffect.enabled = isPlay;
  }

  // 효과음 재생
  public void PlaySfx(Sfx sfx)
  {
    for (int i = 0; i < sfxPlayers.Length; i++)
    {
      int loopIndex = (i + channelIndex) % sfxPlayers.Length;

      if (sfxPlayers[loopIndex].isPlaying) continue;

      // 효과음이 2개이상이면 랜덤으로함
      int ranIndex = 0;
      if (sfx == Sfx.Hit || sfx == Sfx.Melee)
      {
        ranIndex = Random.Range(0, 2);
      }
      channelIndex = loopIndex;
      sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
      sfxPlayers[loopIndex].Play();
      break;
    }
  }
}
