using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour {

    public int winner; // 0 noWinner 1 Player 2 AI
    private bool isPlay;
    public AudioSource win, lose;

	// Use this for initialization
	void Start () {
        isPlay = false;

    }
	
	// Update is called once per frame
	void Update ()
    {

        if (winner == 0)
        {
            isPlay = false;
        }
        else if(winner == 1 && !isPlay)
        {
            win.Play();
            isPlay = true;
            winner = 0;
        }
        else if(winner == 2 && !isPlay)
        {
            lose.Play();
            isPlay = true;
            winner = 0;
        }
	}
    
}
