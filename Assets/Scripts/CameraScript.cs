using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraScript : MonoBehaviour
{
    Vector3 m_trackingPosition;

    [SerializeField]
    int m_speed = 10;
    // Update is called once per frame

    float m_fieldOfView;

    [SerializeField]
    GameObject m_player;

    PostProcessVolume m_postProcessVolume;
    DepthOfField m_depthOfField;
    Vignette m_vignette;
    Camera m_camera;

    [Range(1, 3)] int zoomLevel = 2;

    [SerializeField]
    private int m_switchSpeed= 1;

    Vector2 m_leftPoint;
    Vector2 m_rightPoint;
    Vector2 m_originalPoint;

    Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.5f;
    private void Start()
    {

        m_leftPoint = new Vector2(-1.5f, 0.5f);
        m_rightPoint = new Vector2(1.5f, 0.5f);
        m_originalPoint = new Vector2(0.5f, 0.5f);

        m_camera = gameObject.GetComponent<Camera>();
        m_postProcessVolume = m_camera.GetComponent<PostProcessVolume>();
        m_postProcessVolume.profile.TryGetSettings(out m_depthOfField);
        m_postProcessVolume.profile.TryGetSettings(out m_vignette);
        m_fieldOfView = m_camera.orthographicSize;
    }

    void Update()
    {
        m_trackingPosition = new Vector3(m_player.transform.position.x, m_player.transform.position.y, -10f);

        transform.position = Vector3.SmoothDamp(transform.position, m_trackingPosition, ref velocity, smoothTime);

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (zoomLevel != 3)
            {
                zoomLevel += 1;
            }
            else
            {
                zoomLevel = 1;
            }
            StopAllCoroutines();
            StartCoroutine(MicroScopeSwitchExitLeft(zoomLevel));
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (zoomLevel != 1)
            {
                zoomLevel -= 1;
            }
            else
            {
                zoomLevel = 3;
            }
            StopAllCoroutines();
            StartCoroutine(MicroScopeSwitchExitRight(zoomLevel));

        }
    }

    // 10, 20, 30
    // .4, .7, 1.0
    // Use those sets for intensisty
    // Intensity is inversaly proportional to size of camera
    IEnumerator MicroScopeSwitchExitLeft(int zoomLevel)
    {
        while (m_vignette.center.value.x > m_leftPoint.x + 0.2f)
        {
            m_vignette.center.value = Vector2.Lerp(m_vignette.center.value, m_leftPoint, m_switchSpeed * Time.deltaTime);
            yield return null;


            if (m_vignette.center.value.x < m_leftPoint.x + 0.2f)
            {
                switch (zoomLevel)
                {
                    case 1:
                        m_fieldOfView = 10;
                        m_vignette.intensity.value = 0.4f;
                        m_camera.orthographicSize = m_fieldOfView;
                        break;

                    case 2:
                        m_fieldOfView = 20;
                        m_vignette.intensity.value = 0.7f;
                        m_camera.orthographicSize = m_fieldOfView;
                        break;

                    case 3:
                        m_fieldOfView = 30;
                        m_vignette.intensity.value = 1f;
                        m_camera.orthographicSize = m_fieldOfView;
                        break;

                    default:
                        break;
                }
                m_vignette.center.value = m_rightPoint;
                m_depthOfField.focalLength.value = 30;
                StartCoroutine(MicroScopeSwitchEnter());
                yield break;
            }
        }
    }

    IEnumerator MicroScopeSwitchExitRight(int zoomLevel)
    {
        while (m_vignette.center.value.x < m_rightPoint.x - 0.2f)
        {
            m_vignette.center.value = Vector2.Lerp(m_vignette.center.value, m_rightPoint, m_switchSpeed * Time.deltaTime);
            yield return null;


            if (m_vignette.center.value.x > m_rightPoint.x - 0.2f)
            {
                switch (zoomLevel)
                {
                    case 1:
                        m_fieldOfView = 10;
                        m_vignette.intensity.value = 0.4f;
                        m_camera.orthographicSize = m_fieldOfView;
                        break;

                    case 2:
                        m_fieldOfView = 20;
                        m_vignette.intensity.value = 0.7f;
                        m_camera.orthographicSize = m_fieldOfView;
                        break;

                    case 3:
                        m_fieldOfView = 30;
                        m_vignette.intensity.value = 1f;
                        m_camera.orthographicSize = m_fieldOfView;
                        break;

                    default:
                        break;
                }
                m_vignette.center.value = m_leftPoint;
                m_depthOfField.focalLength.value = 30;
                StartCoroutine(MicroScopeSwitchEnter());
                yield break;
            }
        }
    }


    IEnumerator MicroScopeSwitchEnter()
    {

        while (m_vignette.center.value.x != m_originalPoint.x)
        {
            m_vignette.center.value = Vector2.Lerp(m_vignette.center.value, m_originalPoint, m_switchSpeed * Time.deltaTime);
            yield return null;

            if (m_vignette.center.value == m_originalPoint)
            {
                StartCoroutine(UnBlurScreen());
                yield break;
            }
        }



    }

    IEnumerator UnBlurScreen()
    {
        while (m_depthOfField.focalLength != 20)
        {
            m_depthOfField.focalLength.value -= 0.2f;
            yield return null;

            if (m_depthOfField.focalLength <= 20)
            {
                m_depthOfField.focalLength.value = 20;
                yield break;
            }
        }    

    }
    //void BlurCamera()
    //{
    //    if (m_depthOfField.focalLength < 30)
    //    {
    //        m_depthOfField.focalLength.value += 1f;
    //    }
    //}

    //void UnBlurCamera()
    //{
    //    if (m_depthOfField.focalLength > 20)
    //    {
    //        m_depthOfField.focalLength.value -= 1f;
    //    }
    //}
}
