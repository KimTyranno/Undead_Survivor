using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public float speed;
  public float health;
  public float maxHealth;
  public RuntimeAnimatorController[] animatorController;
  public Rigidbody2D target;
  bool isLive;

  Rigidbody2D rigid;
  Animator anim;
  SpriteRenderer spriter;
  void Awake()
  {
    rigid = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    spriter = GetComponent<SpriteRenderer>();
  }

  void FixedUpdate()
  {
    if (!isLive) return;
    // 몬스터가 가야할 방향
    Vector2 dirVec = target.position - rigid.position;
    // 몬스터가 가야할 위치 (즉, 플레이어가 키를 입력한 값을 더한 이동임)
    Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
    rigid.MovePosition(rigid.position + nextVec);
    // 물리속도가 이동에 영향을 주지않도록 속도를 제거
    rigid.velocity = Vector2.zero;
  }

  void LateUpdate()
  {
    if (!isLive) return;
    spriter.flipX = target.position.x < rigid.position.x;
  }

  // 스크립트가 활성화될때 호출되는 함수임
  void OnEnable()
  {
    target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    isLive = true;
    health = maxHealth;
  }

  public void Init(SpawnData initData)
  {
    anim.runtimeAnimatorController = animatorController[initData.spriteType];
    speed = initData.speed;
    maxHealth = health = initData.health;

  }

  void OnTriggerEnter2D(Collider2D other)
  {
    // 충돌한게 총알이 아니면 무시
    if (!other.CompareTag("Bullet")) return;

    // 데미지 들어감
    health -= other.GetComponent<Bullet>().damage;

    if (health > 0)
    {

    }
    else
    {
      Dead();
    }
  }

  void Dead()
  {
    gameObject.SetActive(false);
  }
}
