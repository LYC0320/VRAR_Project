using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_script_new : MonoBehaviour
{

    enum BallState { player_racket, player_table, oppo_table }
    public enum Turn { player, AI }

    public GameObject floor;
    public GameObject table;
    public GameObject racket;
    public GameObject hand;
    public GameObject AI_hitbox;
    public GameObject score_text;
    public GameObject deuce_text;
    public GameObject gameover_text;

    public GameObject Canvas;
    public GameObject Canvas_panel;
    public GameObject Canvas_text;

    public GameObject ball;

    public AudioSource win_sound;
    public AudioSource lose_sound;

    public Material win_material;
    public Material lose_material;

    private Rigidbody rb;
    private Vector3 init_pos;
    private UnityEngine.UI.Text tm;
    private BallState now_state;
    public Turn now_turn;

    public int player_point = 0;
    public int AI_point = 0;
    private int round = 0;
    public bool end;
    private bool first = false;
    private bool deuce = false;
    private float timer = 0;
    private string prefix = "        YOU  AI\nScore : ";

    public Ball myHit;
    public ControllerGrabObject isStart;

    public soundManager SM;

    // Use this for initialization
    void Start()
    {
        now_state = BallState.player_racket;
        now_turn = Turn.player;
        init_pos = gameObject.transform.position;
        tm = score_text.GetComponent<UnityEngine.UI.Text>();
        end = true;
        first = false;
        deuce = false;
        timer = 0;
        rb = GetComponent<Rigidbody>();
        tm.text = prefix + player_point.ToString() + " - " + AI_point.ToString();
        //player_point = 0;
        //AI_point = 0;
        round = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (end)
        {
            timer += Time.deltaTime;
            
            if (timer > 1f && first)
            {
                ball.GetComponent<TrailRenderer>().enabled = false;
                //Debug.Log("reset position");
                first = false;
                transform.position = init_pos;

                //float trailTimer = 0f;

                //while (true)
                //{
                //    trailTimer += Time.deltaTime;
                //    Debug.Log("ddddd");
                //}

                rb.isKinematic = false;
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 0, 0);

                hand.SetActive(true);


                isStart.isGrab = false;
                isStart.isRelease = false;
                isStart.objectInHand = null;
                isStart.collidingObject = null;


                

                /*if (now_turn == Turn.AI)
                {
                    timer = 0;
                    end = false;
                    first = true;
                }*/
            }
        }

        if (isStart.isGrab && isStart.isRelease && timer > 1f)
        {
            //Debug.Log("Exit");
            timer = 0;
            end = false;
            first = true;
            hand.SetActive(false);
        }

        if (isStart.isGrab)
        {
            ball.GetComponent<TrailRenderer>().enabled = true;
        }
        /* if (now_turn == Turn.player)
         {
             Debug.Log("Turn : Player");
             if (now_state == BallState.player_racket)
             {
                 Debug.Log("State : racket");
             }
             else
             {
                 Debug.Log("State : oppo table");
             }
         }
         else
         {
             if (now_turn == Turn.AI)
             {
                 Debug.Log("Turn : AI");
                 if (now_state == BallState.player_racket)
                 {
                     Debug.Log("State : racket");
                 }
                 else
                 {
                     Debug.Log("State : oppo table");
                 }
             }
         }*/
    }


    void OnTriggerEnter(Collider collision)
    {
        if (!end)
        {
            switch (now_state)
            {
                case BallState.player_racket:
                    if (collision.name == "BeachBat3" || collision.name == "BeachBat3_Backhand")
                    {
                        if (now_turn == Turn.player)
                        {
                            /*if (round == 0)
                            {
                                now_state = BallState.player_table;
                            }
                            else*/
                            {
                                now_state = BallState.oppo_table;
                            }
                        }
                        else
                        {
                            Lose(now_turn);
                        }
                    }
                    else if (collision.gameObject == AI_hitbox)
                    {
                        if (now_turn == Turn.AI)
                        {
                            /*if (round == 0)
                            {
                                now_state = BallState.player_table;
                            }
                            else*/
                            {
                                now_state = BallState.oppo_table;
                            }
                        }
                        else
                        {
                            Lose(now_turn);
                        }
                    }
                    else
                    {
                        Lose(now_turn);
                    }
                    break;
                case BallState.player_table:
                    if (collision.name == "PBR_Table")
                    {
                        if (now_turn == BallTableIndex())
                        {
                            now_state = BallState.oppo_table;
                        }
                        else
                        {
                            Lose(now_turn);
                        }
                    }
                    else
                    {
                        Lose(now_turn);
                    }
                    break;
                case BallState.oppo_table:
                    //if (collision.name == "PBR_Table" && hitbox.hasCollided)
                    // hitbox 

                    // OnTrigger{
                            // if ball trigger
                                // hitbox.hasCollided - true;
                    //}

                    // hitbox.hadCollided
                    if (collision.name == "PBR_Table")
                    {
                        //Debug.Log("hit table");
                        if (now_turn != BallTableIndex())
                        {
                            round++;
                            now_state = BallState.player_racket;
                            if (now_turn == Turn.AI)
                            {
                                now_turn = Turn.player;
                            }
                            else
                            {
                                now_turn = Turn.AI;
                            }
                        }
                        else
                        {
                            Lose(now_turn);
                        }
                    }
                    else
                    {
                        Lose(now_turn);
                    }
                    break;
                default:
                    break;
            }
        }
        else if(timer > 1)
        {

        }
    }

    void Lose(Turn turn)
    {
        if (turn == Turn.AI)
        {
            win_sound.Play();
            //Debug.Log("AI lose");
            player_point++;
            now_turn = Turn.player;
        }
        else
        {
            lose_sound.Play();
            //Debug.Log("player lose");
            AI_point++;
            now_turn = Turn.player;
        }

        tm.text = prefix + player_point.ToString() + " - " + AI_point.ToString();
        now_state = BallState.player_racket;
        end = true;
        round = 0;
        //hand.SetActive(true);

        if (!deuce)
        {
            if (player_point == 10 && AI_point == 10)
            {
                deuce = true;
                deuce_text.SetActive(true);
            }
            else if (player_point == 11)
            {
                GameOver(Turn.player);
            }
            else if (AI_point == 11)
            {
                GameOver(Turn.AI);
            }
        }
        else
        {
            if (player_point - AI_point == 2)
            {
                GameOver(Turn.player);
            }
            else if (player_point - AI_point == -2)
            {
                GameOver(Turn.AI);
            }
        }
    }

    Turn BallTableIndex()
    {
        Vector3 now_pos = gameObject.transform.position;

        if (/*now_pos.z >= -32.678f && now_pos.z <= -31.177f && now_pos.x <= 2.132f && */now_pos.x >= 0.66f)//0.6361f)
        {
            //Debug.Log("player table");
            return Turn.player;
        }
        else
        {
            //Debug.Log("AI table");
            return Turn.AI;
        }
    }

    void GameOver(Turn winner)
    {
        if (winner == Turn.player)
        {
            gameover_text.GetComponent<Renderer>().material = win_material;
            SM.winner = 1;
        }
        else
        {
            gameover_text.GetComponent<Renderer>().material = lose_material;
            SM.winner = 2;
        }
        hand.SetActive(true);


        isStart.isGrab = false;
        isStart.isRelease = false;
        isStart.objectInHand = null;
        isStart.collidingObject = null;


        gameover_text.SetActive(true);
        Canvas_text.SetActive(true);
        Canvas_panel.SetActive(true);
        Canvas.SetActive(true);
        reset();
        //gameObject.SetActive(false);
    }

    void reset()
    {
        now_state = BallState.player_racket;
        now_turn = Turn.player;
        timer = 0;
        player_point = 0;
        AI_point = 0;
        //tm.text = prefix + player_point.ToString() + " - " + AI_point.ToString();
        end = true;
        first = false;
        deuce = false;
        transform.position = init_pos;
        rb.isKinematic = false;
        rb.velocity = new Vector3(0, 0, 0);
        isStart.isGrab = false;
        isStart.isRelease = false;

        deuce_text.SetActive(false);

        //SM.winner = 0;
    }
}
