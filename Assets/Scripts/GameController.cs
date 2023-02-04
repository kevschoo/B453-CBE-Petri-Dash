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

    public List<GameObject> m_foodList = new List<GameObject>();
    void Start()
    {
        StartCoroutine(SpawnFood());
    }

    IEnumerator SpawnFood()
    {

        while (m_foodList.Count < 10)
        {
            int randomChance = Random.Range(1, 10);

            if (randomChance > 7)
            {
                 m_foodList.Add(Instantiate(m_superFood, new Vector3(Random.Range(-40, 40), Random.Range(-40, 40), 0), Quaternion.identity));
            }
            else
            {
                m_foodList.Add(Instantiate(m_food, new Vector3(Random.Range(-40, 40), Random.Range(-40, 40), 0), Quaternion.identity));
            }

            yield return new WaitForSeconds(1f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
