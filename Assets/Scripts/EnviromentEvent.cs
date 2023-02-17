using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentEvent : MonoBehaviour
{

    [SerializeField]
    bool _ExperimentReady = false;
    [SerializeField]
    float _ExperimentLength = 30f;
    [SerializeField]
    float _ExperimentCooldown = 30f;
    [SerializeField]
    bool _ExperimentEnded = false;

    IEnumerator StartExperiment()
    {
        int experimentType = Random.Range(1, 4);
        _ExperimentReady = false;
        _ExperimentEnded = false;
        //Add stuff to map during experiment
        switch(experimentType)
            {
                case 1: //Damaging Hazard
                    break;
                case 2: //Movement Hazard
                    break;
                case 3: //Blocking Hazard
                    break;
                default:
                    break;
            }
        yield return new WaitForSeconds(_ExperimentLength);
        _ExperimentEnded = true;
        //Clean up map after experiement
        switch(experimentType)
            {
                case 1: //Damaging Hazard
                    break;
                case 2: //Movement Hazard
                    break;
                case 3: //Blocking Hazard
                    break;
                default:
                    break;
            }
        yield return new WaitForSeconds(_ExperimentCooldown);
        _ExperimentReady = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
