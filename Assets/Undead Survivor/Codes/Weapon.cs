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
  float timer;
  Player player;

  void Awake()
  {
    // GetComponentInParent로 부모의 컴포넌트를 가져올 수 있다.
    // player = GetComponentInParent<Player>();

    // Weapon이 더이상 게임이 시작하자마자 player의 자식으로 존재하지 않기때문에 GameManager에서 초기화하게 변경
    player = GameManager.instance.player;
  }

  // Update is called once per frame
  void Update()
  {
    if (!GameManager.instance.isLive) return;

    switch (id)
    {
      case 0:
        transform.Rotate(Vector3.back * speed * Time.deltaTime);

        // 근접공격시 오디오 (이 위치가 아닌 다른곳에서 해야할듯)
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Melee);
        break;
      default:
        timer += Time.deltaTime;
        if (timer > speed)
        {
          timer = 0f;
          Fire();
        }
        break;
    }

    if (Input.GetButtonDown("Jump")) LevelUp(10, 1);
  }

  public void LevelUp(float damage, int count)
  {
    this.damage = damage * Character.Damage;
    this.count += count;

    if (id == 0) Batch();

    // 레벨업한 무기에 맞게 기어를 다시 적용
    player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
  }
  public void Init(ItemData data)
  {
    // 기본설정
    name = "Weapon " + data.itemId;
    // 플레이어의 자식오브젝트로 생성시키기 위함
    transform.parent = player.transform;
    // localPosition(지역위치) 사용하여 플레리어안에서 위치를 초기화
    transform.localPosition = Vector3.zero;

    // 프로퍼티 설정
    id = data.itemId;
    damage = data.baseDamage * Character.Damage;
    count = data.baseCount + Character.Count;

    for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
    {
      // prefabId는 풀매니저에서 찾아서 초기화시킴 (프리팹아이디는 풀매니저의 인덱스와 동일)
      if (data.projectile == GameManager.instance.pool.prefabs[i])
      {
        prefabId = i;
        break;
      }
    }
    switch (id)
    {
      case 0:
        speed = 150 * Character.WeaponSpeed;
        Batch();
        break;
      default:
        speed = 0.3f * Character.WeaponRate;
        break;
    }

    // Hand 설정
    Hand hand = player.hands[(int)data.itemType];
    hand.spriter.sprite = data.hand;
    hand.gameObject.SetActive(true);

    // 특정함수 호출을 모든자식에게 방송하는 함수 (player 오브젝트가 가지고 있는 모든 gear는 ApplyGear()를 실행하게됨)
    // 두번째인자는 Gear가 없을때 에러나 나는거 대응
    player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
      bullet.Translate(bullet.up * 1.5f, Space.World);
      bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100은 무한으로 관통하기 위함
    }
  }

  void Fire()
  {
    // 대상이 있는지 확인
    if (!player.scanner.nearestTarget) return;

    // 타겟의 위치
    Vector3 targetPos = player.scanner.nearestTarget.position;
    // 타겟의 방향 (크기가 포함된 방향)
    Vector3 dir = targetPos - transform.position;
    dir = dir.normalized;

    Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
    bullet.position = transform.position;
    // TODO: FromToRotation이란 지정된 축을 중심으로 목표를 향해 회전하는 함수
    bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    bullet.GetComponent<Bullet>().Init(damage, count, dir);

    // 원거리 공격발사시 오디오
    AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
  }
}
