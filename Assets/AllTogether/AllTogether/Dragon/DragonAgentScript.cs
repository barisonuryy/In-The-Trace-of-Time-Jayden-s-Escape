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
            //ajan�n platformun s�n�rlar�ndan d���p d��medi�inin kontrol� yap�ld�.
            rgb.angularVelocity = Vector3.zero;
            rgb.velocity = Vector3.zero;
            //Ajan d��t�kten sonra yeniden platformun �st�ne yerle�ir.
            transform.localPosition = new Vector3(0, 1f, 0);
            target.transform.localPosition = new Vector3(-50f,1f,-13f);
        }
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Baz� �nemli verilerin g�r�nt�lenmesi ve kaydedilmesi sa�lan�r.

        //Ajan ve target'�n pozisyon bilgileri.S�rekli de�i�en bilgiler oldu�u i�in tutulmal�d�r.
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);
        //Ajan�n h�z bilgileri -> x ve z eksenlerinde hareket sa�lan�r.
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
                //Ajan�n hedefle aras�ndaki mesafe fark�na g�re �d�llendirme ve cezaland�rma
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
