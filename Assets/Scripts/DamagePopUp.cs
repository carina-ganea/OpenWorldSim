using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshPro textMesh;
    public double disappearTime = 4;
    public float disappearSpeed = 300f;
    private double start;
    private Color textColor;

    public static DamagePopUp Create(Vector3 position, int dmg, Quaternion rotation, bool crit){
        Transform dmgPopUpTransform = Instantiate(Resources.Load<Transform>("Damage"), position, rotation);
        DamagePopUp dmgPopUp = dmgPopUpTransform.GetComponent<DamagePopUp>();
        dmgPopUp.Setup(dmg, crit);
        return dmgPopUp;
    }

    private void Awake(){
        textMesh = transform.GetComponent<TextMeshPro>();
        textColor = textMesh.color;
        start = Time.timeAsDouble;
    }

    public void Setup(int number, bool crit){
        textMesh.SetText(number.ToString());
        if(crit){
            textMesh.fontSize = 6;
            textMesh.fontStyle = FontStyles.Bold;
        }
    }

    private void Update(){
        float MoveSpeed = 0.01f;
        transform.position += new Vector3(0f, 1f, 0f) * Mathf.Log((float)(Time.timeAsDouble - start + 1f)) * MoveSpeed;

        if( Time.timeAsDouble - start >= disappearTime){
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if(textColor.a <= 0){
                Destroy(gameObject);
            }
        }


    }
}
