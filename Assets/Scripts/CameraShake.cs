using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // How long the object should shake for.
    [SerializeField] float shakeDuration = 1f;
    float shakeDurationCalc;

    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField] float shakeAmount = 0.7f;
    //public float decreaseFactor = 1.0f;

    [SerializeField] AnimationCurve curve;

    public static bool shaketrue= false;

    Vector3 originalPos;

    void Awake()
    {

    }

    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shaketrue)
        {
            if (shakeDurationCalc > 0) {
                transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * curve.Evaluate(1 - shakeDuration);

                // shakeDuration -= Time.deltaTime * decreaseFactor;
                shakeDurationCalc -= Time.deltaTime;
                
            } else {
                shakeDurationCalc = shakeDuration;
                transform.localPosition = originalPos;
                shaketrue = false;
            }
        }
    }

    public void shakecamera()
    {
        shakeDurationCalc = shakeDuration;
        transform.localPosition = originalPos;
        shaketrue = true;
    }
}