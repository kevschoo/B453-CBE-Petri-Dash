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
    //[SerializeField]
    //bool _ExperimentEnded = false;

    [SerializeField] int _HazardAmount = 50;
    [SerializeField] float _minX = -10f;
    [SerializeField] float _maxX = 10f;
    [SerializeField] float _minY = -10f;
    [SerializeField] float _maxY = 10f;

    [SerializeField] GameObject DamageHazard;
    [SerializeField] GameObject MovementHazard;
    [SerializeField] GameObject BlockHazard;

    IEnumerator StartExperiment()
    {
        int experimentType = Random.Range(1, 9);
        _ExperimentReady = false;
        //_ExperimentEnded = false;
        //Add stuff to map during experiment
        switch(experimentType)
            {
                case 1: //Damaging Hazard
                    GenerateHazard(DamageHazard);
                    break;
                case 2: //Movement Hazard
                    GenerateHazard(MovementHazard);
                    break;
                case 3: //Blocking Hazard
                    GenerateHazard(BlockHazard);
                    break;
                case 4: //Damaging Hazard
                    GenerateHazard(DamageHazard);
                    break;
                case 5: 
                    GenerateHazard(MovementHazard);
                    GenerateHazard(DamageHazard);
                    break;
                case 6:
                    GenerateHazard(BlockHazard);
                    GenerateHazard(DamageHazard);
                    break;
                case 7: 
                    GenerateHazard(MovementHazard);
                    GenerateHazard(BlockHazard);
                    break;
                case 8: 
                    GenerateHazard(BlockHazard);
                    GenerateHazard(DamageHazard);
                    GenerateHazard(MovementHazard);
                    break;
                default:
                    Debug.Log("Error on Hazard Gen");
                    break;
            }
        yield return new WaitForSeconds(_ExperimentLength);
        //_ExperimentEnded = true;
        //Clean up map after experiement
        /*
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
            */
        yield return new WaitForSeconds(_ExperimentCooldown);
        _ExperimentReady = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_ExperimentReady)
        {
            StartCoroutine(StartExperiment());
        }
    }

    void GenerateHazard(GameObject Hazard)
    {
        for(int i = 0; i < _HazardAmount; i++)
        {
            float x = Random.Range(_minX, _maxX);
            float y = Random.Range(_minY, _maxY);
            Vector3 position = new Vector3(x, y, 0f);
            Instantiate(Hazard, position, Quaternion.identity);
        }
    }

}
