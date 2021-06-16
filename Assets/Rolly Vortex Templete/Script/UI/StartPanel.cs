﻿using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
public class StartPanel : MonoBehaviour, IPointerClickHandler {
    private Button m_btnSkin;
    private Button m_btnTube;

    void Start ()
    {
        m_btnSkin = transform.Find("Skin").GetComponent<Button>();
        m_btnSkin.onClick.AddListener(OnSkinClick);
        m_btnTube = transform.Find("Tube").GetComponent<Button>();
        m_btnTube.onClick.AddListener(OnTubeClick);
        m_btnTube = transform.Find("Powerups").GetComponent<Button>();
        m_btnTube.onClick.AddListener(OnPowerClick);
    }

    private void OnSkinClick()
    {
        UIManager.Instacne.Open(UIType.SkinPanel);
    }

    private void OnTubeClick()
    {
        UIManager.Instacne.Open(UIType.TubePanel);
        UIManager.Instacne.ShowTotalCoinNum();
    }
    private void OnPowerClick()
    {
        UIManager.Instacne.Open(UIType.PowerupsPanel);
        UIManager.Instacne.ShowTotalCoinNumPow();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
        GameManager.Instacne.GameState = GameStateEnum.StartGame;
        UIManager.Instacne.Open(UIType.GamePanel);
        GameObject.FindWithTag("Player").GetComponent<PlayerMove>().GameStart();
    }
}
