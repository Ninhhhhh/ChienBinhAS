using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0,75,0 );
    public float timeFade = 1f;

    private float timeElapsed;
    TextMeshProUGUI textMeshPro;
    RectTransform textTranfrom;
    private Color startColor;

    private void Awake()
    {
        textTranfrom = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textTranfrom.position += moveSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;

        if(timeElapsed < timeFade)
        {
            float fadeAlpha = startColor.a * (1 - (timeElapsed / timeFade));
            textMeshPro.color = new Color(startColor.r, startColor.g,startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
