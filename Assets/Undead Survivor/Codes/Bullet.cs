using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  public float damage;
  // 관통
  public int per;

  Rigidbody2D rigid;

  void Awake()
  {
    rigid = GetComponent<Rigidbody2D>();
  }
  public void Init(float damage, int per, Vector3 dir)
  {
    this.damage = damage;
    this.per = per;

    // 관통이 무한(-1)보다 크면 속도를 적용
    if (per > -1)
    {
      rigid.velocity = dir * 15f;
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    // 적이 아니거나 근접무기면 관통에 대한 로직을 무시함
    if (!other.CompareTag("Enemy") || per == -1) return;

    per--;
    // 관통값이 줄어들면서 -1이 되면 비활성화 시킴
    if (per == -1)
    {
      // 물리속도도 미리 초기화 시켜줌
      rigid.velocity = Vector2.zero;
      gameObject.SetActive(false);
    }
  }
}
