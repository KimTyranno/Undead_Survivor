using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
  public GameObject[] lockCharacters;
  public GameObject[] unlockCharacters;
  public GameObject uiNotice;
  enum Achive { UnlockPotato, UnlockBean }
  Achive[] achives;
  WaitForSecondsRealtime wait;

  void Awake()
  {
    // Achive[] 타입으로 명시적으로 변경
    achives = (Achive[])Enum.GetValues(typeof(Achive));

    wait = new WaitForSecondsRealtime(5);

    // 기존데이터가 없으면 초기화한 데이터를 로드함
    if (!PlayerPrefs.HasKey("MyData")) Init();
  }

  void Init()
  {
    PlayerPrefs.SetInt("MyData", 1);

    foreach (Achive achive in achives)
    {
      /**
        PlayerPrefs.SetInt("UnlockPotato", 0);
        PlayerPrefs.SetInt("UnlockBean", 0);
        위에 처럼 일일이 써도 되는데, 저장데이터가 여러개가 되는경우를 대비
      */
      PlayerPrefs.SetInt(achive.ToString(), 0);
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    UnlockCharacter();
  }

  void UnlockCharacter()
  {
    for (int i = 0; i < lockCharacters.Length; i++)
    {
      string achiveName = achives[i].ToString();

      // lock이면 0 unlock이면 1 을 가져옴
      bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
      lockCharacters[i].SetActive(!isUnlock);
      unlockCharacters[i].SetActive(isUnlock);
    }
  }
  // Update is called once per frame
  void LateUpdate()
  {
    foreach (Achive achive in achives) CheckAchive(achive);
  }

  void CheckAchive(Achive achive)
  {
    bool isAchive = false;
    switch (achive)
    {
      case Achive.UnlockPotato:
        // 제한시간까지 생존했을때 enemyCleaner로 몬스터를 처치하기때문에, 이 경우에는 감자농부의 해금을 막기위함
        if (GameManager.instance.isLive) isAchive = GameManager.instance.kill >= 10;
        break;
      case Achive.UnlockBean:
        isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
        break;
    }
    if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
    {
      PlayerPrefs.SetInt(achive.ToString(), 1);

      for (int i = 0; i < uiNotice.transform.childCount; i++)
      {
        bool isActive = i == (int)achive;
        uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
      }
      StartCoroutine(NoticeRoutine());
    }
  }

  IEnumerator NoticeRoutine()
  {
    uiNotice.SetActive(true);
    // 캐릭터 해금시 오디오
    AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
    yield return wait;
    uiNotice.SetActive(false);
  }
}
