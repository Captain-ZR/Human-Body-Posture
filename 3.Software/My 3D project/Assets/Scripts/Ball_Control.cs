using UnityEngine;

public class Ball_Control : MonoBehaviour
{
    Rigidbody ball;
    GameObject target;
    GameObject[] figures = new GameObject[12];

    GameObject figure1;
    GameObject figure2;
    GameObject figure3;
    GameObject figure4;
    GameObject figure5;
    GameObject figure6;
    GameObject figure7;
    GameObject figure8;
    GameObject figure9;
    GameObject figure10;
    GameObject figure11;

    float h = 3;
    float gravity = -9.8f;
    float previousTime, currentTime;
    public int intervalTime = 3;

    int randouNumber;       // 对象随机数

    // Start is called before the first frame update
    void Start()
    {
        figure1 = GameObject.Find("mixamorig:RightForeArm");
        figure2 = GameObject.Find("mixamorig:RightArm");
        figure3 = GameObject.Find("mixamorig:LeftForeArm");
        figure4 = GameObject.Find("mixamorig:LeftArm");
        figure5 = GameObject.Find("mixamorig:Head");
        figure6 = GameObject.Find("mixamorig:Spine1");
        figure7 = GameObject.Find("mixamorig:RightLeg");
        figure8 = GameObject.Find("mixamorig:RightUpLeg");
        figure9 = GameObject.Find("mixamorig:LeftLeg");
        figure10 = GameObject.Find("mixamorig:LeftUpLeg");
        figure11 = GameObject.Find("mixamorig:Hips");

        figures[1] = figure1;
        figures[2] = figure2;
        figures[3] = figure3;
        figures[4] = figure4;
        figures[5] = figure5;
        figures[6] = figure6;
        figures[7] = figure7;
        figures[8] = figure8;
        figures[9] = figure9;
        figures[10] = figure10;
        figures[11] = figure11;

        ball = this.GetComponent<Rigidbody>();
        //target = GameObject.Find("mixamorig:LeftFoot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Launch()
    {
        ball.velocity = GetBallVelocity();
    }

    Vector3 GetBallVelocity()
    {
        randouNumber = Random.Range(1, 11);
        target = figures[randouNumber];
        print(randouNumber);

        float displacementY = target.transform.position.y - ball.position.y;
        Vector3 displacementXZ = new Vector3(target.transform.position.x - ball.position.x, 0, target.transform.position.z - ball.position.z);
        
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 vectorcityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));

        return vectorcityXZ + velocityY;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "ybot")
        {
            //Debug.Log("peng");
            previousTime = Time.time;            
        }
        if(collision.gameObject.name == "Ground")
        {
            currentTime = Time.time;
            if(currentTime - previousTime > intervalTime)
            {
                Launch();
            }
        }
    }
}
