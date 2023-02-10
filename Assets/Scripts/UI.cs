using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterStats))]
public class UI : MonoBehaviour
{
    public GameObject hp_ui;
    public GameObject stamina_ui;
    public Transform target;

    Transform HP;
    Transform ST;
    Image healthSlider;
    Image staminaSlider;


    // Start is called before the first frame update
    void Start()
    {
        Canvas c = GameObject.Find("Display").GetComponent<Canvas>();
        {
            if (c.renderMode == RenderMode.ScreenSpaceOverlay){
                HP = Instantiate(hp_ui, c.transform).transform;
                ST = Instantiate(stamina_ui, c.transform).transform;
                healthSlider = HP.GetChild(0).GetChild(0).GetComponent<Image>();
                staminaSlider = ST.GetChild(0).GetComponent<Image>();
            }
        }

        GetComponent<CharacterStats>().OnHealthChange += OnHealthChange;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnHealthChange(int HP, int currHP){
        healthSlider.fillAmount = currHP / (float)HP;
    }
}
