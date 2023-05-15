using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  public int id;
  public int prefabId;
  public float damage;
  public int count;
  public float speed;

  void Start()
  {
    Init();
  }
  // Update is called once per frame
  void Update()
  {
    switch (id)
    {
      case 0:
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
        break;
      default:
        break;
    }

    if (Input.GetButtonDown("Jump")) LevelUp(20, 5);
  }

  public void LevelUp(float damage, int count)
  {
    this.damage = damage;
    this.count += count;
    if (id == 0) Batch();
  }
  public void Init()
  {
    switch (id)
    {
      case 0:
        speed = 150;
        Batch();
        break;
      default:
        break;
    }
  }

  void Batch()
  {
    for (int i = 0; i < count; i++)
    {
      Transform bullet;

      // 자식오브젝트의 갯수 확인
      if (i < transform.childCount)
      {
        // index가 아직 childCount 범위내일경우 GetChild로 가져옴 (즉, 기존에 있는 오브젝트를 먼저 재활용함)
        // 이미 GetChild를 사용함으로써 부모가 weapon으로 되어있으므로 부모를 변경할 필요는 없다.
        bullet = transform.GetChild(i);
      }
      else
      {
        // 갯수가 모자라면 풀링에서 가져옴
        bullet = GameManager.instance.pool.Get(prefabId).transform;
        // bullet의 부모가 pollManager이기때문에 weapon으로 변경해줌
        bullet.parent = transform;
      }

      // 위치와 회전을 초기화 (부모의 위치와 회전이 계속바뀌니까 자식을 생성할때는 이것의 영향을 받지않게 0,0,0으로 초기화 시켜줌)
      bullet.localPosition = Vector3.zero;
      bullet.localRotation = Quaternion.identity;

      // 근접무기의 z축 변경
      Vector3 rotVec = Vector3.forward * 360 * i / count;
      bullet.Rotate(rotVec);
      // z축을 변경후 자기자신의 위쪽으로 이동 (플레이어 주변에서 돌게하기 위함임, 1.2f는 플레이어와의 거리임)
      bullet.Translate(bullet.up * 1.2f, Space.World);
      bullet.GetComponent<Bullet>().Init(damage, -1); // -1은 무한으로 관통하기 위함
    }
  }
}
