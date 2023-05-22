using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
  RectTransform rect;
  Item[] items;

  void Awake()
  {
    rect = GetComponent<RectTransform>();
    items = GetComponentsInChildren<Item>(true);
  }

  public void Show()
  {
    Next();
    rect.localScale = Vector3.one;
    GameManager.instance.Stop();

    // 캐릭터가 레벨업하고 아이템선택창이 보일때 오디오
    AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
    AudioManager.instance.EffectBgm(true);
  }
  public void Hide()
  {
    rect.localScale = Vector3.zero;
    GameManager.instance.Resume();
    // 아이템을 선택하고 꺼질때 오디오
    AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    AudioManager.instance.EffectBgm(false);
  }

  public void Select(int i)
  {
    items[i].onClick();
  }

  void Next()
  {
    // 모든아이템 비활성화
    foreach (Item item in items)
    {
      item.gameObject.SetActive(false);
    }
    // 랜덤하게 3개의 아이템을 활성화
    int[] ran = new int[3];
    while (true)
    {
      // 랜덤으로 3개의 값을 가져옴
      ran[0] = Random.Range(0, items.Length);
      ran[1] = Random.Range(0, items.Length);
      ran[2] = Random.Range(0, items.Length);

      // 3개의 값이 전부 달라야 빠져나오게함
      if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2]) break;
    }

    for (int i = 0; i < ran.Length; i++)
    {
      // 랜덤으로 뽑은 3개의 인덱스에 해당하는 아이템을 가져옴
      Item ranItem = items[ran[i]];

      // 만렙아이템의 경우는 소비아이템으로 대체
      if (ranItem.level == ranItem.data.damages.Length)
      {
        items[4].gameObject.SetActive(true);
      }
      else
      {
        // 만렙이 아니면 그대로 표시
        ranItem.gameObject.SetActive(true);
      }
    }
  }
}
