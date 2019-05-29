using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    Rigidbody ballRig;
    public GameObject racketObj;
    public TableNormal table, racket;
    public float Elasticity_Coefficient = 0.9f;
    public Reflection reflection;
    public GameObject midPointObject;
    public GameObject testBall;
    public ball_script_new myEnd;
    public HandShock HS;
    public bool hit = false;

    AudioSource pingSound;
    public AudioClip ball2Table, ball2Racket;

    Vector3 gravityVec3 = new Vector3(0f, -9.8f, 0f);
    Vector3 racketPrevPos, racketPreAngular, racketVelocity, racketAngularVelocity;

    public List<Vector3> bezierCurvePoints = new List<Vector3>();
    int pointNum = 0;
    public float k = 0f, t = 0f, myTime = 0f, hitTime = 0f;
    public int whoHit;
    float hitCoef = 2.3f, bezierStep = 0.02f, test = 0f, testNum = 1f;

    public bool shock = false, hitTimeStart = false;

    public ParticleSystem hitEffect;

    float p;

    public VelocityManager VM;
    private bool isServe = false;
    private int serveSoundCount = 0;
    private bool serveSoundBool = false;
    private float skillTimeStep = 0.002f, skillTimer = 0f;

    public bool cheating = false;
    private float tableHeightPlusBallRadius = 0.8850001f;

    void Start ()
    {
        ballRig = GetComponent<Rigidbody>();
        racketPrevPos = racketObj.transform.position;
        racketPreAngular = racketObj.transform.eulerAngles;
        pingSound = GetComponent<AudioSource>();
        isServe = true;
    }

    private void FixedUpdate()
    {
       
        gravity();

        // avoid the ball penetrating
        if (Mathf.Abs(ballRig.velocity.y) < 0.3f && Mathf.Abs(ballRig.velocity.x) < 0.3f) 
        {
            GetComponent<Collider>().isTrigger = false;
        }
        else
        {
            GetComponent<Collider>().isTrigger = true;
        }

        if (HS.isSkill)
        {
            if (skillTimer > skillTimeStep)
            {
                ballMovement();
                skillTimer = 0f;
                Time.timeScale = 0.3f;
            }
            else
            {
                skillTimer += Time.fixedDeltaTime;
            }
        }
        else
        {
            ballMovement();
            Time.timeScale = 0.8f;
        }
        

        if (myEnd.end)
        {
            isServe = true;
            HS.isSkill = false;
        }
    }


    void Update()
    {
        racketVelocity = (racketObj.transform.position - racketPrevPos) / (120f * Time.deltaTime);
        racketAngularVelocity = (racketObj.transform.eulerAngles - racketPreAngular) / (120f * Time.deltaTime);

        if (myTime > 120f * Time.deltaTime)
        {
            racketPrevPos = racketObj.transform.position;
            racketPreAngular = racketObj.transform.eulerAngles;
            myTime = 0f;
        }

        myTime += Time.deltaTime;


        if (hitTimeStart)
        {
            hitTime += Time.deltaTime;
        }
        else
        {
            hitTime = 0f;
        }

        isCheating();
        
    }

    private void gravity()
    {
        ballRig.AddForce(gravityVec3);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 responseSpeed;

        if (other.name == "PBR_Table" || other.name == "WallBottom" || other.name == "little_glass_table") 
        {
            // R = I-2(I dot N)*N
            responseSpeed = ballRig.velocity - 2 * (Vector3.Dot(ballRig.velocity, table.normal)) * table.normal;
            responseSpeed *= Elasticity_Coefficient;
            ballRig.velocity = responseSpeed;

            soundPlay(ball2Table);

            ballRig.isKinematic = false;

            //bezierCurvePoints.Clear();
        }
        else if (other.name == "BeachBat3")
        {
          
            //Debug.Log(Vector3.Dot(VM.velocity, VM.velocity));

            soundPlay(ball2Racket);

            bezierCurvePoints.Clear();
            pointNum = 0;
            ballRig.isKinematic = true;

            if (!isServe)
            {

                Vector3 startingPoint = transform.position;
                Vector3 midPoint = midPointObject.transform.position;
                Vector3 endPoint = reflection.getRandomPoint();
                //Vector3 y = new Vector3(0f, 1f, 0f);
                //Vector3 midPointNormal;

                if (!cheating)
                {
                    endPoint.z = startingPoint.z + (racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.4f + 0.4f)).z;
                    endPoint.x = startingPoint.x + (racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.4f + 0.4f)).x;
                    //endPoint.z += startingPoint.z + (racket.normal * (Vector3.Dot(VM.angularVelocity, VM.angularVelocity) * 0.1f)).z;
                    //endPoint.x += startingPoint.x + (racket.normal * (Vector3.Dot(VM.angularVelocity, VM.angularVelocity) * 0.1f)).x;
                    endPoint.y += racket.normal.y;

                    midPoint.z = (startingPoint.z + endPoint.z) / 2f;
                    midPoint.x = (startingPoint.x + endPoint.x) / 2f;
                    midPoint.y += racket.normal.y;

                    Debug.Log("V:" + Vector3.Dot(VM.velocity, VM.velocity));
                    Debug.Log("AV:" + Vector3.Dot(VM.angularVelocity, VM.angularVelocity));

                }
                else
                {
                    endPoint.z = startingPoint.z + (racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.1f + hitCoef)).z;
                    endPoint.x = startingPoint.x + (racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.1f + hitCoef)).x;
                    midPoint.z = (startingPoint.z + endPoint.z) / 2f;
                    midPoint.x = (startingPoint.x + endPoint.x) / 2f;
                }

                //endPoint.z = startingPoint.z + (racket.normal * (Vector3.Dot(racketVelocity, racketVelocity) * 0.8f + hitCoef)).z;
                //endPoint.x = startingPoint.x + (racket.normal * (Vector3.Dot(racketVelocity, racketVelocity) * 0.8f + hitCoef)).x;

                //endPoint.z = startingPoint.z + (racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.4f + 0.9f)).z;
                //endPoint.x = startingPoint.x + (racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.4f + 0.9f)).x;

                //endPoint = startingPoint + (racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.4f + 0.9f));
                //endPoint.y -= 0.25f;

                //endPoint.y = startingPoint.y + (racket.normal * (Vector3.Dot(racketVelocity, racketVelocity) * 0.5f + hitCoef)).y;
                //midPoint.z = (startingPoint.z + endPoint.z) / 2f;
                //midPoint.x = (startingPoint.x + endPoint.x) / 2f;

                //midPoint = (endPoint - startingPoint) * 1f / 2f + startingPoint;

                //float cos = Vector3.Dot(y, endPoint - startingPoint) / (Vector3.Distance(Vector3.zero, endPoint - startingPoint));
                //midPointNormal = y - cos * (endPoint - startingPoint);
                //midPointNormal = midPointNormal.normalized;

                //midPoint += midPointNormal * 0.55f;

                while (k <= 1f)
                {
                    Vector3 tempPoint;
                    tempPoint = (1f - k) * (1f - k) * startingPoint + 2f * k * (1f - k) * midPoint + k * k * endPoint;
                    if (tempPoint.y >= tableHeightPlusBallRadius)
                    {
                        bezierCurvePoints.Add(tempPoint);
                    }
                    else
                    {
                        //break;
                    }
                    k += bezierStep;
                    //Instantiate(testBall, tempPoint, Quaternion.identity);
                }
            }
            else
            {
                Serve(false);
                isServe = false;
            }

            

            k = 0f;

            whoHit = 0;

            shock = true;

            hit = false;

            HS.isSkill = false;
        }
        else if (other.name == "BeachBat3_Backhand")
        {

            soundPlay(ball2Racket);

            bezierCurvePoints.Clear();
            pointNum = 0;
            ballRig.isKinematic = true;

            if (!isServe)
            {


                Vector3 startingPoint = transform.position;
                Vector3 midPoint = midPointObject.transform.position;
                Vector3 endPoint = reflection.getRandomPoint();

                //endPoint.z = startingPoint.z + (-racket.normal * (Vector3.Dot(racketVelocity, racketVelocity) * 0.8f + hitCoef)).z;
                //endPoint.x = startingPoint.x + (-racket.normal * (Vector3.Dot(racketVelocity, racketVelocity) * 0.8f + hitCoef)).x;

                if (!cheating)
                {
                    endPoint.z = startingPoint.z + (-racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.4f + 0.4f)).z;
                    endPoint.x = startingPoint.x + (-racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.4f + 0.4f)).x;
                    //endPoint.z += startingPoint.z + (-racket.normal * (Vector3.Dot(VM.angularVelocity, VM.angularVelocity) * 0.1f)).z;
                    //endPoint.x += startingPoint.x + (-racket.normal * (Vector3.Dot(VM.angularVelocity, VM.angularVelocity) * 0.1f)).x;
                    endPoint.y += -racket.normal.y;

                    midPoint.z = (startingPoint.z + endPoint.z) / 2f;
                    midPoint.x = (startingPoint.x + endPoint.x) / 2f;
                    midPoint.y += -racket.normal.y;

                    Debug.Log("V:" + Vector3.Dot(VM.velocity, VM.velocity));
                    Debug.Log("AV:" + Vector3.Dot(VM.angularVelocity, VM.angularVelocity));

                }
                else
                {
                    endPoint.z = startingPoint.z + (-racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.1f + hitCoef)).z;
                    endPoint.x = startingPoint.x + (-racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.1f + hitCoef)).x;
                    midPoint.z = (startingPoint.z + endPoint.z) / 2f;
                    midPoint.x = (startingPoint.x + endPoint.x) / 2f;
                }

                while (k <= 1f)
                {
                    Vector3 tempPoint;
                    tempPoint = (1f - k) * (1f - k) * startingPoint + 2f * k * (1f - k) * midPoint + k * k * endPoint;
                    if (tempPoint.y >= tableHeightPlusBallRadius)
                    {
                        bezierCurvePoints.Add(tempPoint);
                    }
                    else
                    {
                        //break;
                    }
                    k += bezierStep;
                    //Instantiate(testBall, tempPoint, Quaternion.identity);
                }
            }
            else
            {
                Serve(true);
                isServe = false;
            }
            k = 0f;

            whoHit = 0;

            shock = true;

            hit = false;

            HS.isSkill = false;
        }
        else if (other.name == "WallLeft")
        {
            Vector3 temp = new Vector3(1f, 0f, 0f);

            responseSpeed = ballRig.velocity - 2 * (Vector3.Dot(ballRig.velocity, temp)) * temp;
            responseSpeed *= Elasticity_Coefficient;
            ballRig.velocity = responseSpeed;
        }
        else if (other.name == "WallRight")
        {
            Vector3 temp = new Vector3(-1f, 0f, 0f);

            responseSpeed = ballRig.velocity - 2 * (Vector3.Dot(ballRig.velocity, temp)) * temp;
            responseSpeed *= Elasticity_Coefficient;
            ballRig.velocity = responseSpeed;
        }
        else if (other.name == "WallFront")
        {
            Vector3 temp = new Vector3(0f, 0f, -1f);

            responseSpeed = ballRig.velocity - 2 * (Vector3.Dot(ballRig.velocity, temp)) * temp;
            responseSpeed *= Elasticity_Coefficient;
            ballRig.velocity = responseSpeed;
        }
        else if (other.name == "WallBack")
        {
            Vector3 temp = new Vector3(0f, 0f, 1f);

            responseSpeed = ballRig.velocity - 2 * (Vector3.Dot(ballRig.velocity, temp)) * temp;
            responseSpeed *= Elasticity_Coefficient;
            ballRig.velocity = responseSpeed;
        }
        else if (other.name == "HitBox")
        {
            hitTimeStart = true;

            p = Random.Range(0f, 1f);

            if (hitTime > Random.Range(0f, 2f))
            {
                bezierCurvePoints.Clear();
                pointNum = 0;

                Vector3 startingPoint = transform.position;
                Vector3 midPoint = midPointObject.transform.position;
                Vector3 endPoint = reflection.getRandomPoint();
                midPoint.z = (startingPoint.z + endPoint.z) / 2f;

                while (k <= 1f)
                {
                    Vector3 tempPoint;
                    tempPoint = (1f - k) * (1f - k) * startingPoint + 2f * k * (1f - k) * midPoint + k * k * endPoint;
                    bezierCurvePoints.Add(tempPoint);
                    k += bezierStep;
                    //Instantiate(testBall, tempPoint, Quaternion.identity);
                }

                ballRig.isKinematic = true;
                k = 0f;

                whoHit = 1;

                hitEffect.Play();

                hitEffect.transform.position = transform.position;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        

        if (other.name == "HitBox")
        {
            if (hitTime > Random.Range(0f, 0.5f) && !myEnd.end && p < 0.925f) 
            {
                hit = true;

                //Debug.Log(p);

                bezierCurvePoints.Clear();
                pointNum = 0;

                Vector3 startingPoint = transform.position;
                Vector3 midPoint = midPointObject.transform.position;
                Vector3 endPoint = reflection.getRandomPoint();
                midPoint.z = (startingPoint.z + endPoint.z) / 2f;

                while (k <= 1f)
                {
                    Vector3 tempPoint;
                    tempPoint = (1f - k) * (1f - k) * startingPoint + 2f * k * (1f - k) * midPoint + k * k * endPoint;
                    bezierCurvePoints.Add(tempPoint);
                    k += bezierStep;
                    //Instantiate(testBall, tempPoint, Quaternion.identity);
                }

                ballRig.isKinematic = true;
                k = 0f;

                whoHit = 1;


                hitEffect.Play();

                hitEffect.transform.position = transform.position;

                hitTimeStart = false;
            }

            if (myEnd.end)
            {
                hitTimeStart = false;
            }
        }
    }

    // 球靜止在球拍上的碰撞
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.name == "BeachBat3" && racketVelocity.y > 1f)
        {
            // 要針對受力方向做加速

            Vector3 tempVec;
            if (Vector3.Dot(racketVelocity, racketVelocity) < 2f)
            {
                tempVec = Vector3.Dot(racketVelocity, racketVelocity) * racket.normal * 4f;
            }
            else
            {
                tempVec = 2f * racket.normal * 4f;
            }

            ballRig.velocity = tempVec;
        }
    }

    // 球靜止在球拍上的碰撞
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "BeachBat3" && racketVelocity.y > 1f)
        {
            // 要針對受力方向做加速
            Vector3 tempVec;
            if (Vector3.Dot(racketVelocity, racketVelocity) < 2f)
            {
                tempVec = Vector3.Dot(racketVelocity, racketVelocity) * racket.normal * 4f;
            }
            else
            {
                tempVec = 2f * racket.normal * 4f;
            }

            ballRig.velocity = tempVec;
        }
    }

    private void ballMovement()
    {
        if (ballRig.isKinematic == true && pointNum < bezierCurvePoints.Count)
        {
            transform.position = bezierCurvePoints[pointNum];
            pointNum++;

            if (pointNum == serveSoundCount && serveSoundBool)
            {
                soundPlay(ball2Table);
                serveSoundBool = false;
            }
        }
        else if (pointNum != 0 && pointNum >= bezierCurvePoints.Count)
        {
            ballRig.isKinematic = false;

            if (whoHit == 1) // HitBox
            {

                if (Vector3.Dot((bezierCurvePoints[bezierCurvePoints.Count - 1] - bezierCurvePoints[bezierCurvePoints.Count - 4]) / (3f * Time.fixedDeltaTime)
                    , (bezierCurvePoints[bezierCurvePoints.Count - 1] - bezierCurvePoints[bezierCurvePoints.Count - 4]) / (3f * Time.fixedDeltaTime)) > 40f)
                {
                    ballRig.velocity = (bezierCurvePoints[bezierCurvePoints.Count - 1] - bezierCurvePoints[bezierCurvePoints.Count - 4]) / (3f * Time.fixedDeltaTime);
                    ballRig.velocity = ballRig.velocity / Vector3.Dot(ballRig.velocity, ballRig.velocity) * 40f;
                }
                else
                {
                    ballRig.velocity = (bezierCurvePoints[bezierCurvePoints.Count - 1] - bezierCurvePoints[bezierCurvePoints.Count - 4]) / (3f * Time.fixedDeltaTime);
                }

            }

            if (whoHit == 0) // Player
            {
                ballRig.velocity = (bezierCurvePoints[bezierCurvePoints.Count - 1] - bezierCurvePoints[bezierCurvePoints.Count - 4]) / (3f * Time.fixedDeltaTime);
            }

            pointNum = 0;
            bezierCurvePoints.Clear();

        }

    }


    private void Serve(bool isBack)
    {
        serveSoundCount = 0;
        serveSoundBool = true;

        Vector3 startingPoint = transform.position;

        Vector3 endPoint;

        //Debug.Log(Vector3.Dot(VM.velocity, VM.velocity));

        float back = 1f;

        if (isBack)
        {
            back = -1f;
        }

        endPoint.z = startingPoint.z + ((back * racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.25f + 1.8f)).z) * 0.4f;
        endPoint.x = startingPoint.x + ((back * racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.25f + 1.8f)).x) * 0.4f;

        endPoint.y = 0.8550002f;

        Vector3 midPoint = (startingPoint + endPoint) / 2f;
        midPoint.y += 0.2f;


        bezierCurvePoints.Clear();

        while (k <= 1f)
        {
            Vector3 tempPoint;
            tempPoint = (1f - k) * (1f - k) * startingPoint + 2f * k * (1f - k) * midPoint + k * k * endPoint;
            bezierCurvePoints.Add(tempPoint);
            k += bezierStep + 0.017f;
            //Instantiate(testBall, tempPoint, Quaternion.identity);
            serveSoundCount++;
        }

        k = 0f;

        Vector3 startingPoint2 = endPoint;
        Vector3 midPoint2 = midPointObject.transform.position;
        Vector3 endPoint2 = reflection.getRandomPoint();

        endPoint2.z = startingPoint.z + ((back * racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.25f + 1.8f)).z);
        endPoint2.x = startingPoint.x + ((back * racket.normal * (Vector3.Dot(VM.velocity, VM.velocity) * 0.25f + 1.8f)).x);

        midPoint2.z = (startingPoint2.z + endPoint2.z) / 2f;
        midPoint2.x = (startingPoint2.x + endPoint2.x) / 2f;
        endPoint2.y -= 0.1f;

        while (k <= 1f)
        {
            Vector3 tempPoint;
            tempPoint = (1f - k) * (1f - k) * startingPoint2 + 2f * k * (1f - k) * midPoint2 + k * k * endPoint2;
            bezierCurvePoints.Add(tempPoint);
            k += bezierStep + 0.005f;
            //Instantiate(testBall, tempPoint, Quaternion.identity);
        }
        k = 0f;

    }

    private void soundPlay(AudioClip clip)
    {
        pingSound.clip = clip;
        pingSound.Play();
    }

    private void isCheating()
    {
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            cheating = true;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            cheating = false;
        }
    }
}
