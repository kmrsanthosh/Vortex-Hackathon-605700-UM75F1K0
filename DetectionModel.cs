using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.DnnModule;

public class DetectionModel : MonoBehaviour
{
    public string detectionModelPath;
    public float confidenceThreshold;
    public float nmsThreshold;

    private Net detectionNet;
    private Mat inputMat;
    private Mat detectionMat;

    private void Start()
    {

        detectionNet = Dnn.readNetFromTensorflow(detectionModelPath);

        inputMat = new Mat();
        detectionMat = new Mat();
    }

    public void Detect(WebCamTexture webCamTexture)
    {

        Utils.webCamTextureToMat(webCamTexture, inputMat);

        Mat blob = Dnn.blobFromImage(inputMat, 1.0, new Size(300, 300), new Scalar(127.5, 127.5, 127.5), true, false);
        detectionNet.setInput(blob);

        detectionNet.forward(detectionMat);

        float maxConfidence = 0;
        Rect maxRect = new Rect();
        for (int i = 0; i < detectionMat.total(); i += 7)
        {
            float confidence = (float)detectionMat.get(0, 0, i, 2)[0];
            if (confidence > confidenceThreshold)
            {
                int classId = (int)detectionMat.get(0, 0, i, 1)[0];
                int left = (int)(detectionMat.get(0, 0, i, 3)[0] * inputMat.cols());
                int top = (int)(detectionMat.get(0, 0, i, 4)[0] * inputMat.rows());
                int right = (int)(detectionMat.get(0, 0, i, 5)[0] * inputMat.cols());
                int bottom = (int)(detectionMat.get(0, 0, i, 6)[0] * inputMat.rows());
                Rect rect = new Rect(left, top, right - left, bottom - top);
                float overlap = 0;
                for (int j = 0; j < detectionMat.total(); j += 7)
                {
                    if (i != j && (int)detectionMat.get(0, 0, j, 1)[0] == classId)
                    {
                        int otherLeft = (int)(detectionMat.get(0, 0, j,                3)[0] * inputMat.cols());
                    int otherTop = (int)(detectionMat.get(0, 0, j, 4)[0] * inputMat.rows());
                    int otherRight = (int)(detectionMat.get(0, 0, j, 5)[0] * inputMat.cols());
                    int otherBottom = (int)(detectionMat.get(0, 0, j, 6)[0] * inputMat.rows());
                    Rect otherRect = new Rect(otherLeft, otherTop, otherRight - otherLeft, otherBottom - otherTop);
                    float intersectionArea = Mathf.Max(0, Mathf.Min(rect.xMax, otherRect.xMax) - Mathf.Max(rect.xMin, otherRect.xMin)) * Mathf.Max(0, Mathf.Min(rect.yMax, otherRect.yMax) - Mathf.Max(rect.yMin, otherRect.yMin));
                    float unionArea = rect.width * rect.height + otherRect.width * otherRect.height - intersectionArea;
                    float iou = intersectionArea / unionArea;
                    if (iou > overlap)
                    {
                        overlap = iou;
                    }
                }
            }
            if (overlap < nmsThreshold && confidence > maxConfidence)
            {
                maxConfidence = confidence;
                maxRect = rect;
            }
        }
    }


    Imgproc.rectangle(inputMat, maxRect, new Scalar(0, 255, 0), 2);


    measurementController.SetObjectSize(maxRect.width, maxRect.height);
}

public void StopDetection()
{

    inputMat.release();
    detectionMat.release();

    detectionNet.setPreferableBackend(0);
    detectionNet.setPreferableTarget(0);
    detectionNet.dispose();
}
}

