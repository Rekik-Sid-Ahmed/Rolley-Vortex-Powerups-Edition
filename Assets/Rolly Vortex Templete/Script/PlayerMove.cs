using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private CharacterController characterController;
    private Vector3 moveDirection;//the movement of ball each frame
    private float degress = 0; // the degress of ball each frame
    private Vector3 m_CurrPos;// current pos of ball
    public float RotateSpeed = 30;// rotate spedd of ball
    public float ZSpeed = 100; // the speed the ball move on the Z Axie.
    public float R = 2.4f;//the radius of the ball rotate.
    private float m_degress = 0;// the total degress of ball
    public Animation PlayerAnimation;//the animation attach to the ball.
    private Vector3 mouseCurrentPos;//current mouse pos
    private Vector3 mousePreviousPos;//previous mouse pos
    public ParticleSystem PlayerParticle;
    public Material material;
    public bool modeSpeed = false;

    public float pushPower = 2.0F;
    public float speed = 5.0f;
    public float DestFov = 50.0f;


    //private shake shakee;


    public GameObject SpawnerObj;
    public GameObject EnemyObj;
    // Create a public reference to the enemy game object.
    //Main camera
    Camera mcamera;
    GameObject cameraObj;

    public GameObject tubeBlue;

    float t = 0.1f;

    public void start()
    {
 
    }
    public void Init()
    {
        //     mcamera = cameraObj.GetComponent<Camera>();
        this.characterController = base.GetComponent<CharacterController>();
        transform.position = new Vector3(0, -R, 9);
        if (PlayerAnimation != null)
            PlayerAnimation.Stop();
        if (PlayerParticle != null)
            PlayerParticle.Stop();
        SpawnerObj = GameObject.Find("Spawner");
        EnemyObj = GameObject.Find("Cube1");

    }

    public void GameStart()
    {
        if (PlayerAnimation != null)
            PlayerAnimation.Play();
        if (PlayerParticle != null)
            PlayerParticle.Play();
    }

    private void FixedUpdate()
    {
        // if(EnemyObj != null)
        // Debug.Log("this is "+EnemyObj.name);

        if (GameManager.Instacne.GameState != GameStateEnum.StartGame)
            return;
        degress = 0;
        if (Input.GetMouseButtonDown(0))
        {
            mouseCurrentPos = mousePreviousPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            //calculate the degress each frame when the finger swipe
            mouseCurrentPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float deltaMousePos = (mousePreviousPos.x - mouseCurrentPos.x);
            float sign = Mathf.Sign(deltaMousePos);
            float speed = Mathf.Abs(deltaMousePos * 200);

            speed = Mathf.Clamp(speed, 0, RotateSpeed);
            speed *= -sign;
            degress = speed;
            mousePreviousPos = mouseCurrentPos;
        }
        m_degress += degress;
        if (m_degress >= 360)
            m_degress -= 360;
        degress = degress / 180 * Mathf.PI;
        Vector2 temp = new Vector2(transform.position.x, transform.position.y);
        Vector2 pos = temp.normalized * R;
        //calculate the new position ,when ball rotate degress angle.
        float x = pos.x * Mathf.Cos(degress) - pos.y * Mathf.Sin(degress);
        float y = pos.x * Mathf.Sin(degress) + pos.y * Mathf.Cos(degress);
        m_CurrPos = new Vector3(x, y, 0);
        //calculate the movement of ball each frame
        moveDirection = m_CurrPos - transform.position;

        moveDirection.z = ZSpeed * Time.deltaTime;
        this.characterController.Move(this.moveDirection);
        //set the rotation when ball rotate degress angle.
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, m_degress));
        if (Spawner.Instance.BarrageZposList.Count > 0)
        {
            // check the ball whether cross the barrage, if cross ,get score
            if (transform.position.z >= Spawner.Instance.BarrageZposList[0])
            {
                Spawner.Instance.BarrageZposList.RemoveAt(0);
                ScoreManager.Instance.AddScore(1);
                AudioManager.Instacne.PlayGetScoreAudio();
                GameObject obj = ObjectPoolManager.instance.GetInstance("ScoreText");
                obj.transform.position = transform.position;
            }
        }
    }


    void OnControllerColliderHit(ControllerColliderHit col)
    {     

        //  Rigidbody body = col.collider.attachedRigidbody;
        // Vector3 pushDir = new Vector3(col.moveDirection.x, 0, col.moveDirection.z);

        
        Debug.Log("This is it" + col.gameObject.name);


        if (modeSpeed == false)
        {
            if (GameManager.Instacne.GameState == GameStateEnum.GameOver)
                return;
            GameObject o = ObjectPoolManager.instance.GetInstance("BallFx");
            o.name = "BallFx";
            o.transform.position = transform.position;
            GameManager.Instacne.GameState = GameStateEnum.GameOver;
            if (PlayerParticle != null)
                PlayerParticle.Stop();
            //shakee.camShake();

            Destroy(gameObject);
            Time.timeScale = 1f;
            AudioManager.Instacne.PlayDeathAudio();
            UIManager.Instacne.ShowResult();
        }
        else
        {

            // Debug.Log("mode speed on");
            //                body.velocity = pushDir * pushPower;

            //Destroy(col.gameObject);
            // ObjectPoolManager.instance.BackToPool(col.gameObject,"Enemy");
            // col.gameObject.SetActive(false);


        }


    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Time")
        {
            Time.timeScale = 0.5f;
            Destroy(other.gameObject);
            StartCoroutine("DeStopTime");
        }
        if (other.gameObject.tag == "Bolt")
        {

            this.gameObject.transform.GetChild(4).gameObject.SetActive(true);

            modeSpeed = true;
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 90f, Time.deltaTime * speed); ;
            Debug.Log("This is amera" + cameraObj);
            ZSpeed = 100;
            Destroy(other.gameObject);
            StartCoroutine("DeStopBolt");

        }

    }

    IEnumerator DeStopTime()
    {
        tubeBlue = GameObject.Find("Tube");


        SpawnerObj.GetComponent<Spawner>().stopT = true;
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 1f;
        SpawnerObj.GetComponent<Spawner>().stopT = false;



    }

    IEnumerator DeStopBolt()
    {
        tubeBlue = GameObject.Find("Tube");

        yield return new WaitForSeconds(3f);
        ZSpeed = 25f;
        Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, 60f, Time.deltaTime * speed); ;


        StartCoroutine("PauseBolt");



    }
    IEnumerator PauseBolt()
    {
        yield return new WaitForSeconds(1.5f);
        ZSpeed = 25;
        modeSpeed = false;


        this.gameObject.transform.GetChild(4).gameObject.SetActive(false);





    }

}
