using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// createAssetMenu란 커스텀메뉴를 생성하는 속성 (유니티 프로젝트탭에서 우클릭 - create에서 확인가능)
[CreateAssetMenu(fileName = "item", menuName = "Scriptable Object/ItemData")]

// ScriptableObject
public class ItemData : ScriptableObject
{
  public enum ItemType { Melee, Range, Glove, Shoe, Heal }

  [Header("# Main Info")]
  public ItemType itemType;
  public int itemId;
  public string itemName;
  // 인스펙터에서 텍스트를 여러줄 넣을 수 있게 해주는속성
  [TextArea]
  public string itemDesc;
  public Sprite itemIcon;

  [Header("# Level Data")]
  public float baseDamage;
  public int baseCount;
  public float[] damages;
  public int[] counts;

  [Header("# Weapon")]
  public GameObject projectile;
  public Sprite hand;
}