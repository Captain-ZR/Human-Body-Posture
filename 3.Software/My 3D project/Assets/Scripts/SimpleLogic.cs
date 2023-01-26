using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLogic : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 3;
    public GameObject target;

    void Start()
    {
        Application.targetFrameRate = 60;

        //Debug.Log("���Կ�ʼ");
        
        //GameObject obj = this.gameObject;

        //string name = obj.name;
        //Debug.Log("��������: " + name);

        //Transform tr = this.gameObject.transform;
        //Vector3 pos = this.transform.position;
        //Debug.Log("λ��" + pos.ToString("f3"));

        //GameObject target = GameObject.Find("target");
        //GameObject car = GameObject.Find("car");

        //this.transform.LookAt(target.transform);

    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.Translate(-0.1f, 0, 0, Space.Self);  //����������ϵ�ƶ�
        //float distance = speed * Time.deltaTime;
        //this.transform.Translate(0, 0, distance, Space.Self);
        if (Input.GetMouseButtonDown(0))
        {
            PlayMusic();
        }
    }

    void PlayMusic()
    {
        AudioSource audio = this.GetComponent<AudioSource>();
        if(audio.isPlaying)
        {
            audio.Stop();
        }
        else
        {
            audio.Play();
        }
    }
}
 