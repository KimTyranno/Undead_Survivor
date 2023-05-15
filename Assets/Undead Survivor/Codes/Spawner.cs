using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

  public Transform[] spawnPoint;
  public SpawnData[] spawnData;
  int level;
  float timer;

  void Awake()
  {
    // 자식의 Transform을 가져옴 (자신도 포함)
    spawnPoint = GetComponentsInChildren<Transform>();
  }
  // Update is called once per frame
  void Update()
  {
    timer += Time.deltaTime;
    level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);
    if (timer > spawnData[level].spawnTime)
    {
      Spawn();
      timer = 0;
    }
  }

  void Spawn()
  {
    GameObject enemy = GameManager.instance.pool.Get();
    // spawnPoint에서 난수를 0부터 하지 않는이유는 0은 자기자신(부모)이기 때문
    enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

    enemy.GetComponent<Enemy>().Init(spawnData[level]);
  }
}
// 직렬화
[System.Serializable]
public class SpawnData
{
  public float spawnTime;
  public int spriteType;
  public int health;
  public float speed;
}