using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public  Boolean StopTime=false;
    private static GameManager instance;
    public static GameManager Instacne
    {
        get
        {
            return instance;
        }
    }
    public GameStateEnum GameState = 0;
    private int continueTimes = 0;
    public GameObject[] BallItemList;
    public int SkinIndex;
    private bool bContinue = false;
    public Material Tube;
    public SkinPanel skinpanelinstance;
    public GameObject PlayerObject;
    public Transform[] Particles;
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        continueTimes = UnityEngine.Random.Range(0, 2);
        continueTimes = 1;
        int skinIndex = PlayerPrefs.GetInt(PlayerPrefTag.SkinIndex);
        GeneratePlayer(skinIndex);
        int tubetex = PlayerPrefs.GetInt(PlayerPrefTag.TubeTexture);
        SetTubeMaterial(tubetex);

    }
   
    public void GeneratePlayer(int index)
    {
        if (PlayerObject != null)
            Destroy(PlayerObject);
        GameObject o = Resources.Load("BallSkin/" + index.ToString()) as GameObject;
        PlayerObject = Instantiate(o);
        PlayerObject.GetComponent<PlayerMove>().Init();
        Camera.main.GetComponent<CameraFollowController>().PlayerTransform = PlayerObject.transform;
        Spawner.Instance.PlayerTransform = PlayerObject.transform;
          Debug.Log("player"+o.transform.position);
       // GameObject effect= GameObject.Find("ElecEffect");
       // effect.transform.SetParent(o.transform,false);
        //effect.transform.localPosition= new Vector3(0f, 0f, 0f);
         //Debug.Log("particle"+Particles[0].transform.position);



    }

    public void StartGame()
    {
        SkinIndex = PlayerPrefs.GetInt(PlayerPrefTag.SkinIndex);
        GameState = GameStateEnum.StartGame;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }


    public void Continue()
    {
        if (continueTimes <= 0)
            return;
    }


    public void ContinueGame()
    {
        GameState = GameStateEnum.StartGame;
    }


    public void ShowGameResult()
    {
        //ScoreManager.Instance.ShowGameResult();
    }

    public void SetTubeMaterial(int index)
    {
        string path = "TubeTexture/" + index.ToString();
        Tube.mainTexture = Resources.Load(path, typeof(Texture)) as Texture;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
