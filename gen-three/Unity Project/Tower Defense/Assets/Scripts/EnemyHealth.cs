﻿using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float UnitHealth = 50f;
    public float FullHealth = 50f;
    public LineRenderer bar;
    Vector3[] healthbarTransform = new Vector3[2];
    private float OriginalScale;
    Vector3[] barPositons = new Vector3[2];

    void Start()
    {
		UnitHealth = FullHealth;
        Vector3 thisPos = this.transform.position;
        healthbarTransform[0] = new Vector3(-0.50f, 0.50f); //start of hp bar
        healthbarTransform[1] = new Vector3(0.50f, 0.50f);  //end of hp bar
        this.gameObject.AddComponent<LineRenderer>();
        bar = this.gameObject.GetComponent<LineRenderer>();
        bar.material = new Material(Shader.Find("Particles/Additive"));
        bar.SetColors(new Color(0f, 256f, 0f), new Color(0f, 256f, 0f));
        bar.SetWidth(0.20f, 0.20f);
    }

    void Update()
    {
        if (UnitHealth < FullHealth)
        {
            healthbarTransform[0] = new Vector3(-0.50f + (((float)(FullHealth - UnitHealth) / 100) / 2), 0.50f); //start of hp bar
            healthbarTransform[1] = new Vector3(0.50f - (((float)(FullHealth - UnitHealth) / 100) / 2), 0.50f);  //end of hp bar
        }

        barPositons[0] = this.transform.position + healthbarTransform[0];
        barPositons[1] = this.transform.position + healthbarTransform[1];
        bar.SetPositions(barPositons);

        if (UnitHealth < 30)
        {
            bar.SetColors(Color.red, Color.red);
        }
    }

    public float blast(int dmg)
    {
		UnitHealth = (float)UnitHealth - dmg;
        return UnitHealth;
    }
}