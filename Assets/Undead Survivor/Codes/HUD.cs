using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
  public enum InfoType { Exp, Level, Kill, Time, Health }
  public InfoType type;

  Text myText;
  Slider mySlider;
  void Awake()
  {
    myText = GetComponent<Text>();
    mySlider = GetComponent<Slider>();
  }

  void LateUpdate()
  {
    switch (type)
    {
      case InfoType.Exp:
        float curExp = GameManager.instance.exp;
        float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
        mySlider.value = curExp / maxExp;
        break;
      case InfoType.Level:
        // F0은 소수점 자리지정 (F1, F2...) 소수점 첫째자리, 둘째자리..
        myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
        break;
      case InfoType.Kill:
        myText.text = string.Format("{0:F0}", GameManager.instance.kill);
        break;
      case InfoType.Time:
        // 남은시간
        float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
        // 분
        int min = Mathf.FloorToInt(remainTime / 60);
        // 초
        int sec = Mathf.FloorToInt(remainTime % 60);
        // D는 자릿수지정 D2의 경우 수치가 없더라도 00으로 표현 (D0, D1, D2 ...)
        myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
        break;
      case InfoType.Health:
        float curHealth = GameManager.instance.health;
        float maxHealth = GameManager.instance.maxHealth;
        mySlider.value = curHealth / maxHealth;
        break;
    }
  }
}
