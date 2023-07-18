using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    private Vector3 _originalPos;
    public static CameraShake _instance;

    void Awake()
    {
        Assign();
    }

    private void Update()
    {
        if (_instance == null) Assign();
    }

    public void Assign()
    {
        _originalPos = transform.localPosition;

        _instance = this;
    }

    public static void Shake (float duration, float amount, bool stopCurrentShake = true, bool twod = false) {
        if (twod) _instance.StartCoroutine(_instance.cShaketwod(duration, amount));
        else
        {
            if(stopCurrentShake) _instance.StopAllCoroutines();
            _instance.StartCoroutine(_instance.cShake(duration, amount));
        }
    }

    public IEnumerator cShake (float duration, float amount) {
        float endTime = Time.time + duration;

        while (Time.time < endTime) {
            transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

            duration -= Time.deltaTime;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }

    public IEnumerator cShaketwod(float duration, float amount)
    {
        _originalPos = transform.position;
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            transform.localPosition = new Vector3(_originalPos.x + Random.insideUnitSphere.x * amount, _originalPos.y + Random.insideUnitSphere.y * amount, _originalPos.z);

            duration -= Time.deltaTime;

            yield return null;
        }

        transform.position = _originalPos;
    }
}