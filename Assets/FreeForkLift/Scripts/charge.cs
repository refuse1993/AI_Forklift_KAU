using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class charge : MonoBehaviour {

    public GameObject fklift;
    public Image energe_bar;
    public float m_totalHp = 100.0f;
    public float m_minusHp = 0.02f;

    // Use this for initialization
    void Start () {
        fklift = GameObject.FindGameObjectWithTag("Vehicle");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == fklift)
        {
            if (energe_bar.fillAmount <= 1.0)
            {
                energe_bar.fillAmount += m_minusHp / m_totalHp;
            }
        }
    }

}
