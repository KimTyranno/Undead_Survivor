using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
  public ItemData.ItemType type;
  public float rate;
  public void Init(ItemData data)
  {
    // 기본설정
    name = "Gear " + data.itemId;
    transform.parent = GameManager.instance.player.transform;
    transform.localPosition = Vector3.zero;

    // 프로퍼티 설정
    type = data.itemType;
    rate = data.damages[0];

    // 기어적용
    ApplyGear();
  }

  public void LevelUp(float rate)
  {
    this.rate = rate;

    // 레벨업시에도 기어적용
    ApplyGear();
  }

  void ApplyGear()
  {
    switch (type)
    {
      case ItemData.ItemType.Glove:
        RateUp();
        break;
      case ItemData.ItemType.Shoe:
        SpeedUp();
        break;
    }
  }
  // Glove로 인한 공격속도 증가
  void RateUp()
  {
    // 모든무기에 적용
    Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

    foreach (Weapon weapon in weapons)
    {
      switch (weapon.id)
      {
        case 0:
          float speed = 150 * Character.WeaponSpeed;
          // 근접 무기
          weapon.speed = speed + (speed * rate);
          break;
        default:
          // 원거리 무기
          speed = 0.3f * Character.WeaponRate;
          weapon.speed = speed * (1f - rate);
          break;
      }
    }
  }

  // Shoe로 인한 이동속도 증가
  void SpeedUp()
  {
    float speed = 3 * Character.Speed;
    GameManager.instance.player.speed = speed + speed * rate;
  }
}
