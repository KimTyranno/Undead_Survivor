using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
  public ItemData data;
  public int level;
  public Weapon weapon;
  public Gear gear;

  Image icon;
  Text textLevel;
  Text textName;
  Text textDesc;

  void Awake()
  {
    // 자식 오브젝트의 컴포넌트를 가져와야하므로 GetComponentsInChildre 사용, 첫번째요소는 자기자신이므로 두번째 요소를 가져옴
    icon = GetComponentsInChildren<Image>()[1];
    icon.sprite = data.itemIcon;

    Text[] texts = GetComponentsInChildren<Text>();
    // texts[0] texts[1] texts[2]의 기준은 인스펙터상의 itemGroup/item 아래에 있는 text들의 순서이다.
    textLevel = texts[0];
    textName = texts[1];
    textDesc = texts[2];

    // 아이템이름은 바뀌지 않으므로 여기서 바로 초기화시킴
    textName.text = data.itemName;
  }

  // 활성화되면 아이템 설명을 표시
  void OnEnable()
  {
    textLevel.text = "Lv." + (level + 1);

    switch (data.itemType)
    {
      case ItemData.ItemType.Melee:
      case ItemData.ItemType.Range:
        textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
        break;
      case ItemData.ItemType.Glove:
      case ItemData.ItemType.Shoe:
        textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
        break;
      default:
        textDesc.text = string.Format(data.itemDesc);
        break;

    }
  }

  public void onClick()
  {
    switch (data.itemType)
    {
      case ItemData.ItemType.Melee:
      case ItemData.ItemType.Range:
        // 최초레벨업은 레벨업이 아닌 오브젝트 생성
        if (level == 0)
        {
          GameObject newWeapon = new GameObject();
          weapon = newWeapon.AddComponent<Weapon>();
          weapon.Init(data);
        }
        else
        {
          float nextDamage = data.baseDamage;
          int nextCount = 0;

          nextDamage += data.baseDamage * data.damages[level];
          nextCount += data.counts[level];

          weapon.LevelUp(nextDamage, nextCount);
        }

        level++;
        break;
      case ItemData.ItemType.Glove:
      case ItemData.ItemType.Shoe:
        // 최초레벨업은 레벨업이 아닌 오브젝트 생성
        if (level == 0)
        {
          GameObject newGear = new GameObject();
          gear = newGear.AddComponent<Gear>();
          gear.Init(data);
        }

        else
        {
          float nextRate = data.damages[level];
          gear.LevelUp(nextRate);
        }

        level++;
        break;
      case ItemData.ItemType.Heal:
        GameManager.instance.health = GameManager.instance.maxHealth;
        break;
    }

    // 한계까지 업그레이드를 하면 더이상 업그레이드 할 수 없도록 버튼을 비활성화
    if (level == data.damages.Length)
    {
      GetComponent<Button>().interactable = false;
    }
  }
}
