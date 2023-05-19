using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  // 인스펙터의 속성들을 보기쉽게 구분하려면 [Header("내용")]을 쓰면됨
  [Header("# Game Object")]
  public Player player;
  public PoolManager pool;
  public LevelUp uiLevelUp;

  [Header("# Game Control")]
  public float gameTime;
  public float maxGameTime = 2 * 10f;
  public bool isLive;

  [Header("# Player Info")]
  public int health;
  public int maxHealth = 100;
  public int level;
  public int kill;
  public int exp;
  public int[] nextExp = { 1, 3, 60, 100, 150, 210, 280, 360, 450, 600 };
  public static GameManager instance;

  void Awake()
  {
    instance = this;
  }

  void Start()
  {
    // 플레이어의 초기체력 설정
    health = maxHealth;

    // 기본무기 지급 (아직 캐릭터 선택이 없으므로 임시)
    uiLevelUp.Select(0);
  }
  void Update()
  {
    // 게임을 멈춘경우 Update() 계열을 사용하는 로직에도 멈추게한다.
    if (!isLive) return;

    gameTime += Time.deltaTime;
    if (gameTime > maxGameTime)
    {
      gameTime = maxGameTime;
    }
  }

  public void GetExp()
  {
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
  }
  public void Resume()
  {
    isLive = true;
    Time.timeScale = 1;
  }
}
