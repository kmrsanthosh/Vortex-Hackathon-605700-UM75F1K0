using System.Collections;
using UnityEngine;

public class DetectionController : MonoBehaviour
{
    public WebCamTexture webCamTexture;
    public DetectionModel detectionModel;
    public MeasurementController measurementController;

    private void Start()
    {

        webCamTexture.Play();

        StartCoroutine(DetectionCoroutine());
        measurementController.StartMeasurement();
    }

    private IEnumerator DetectionCoroutine()
    {
        while (true)
        {
            detectionModel.Detect(webCamTexture);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        webCamTexture.Stop();

        detectionModel.StopDetection();
        measurementController.StopMeasurement();
    }
}
