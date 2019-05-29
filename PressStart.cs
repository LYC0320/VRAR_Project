using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStart : MonoBehaviour
{

    public GameObject ball;

    public GameObject canvas, panel, text;

    public GameObject gameover_text;
    
    public GameObject Score;

    public ControllerGrabObject isStart;

    public Material dissolve;

    public AudioSource BGM;

    private float progress;
    private float timer = 0;
    private bool isReturn = false;

    private bool music = true;


    void Start()
    {
        progress = 1.0f;
        dissolve.SetFloat("_Progress", progress);
        BGM.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isReturn)
        {
            if (progress >= 1f)
            {
                ball.SetActive(false);
                gameover_text.SetActive(false);
            }

            // 1
            if (isStart.isStart)
            {
                //Debug.Log(isStart.isStart + "123");
                dissolve.SetFloat("_Progress", progress);
                progress -= Time.fixedDeltaTime / 4.0f;  
            }

            if (progress <= 0f)
            {
                isStart.isStart = false;
                isReturn = true;
                //progress = 0;
                //dissolve.SetFloat("_Progress", progress);
                StartGame();

            }
        }
        else
        {
            if (progress >= 1f)
            {
                isReturn = false;
                timer = 0f;
                music = true;
            }
            else if (timer > 6.0f)
            {
                progress += Time.fixedDeltaTime / 4.0f;  // spawn
                dissolve.SetFloat("_Progress", progress);

                if (music)
                {
                    BGM.Play();
                    music = false;
                }
            }
            else
            {
                ball.SetActive(false);
                timer += Time.deltaTime;
            }
        }
    }

    private void StartGame()
    {
        ball.SetActive(true);
        Score.GetComponent<UnityEngine.UI.Text>().text = "         YOU AI\nScore: 0 - 0";
        text.SetActive(false);
        panel.SetActive(false);
        canvas.SetActive(false);
    }
}
