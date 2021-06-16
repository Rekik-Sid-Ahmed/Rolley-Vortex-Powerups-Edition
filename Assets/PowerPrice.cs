using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PowerPrice : MonoBehaviour, IPointerClickHandler
{
   [HideInInspector]
    public Image Icon;
    [HideInInspector]
    public Image Check;
    private Transform Mask;
    private Text PriceText;
    public int SkinIndex = 0;
    public int Price;
    private bool bOwned = false;
	// Use this for initialization
	void Awake () {
        Mask = transform.Find("mask");
        PriceText = Mask.Find("Text").GetComponent<Text>();
        Icon = transform.Find("Icon").GetComponent<Image>();
        Check = transform.Find("check").GetComponent<Image>();
        
	}
	
    void OnEnable()
    {
        RefreshUI();
    }

    public void BuySuccess()
    {
        Debug.Log("success bought");
        string power = PlayerPrefs.GetString(PlayerPrefTag.Powerup);
        power = power + '$' + SkinIndex.ToString();
        PlayerPrefs.SetString(PlayerPrefTag.Powerup, power);
        RefreshUI();
    }

    public void RefreshUI()
    { 
        PriceText.text = Price.ToString();
        Check.gameObject.SetActive(false);

        if (!PlayerPrefs.HasKey(PlayerPrefTag.Powerup))
        {
            PlayerPrefs.SetString(PlayerPrefTag.Powerup, "0");
        }
        if (!PlayerPrefs.HasKey(PlayerPrefTag.PowerSpeed))
        {
            PlayerPrefs.SetInt(PlayerPrefTag.PowerSpeed, 0);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefTag.PowerTime))
        {
            PlayerPrefs.SetInt(PlayerPrefTag.PowerTime, 0);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefTag.PowerDouble))
        {
            PlayerPrefs.SetInt(PlayerPrefTag.PowerDouble, 0);
        }

        string power = PlayerPrefs.GetString(PlayerPrefTag.Powerup);
        // Debug.Log("power is"+power) ;
        string[] powers = power.Split('$');
        bOwned = false;
        for (int i=0; i<powers.Length; i++)
        {

            // Check if the
            if(int.Parse(powers[i]) == SkinIndex)
            {// Debug.Log("power is"+power);
                bOwned = true;
                
                    Check.gameObject.SetActive(true);
                 
                break;
            }
            
        }
        if(bOwned)
        {
            Mask.gameObject.SetActive(false);
        }
        else
        {
            Mask.gameObject.SetActive(true);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    { Debug.Log("Clicked");
        if(bOwned)
        {
           //GameManager.Instacne.SetTubeMaterial(SkinIndex);

           switch(SkinIndex)
           {
             case 1:PlayerPrefs.SetInt(PlayerPrefTag.PowerSpeed, 1); break;  
             case 2:PlayerPrefs.SetInt(PlayerPrefTag.PowerTime, 1); break;  
             case 3:PlayerPrefs.SetInt(PlayerPrefTag.PowerDouble, 1); break;  

           }
            
           // Debug.Log("double is "+PlayerPrefs.GetInt(PlayerPrefTag.PowerDouble));
            
           UIManager.Instacne.Close(UIType.PowerupsPanel);
            Debug.Log("Here is bought");
        }
        else
        {
            Debug.Log("double is "+PlayerPrefTag.PowerDouble);
           UIManager.Instacne.ShowBuyPanelPow(this);
           Debug.Log("Here is  NOOT bought");
        }
    }
}
