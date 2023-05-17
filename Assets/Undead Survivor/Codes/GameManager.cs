using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  // 인스펙터의 속성들을 보기쉽게 구분하려면 [Header("내용")]을 쓰면됨
  [Header("# Game Object")]
  public Player player;
  public PoolManager pool;
  [Header("# Game Control")]
  public float gameTime;
  public float maxGameTime = 2 * 10f;

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
  }
  void Update()
  {
    gameTime += Time.deltaTime;
    if (gameTime > maxGameTime)
    {
      gameTime = maxGameTime;
    }
  }

  public void GetExp()
  {
    exp++;
    // 레벨업에 필요한 경험치에 도달하면 레벨업
    if (exp == nextExp[level])
    {
      level++;
      exp = 0;
    }
  }
}
