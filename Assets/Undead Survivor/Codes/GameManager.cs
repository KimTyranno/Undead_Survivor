using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  // 인스펙터의 속성들을 보기쉽게 구분하려면 [Header("내용")]을 쓰면됨
  [Header("# Game Object")]
  public Player player;
  public PoolManager pool;
  public LevelUp uiLevelUp;
  public Result uiResult;
  public GameObject enemyCleaner;
  public Transform uiJoy;

  [Header("# Game Control")]
  public float gameTime;
  public float maxGameTime = 2 * 10f;
  public bool isLive;

  [Header("# Player Info")]
  public int playerId;
  public float health;
  public float maxHealth = 100;
  public int level;
  public int kill;
  public int exp;
  public int[] nextExp = { 3, 6, 10, 20, 35, 55, 85, 125, 175, 235 };
  public static GameManager instance;

  void Awake()
  {
    instance = this;

    // 프레임을 지정 (지정하지 않으면 기본적으로 30프레임)
    Application.targetFrameRate = 60;
  }

  public void GameStart(int id)
  {
    playerId = id;
    // 플레이어의 초기체력 설정
    health = maxHealth;

    // 플레이어를 황성화
    player.gameObject.SetActive(true);

    // 기본무기 지급 (%2로 한 이유는 무기보다 캐릭터가 많을경우를 대비)
    uiLevelUp.Select(id % 2);

    Resume();

    // 배경사운드 재생
    AudioManager.instance.PlayBgm(true);
    // 캐릭터선택시 오디오
    AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
  }

  public void GameOver()
  {
    StartCoroutine(GameOverRoutine());
  }

  IEnumerator GameOverRoutine()
  {
    isLive = false;
    yield return new WaitForSeconds(0.5f);

    // 아래의 코드는 WaitForSeconds함수에 의해 0.5초후 실행됨 (묘비 애니메이션을 위함)
    uiResult.gameObject.SetActive(true);
    uiResult.Lose();
    Stop();

    // 배경사운드 종료
    AudioManager.instance.PlayBgm(false);
    // 게임패배시 오디오
    AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
  }
  public void GameVictory()
  {
    StartCoroutine(GameVictoryRoutine());
  }

  IEnumerator GameVictoryRoutine()
  {
    isLive = false;
    // 모든 몬스터를 처리함
    enemyCleaner.SetActive(true);
    yield return new WaitForSeconds(0.5f);

    // 아래의 코드는 WaitForSeconds함수에 의해 0.5초후 실행됨
    uiResult.gameObject.SetActive(true);
    uiResult.Win();
    Stop();

    // 배경사운드 종료
    AudioManager.instance.PlayBgm(false);
    // 게임승리시 오디오
    AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
  }
  public void GameRetry()
  {
    SceneManager.LoadScene(0);
  }
  public void GameQuit()
  {
    Application.Quit();
  }
  void Update()
  {
    // 게임을 멈춘경우 Update() 계열을 사용하는 로직에도 멈추게한다.
    if (!isLive) return;

    gameTime += Time.deltaTime;
    if (gameTime > maxGameTime)
    {
      gameTime = maxGameTime;
      GameVictory();
    }
  }

  public void GetExp()
  {
    // enemyCleaner때 경험치 획득을 막기위함
    if (!isLive) return;

    exp++;
    // 레벨업에 필요한 경험치에 도달하면 레벨업, 최대로 설정한 레벨에 도달해도 레벨업은 하지만, 이후부터의 경험치는 계속 최대설정레벨의 경험치가됨
    if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
    {
      level++;
      exp = 0;
      uiLevelUp.Show();
    }
  }

  public void Stop()
  {
    isLive = false;

    // 유니티의 시간속도 (배율임, 기본값은 1)
    // 0을주면 시간을 멈춘다.
    Time.timeScale = 0;

    // 조이스틱을 안보이게함
    uiJoy.localScale = Vector3.zero;
  }
  public void Resume()
  {
    isLive = true;
    Time.timeScale = 1;

    // 조이스틱을 보이게함
    uiJoy.localScale = Vector3.one;
  }
}
