using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image Bar;
    public float Fill;

    void Start() {
        // Bar.fillAmount = 1f;
        // Fill = 1f;
    }    

    void Update() {
        // Fill -= Time.deltaTime * 0.1f;

        // Bar.fillAmount = Fill;
    }
    public void updateHealth (float value) {
        // while (Fill != value){
        //     Fill -= Time.deltaTime * 0.1f;

        //     Bar.fillAmount = Fill;
        // }
        Bar.fillAmount = value;
    }
}
