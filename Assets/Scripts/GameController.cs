using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject m_food;
    [SerializeField]
    GameObject m_superFood;

    [SerializeField]
    float m_roundTime = 60;

    [SerializeField]
    bool m_roundInProgress;

    public List<GameObject> m_foodList = new List<GameObject>();
    void Start()
    {
        StartCoroutine(SpawnFood());
    }

    IEnumerator SpawnFood()
    {

        while (m_roundInProgress)
        {
            if (m_foodList.Count < 50)
            {
                int randomChance = Random.Range(1, 10);


                //30% chance to spawn super trait food
                if (randomChance > 7)
                {
                     m_foodList.Add(Instantiate(m_superFood, new Vector3(Random.Range(-80, 80), Random.Range(-80, 80), 0), Quaternion.identity));
                }
                //70% chance to spawn normal food
                else
                {
                    m_foodList.Add(Instantiate(m_food, new Vector3(Random.Range(-80, 80), Random.Range(-80, 80), 0), Quaternion.identity));
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (m_roundInProgress)
        {
            if (m_roundTime > 0)
            {
                m_roundTime -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time is up");
                m_roundTime = 0;
                m_roundInProgress= false;
            }
        }
    }
}
