using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BuyPower : MonoBehaviour
{
    // Start is called before the first frame update
    private Image Icon;
    private Button btn_Cancel;
    private Button btn_OK;
    private TubeTexture tube;
    private PowerPrice power;
    private Text PriceText;
    // Use this for initialization
    void Awake () {
        Icon = transform.Find("MessageBox1/Icon1").GetComponent<Image>();
        PriceText = Icon.transform.Find("Price").GetComponent<Text>();
        btn_Cancel = transform.Find("MessageBox1/Cancel1").GetComponent<Button>();
        btn_OK = transform.Find("MessageBox1/OK1").GetComponent<Button>();
        btn_Cancel.onClick.AddListener(Cancel);
        btn_OK.onClick.AddListener(BuyPow);
    }

    void Buy()
    {
        int coin = PlayerPrefs.GetInt(PlayerPrefTag.Coin);
        if(coin >= tube.Price)
        {
            coin -= tube.Price;
            ScoreManager.Instance.ConsumeCoin(tube.Price);
            tube.BuySuccess();
            gameObject.SetActive(false);
        }
    }
      void BuyPow()
    {   
        int coin = PlayerPrefs.GetInt(PlayerPrefTag.Coin);
        if(coin >= power.Price)
        {
            coin -= power.Price;
            ScoreManager.Instance.ConsumeCoinPow(power.Price);
         
            power.BuySuccess();
            gameObject.SetActive(false);
        }
    }

    void Cancel()
    {
       // Debug.Log("Canceeeel");
        //UIManager.Instacne.Close(UIType.BuyPower);
        this.gameObject.SetActive(false);
    }

    public void Show(TubeTexture tube)
    {
        this.tube = tube;
        gameObject.SetActive(true);
        Icon.sprite = tube.Icon.sprite;
        int coin = PlayerPrefs.GetInt(PlayerPrefTag.Coin);
        PriceText.text = coin.ToString() + "/" + tube.Price.ToString();
    }
    public void ShowPow(PowerPrice power)
    {
        this.power = power;
        gameObject.SetActive(true);
        Icon.sprite = power.Icon.sprite;
        int coin = PlayerPrefs.GetInt(PlayerPrefTag.Coin);
        PriceText.text = coin.ToString() + "/" + power.Price.ToString();
    }
}
