using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof (NewCarController))]
public class NewCarUserControl : MonoBehaviour
    {
        private NewCarController m_Car; // the car controller we want to use

        public bool flag = false;
        public Text gameover;
        public Text Timelabel_player;

        public GameObject oil_empty; // 텅비었~s
        public GameObject oil_full; // 가득~s
        public GameObject bk;

        public Image energe_bar_green; // 녹썍~s

        public float m_totalHp;
        public float m_minusHp;
        public float temp;

        public GameObject[] stage = new GameObject[5];
        public GameObject camera;

        public Text[] round_result = new Text[5];
        public GameObject[] box = new GameObject[14];
        public int count = 1;

        public GameObject folklift_player;
        public GameObject folklift_planner;

        public float Timecount_player = 0;
        public float next_rate = 0.0f;

    private void Start()
    {
        oil_empty.SetActive(false);
        oil_full.SetActive(false);

        m_totalHp = 100.0f;
        m_minusHp = 0.05f;
        gameover.text = "ROUND "+count.ToString();
    }

    private void Awake()
        {
        // get the car controller
        m_Car = GetComponent<NewCarController>();
        }

    private void energe_color()
    {
        if (energe_bar_green.fillAmount >= 0.6)
        {
            energe_bar_green.color = new Color(0.0f, 255.0f, 0.0f);
            if (energe_bar_green.fillAmount < 1.0f)
            {
                oil_full.SetActive(false);
            }
        }

        if (energe_bar_green.fillAmount <= 0.5 && energe_bar_green.fillAmount > 0.3)
        {
            energe_bar_green.color = new Color(241.0f, 255.0f, 0.0f);
            oil_empty.SetActive(false);
        }

        if (energe_bar_green.fillAmount <= 0.3)
        {
            energe_bar_green.color = new Color(255.0f, 0.0f, 0.0f);
            oil_empty.SetActive(true);
        }

        if (energe_bar_green.fillAmount == 0.0)
        {
            // 에너지가 없으므로 움직일 수 없습니다. 멘트가 필요하고, 무한 break 걸리게끔 해줘야된다.
        }


    }

    private void FixedUpdate()
    {
        charge();
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 체력바s 코드
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            energe_bar_green.fillAmount -= m_minusHp / m_totalHp;
            energe_color();
        }


        // pass the input to the car!

        if ((box[0].transform.localPosition.x > -14) && (box[0].transform.localPosition.x < -8)) stage[0].SetActive(true);
        if ((box[1].transform.localPosition.x > -14) && (box[1].transform.localPosition.x < -8)) stage[0].SetActive(true);

        if ((box[4].transform.localPosition.x > -14) && (box[4].transform.localPosition.x < -8)) stage[1].SetActive(true);
        if ((box[5].transform.localPosition.x > -14) && (box[5].transform.localPosition.x < -8)) stage[1].SetActive(true);

        if ((box[8].transform.localPosition.x > -14) && (box[8].transform.localPosition.x < -8)) stage[2].SetActive(true);
        if ((box[9].transform.localPosition.x > -14) && (box[9].transform.localPosition.x < -8)) stage[2].SetActive(true);

        if ((box[10].transform.localPosition.x > -14) && (box[10].transform.localPosition.x < -8)) stage[3].SetActive(true);
        if ((box[11].transform.localPosition.x > -14) && (box[11].transform.localPosition.x < -8)) stage[3].SetActive(true);

        if ((box[12].transform.localPosition.x > -14) && (box[12].transform.localPosition.x < -8)) stage[4].SetActive(true);
        if ((box[13].transform.localPosition.x > -14) && (box[13].transform.localPosition.x < -8)) stage[4].SetActive(true);


        if (count < 6)
        {
            gameover.text = "ROUND " + count.ToString();
            Timecount_player += Time.deltaTime;
            Timelabel_player.text = string.Format("{0:N2}", "Player " + Timecount_player);
        }
        else
        {
            gameover.text = "FINISH";
            Timelabel_player.enabled = false;
        }

#if !MOBILE_INPUT
        float handbrake = Input.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }

    private void charge()
    {
        float nextCharge = 2.0f;
        if (flag)
        {
            if (energe_bar_green.fillAmount <= 1.0f && Time.time > next_rate)
            {
                next_rate = Time.time + nextCharge;
                energe_bar_green.fillAmount += 0.1f;
                if (energe_bar_green.fillAmount == 1.0f)
                {
                        oil_full.SetActive(true);
                }
            }
        }
        energe_color();

    }
    private void OnTriggerExit(Collider other)
    {
        flag = false;
            if (other.gameObject == bk)
            {
                bk.transform.localScale = new Vector3(1.0f, 0.25f, 1.0f);
            }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == bk)
        {
            bk.transform.localScale = new Vector3(1.0f, 0.05f, 1.0f);

            flag = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == bk)
        {
            bk.transform.localScale = new Vector3(1.0f, 0.05f, 1.0f);
        }

        if (other.gameObject.CompareTag("Finish"))
        {
                other.gameObject.SetActive(false);
            if (count < 6) {
                temp = Timecount_player;
                count = count + 1;
            }

            if (count == 2)
            {
                round_result[0].text = temp.ToString();
                camera.transform.position = new Vector3(50.8f, 11.11f, 10.45f);
                folklift_player.transform.position = new Vector3(49.75f, 0.00715977f, 14.9f);
                folklift_planner.transform.position = new Vector3(63.2f, 0.00715977f, 14.9f);

                Timecount_player = 0;
            }
            else if (count == 3)
            {
                round_result[1].text = temp.ToString();
                camera.transform.position = new Vector3(22.5f, 15.8f, 29.48f);
                camera.transform.eulerAngles = new Vector3(60f, 0, 0);
                folklift_player.transform.position = new Vector3(13.75f, 0.00715977f, 31.9f);
                folklift_planner.transform.position = new Vector3(17f, 0.00715977f, 31.9f);

                Timecount_player = 0;
            }
            else if (count == 4)
            {
                round_result[2].text = temp.ToString();
                camera.transform.position = new Vector3(60.6f, 21.1f, 35f);
                camera.transform.eulerAngles = new Vector3(80f, 0, 0);
                folklift_player.transform.position = new Vector3(47.86f, 0.00715977f, 32.4f);
                folklift_planner.transform.position = new Vector3(51.86f, 0.00715977f, 32.4f);

                Timecount_player = 0;
            }
            else if (count == 5)
            {
                round_result[3].text = temp.ToString();
                camera.transform.position = new Vector3(23.9f, 21.3f, 55.9f);
                camera.transform.eulerAngles = new Vector3(80f, 0, 0);
                folklift_player.transform.position = new Vector3(12f, 0.00715977f, 52f);
                folklift_planner.transform.position = new Vector3(15.5f, 0.00715977f, 52f);

                Timecount_player = 0;
            }
            else if (count == 6)
            {
                round_result[4].text = temp.ToString();
                gameover.text = "FINISH";
                Timelabel_player.enabled = false;
            }

        }
    }
}

