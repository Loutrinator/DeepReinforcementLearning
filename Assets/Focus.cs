using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Focus : MonoBehaviour
{
    [SerializeField] private float focusSpeed = 5f;
    [SerializeField] private Transform target;
    private Volume postProcessVolume;
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Volume volume; 
    private DepthOfField DOF;

    private float focus;

    void Awake ()
    {
        postProcessVolume = camera.gameObject.GetComponent<Volume>();
        volume.profile.TryGet(out DOF); //TryGetSettings(out DOF);
    }

    void Start()
    {
        focus = (target.position - camera.transform.position).magnitude;
    }
    void Update ()
    {
        focus = Mathf.Lerp(focus, (target.position - camera.transform.position).magnitude, focusSpeed * Time.deltaTime);
        DOF.focusDistance.value = focus;
    }
}
