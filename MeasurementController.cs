using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MeasurementController : MonoBehaviour
{
    public Text sizeText;

    private float objectWidth;
    private float objectHeight;
    private float objectSize;

    private void Start()
    {
        objectWidth = 0;
        objectHeight = 0;
        objectSize = 0;
    }

    public void StartMeasurement()
    {
        StartCoroutine(MeasurementCoroutine());
    }

    public void StopMeasurement()
    {
        StopAllCoroutines();
    }

    private IEnumerator MeasurementCoroutine()
    {

        while (true)
        {
            objectSize = objectWidth * objectHeight;
            sizeText.text = string.Format("Object Size: {0} square pixels", objectSize);
            yield return null;
        }
    }

    public void SetObjectSize(float width, float height)
    {
        objectWidth = width;
        objectHeight = height;
    }
}
