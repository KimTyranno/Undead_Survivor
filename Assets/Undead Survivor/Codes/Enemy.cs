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
  Collider2D coll;
  Animator anim;
  SpriteRenderer spriter;
  WaitForFixedUpdate wait;
  void Awake()
  {
    rigid = GetComponent<Rigidbody2D>();
    coll = GetComponent<Collider2D>();
    anim = GetComponent<Animator>();
    spriter = GetComponent<SpriteRenderer>();
    wait = new WaitForFixedUpdate();
  }

  void FixedUpdate()
  {
    if (!GameManager.instance.isLive) return;

    // GetCurrentAnimatorStateInfo란 현재상태의 정보를 가져오는 함수
    if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;
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
    if (!GameManager.instance.isLive) return;
    if (!isLive) return;
    spriter.flipX = target.position.x < rigid.position.x;
  }

  // 스크립트가 활성화될때 호출되는 함수임
  void OnEnable()
  {
    target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    isLive = true;
    coll.enabled = true;
    rigid.simulated = true;
    spriter.sortingOrder = 2;
    anim.SetBool("Dead", false);
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
    if (!other.CompareTag("Bullet") || !isLive) return;

    // 데미지 들어감
    health -= other.GetComponent<Bullet>().damage;

    // 넉백
    StartCoroutine(KnockBack());

    if (health > 0)
    {
      // 애니메이터의 상태를 변경
      anim.SetTrigger("Hit");

      // 몬스터 피격시 오디오
      AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
    }
    else
    {
      isLive = false;
      // Collider도 비활성화
      coll.enabled = false;
      // RigidBody도 물리적 비활성화
      rigid.simulated = false;
      // 다른 오브젝트를 가리지 않게 sort를 한단계 내림
      spriter.sortingOrder = 1;
      // 죽는 애니메이션
      anim.SetBool("Dead", true);
      GameManager.instance.kill++;
      GameManager.instance.GetExp();

      // enemyCleaner일때는 사운드를 재생하지 않도록하기 위함
      if (GameManager.instance.isLive)
      {
        // 몬스터 처치시 오디오
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
      }
    }
  }

  IEnumerator KnockBack()
  {
    //  yield return null; // 1프레임 쉬기
    //  yield return new WaitForSeconds(2f); // 2초 쉬기
    yield return wait; // 다음 하나의 물리프레임 딜레이
    Vector3 playerPos = GameManager.instance.player.transform.position;
    // 플레이어 기준의 반대방향?
    Vector3 dirVec = transform.position - playerPos;
    rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
  }
  void Dead()
  {
    gameObject.SetActive(false);
  }
}
