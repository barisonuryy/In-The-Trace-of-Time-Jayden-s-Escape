using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class DragonAgentScript : Agent
{
    private Rigidbody rgb;
    [SerializeField] Transform target;
    [SerializeField] Transform originalPosition;
    [SerializeField] Transform lookAt;
    public float carpan = 5f;
    private float distance;
    private Animator _animator;
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    public override void OnEpisodeBegin()
    {
        if (transform.localPosition.y < 0f)
        {
            //ajanýn platformun sýnýrlarýndan düþüp düþmediðinin kontrolü yapýldý.
            rgb.angularVelocity = Vector3.zero;
            rgb.velocity = Vector3.zero;
            //Ajan düþtükten sonra yeniden platformun üstüne yerleþir.
            transform.localPosition = new Vector3(0, 1f, 0);
            target.transform.localPosition = new Vector3(-50f,1f,-13f);
        }
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Bazý önemli verilerin görüntülenmesi ve kaydedilmesi saðlanýr.

        //Ajan ve target'ýn pozisyon bilgileri.Sürekli deðiþen bilgiler olduðu için tutulmalýdýr.
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);
        //Ajanýn hýz bilgileri -> x ve z eksenlerinde hareket saðlanýr.
        sensor.AddObservation(rgb.velocity.x);
        sensor.AddObservation(rgb.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        distance = Vector3.Distance(transform.localPosition, target.localPosition);
        Vector3 controlSignal = Vector3.zero;
        _animator.SetBool("isAttack", false);
        if (transform.localPosition.y < 0f)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
        else
        {
            if (distance < 20)
            {
                _animator.SetBool("onLocation", false);
                transform.LookAt(target.transform);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.localPosition, Time.deltaTime * 3);
                controlSignal.x = actions.ContinuousActions[0];
                controlSignal.z = actions.ContinuousActions[1];
                rgb.AddForce(controlSignal*carpan);
                //Ajanýn hedefle arasýndaki mesafe farkýna göre ödüllendirme ve cezalandýrma
                if (distance < 3f)
                {
                    Debug.Log("Inside If");
                    _animator.SetBool("isAttack",true);
                    SetReward(1.0f);
                    EndEpisode();
                }
            }
            else
            {
                //_animator.SetBool("onLocation", false);
                transform.LookAt(lookAt.transform);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition.localPosition, Time.deltaTime * 3);
                _animator.SetBool("onLocation", true);
                controlSignal = Vector3.zero;
                distance = Vector3.Distance(transform.localPosition, target.localPosition);
                SetReward(-1.0f);
                EndEpisode();
            }
            EndEpisode();
        }
        Debug.Log(distance);
        
    }


}
