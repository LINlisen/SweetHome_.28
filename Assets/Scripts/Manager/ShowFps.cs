using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFps : MonoBehaviour
{
    // Start is called before the first frame update
    public Text fpsText;
    public float deltaTime;
    [SerializeField] public int HopeFps = 90;
    private void Start()
    {
        Application.targetFrameRate = HopeFps;
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
