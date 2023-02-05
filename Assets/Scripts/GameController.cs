using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject m_food;
    [SerializeField]
    GameObject m_superFood;

    [SerializeField]
    bool m_roundInProgress;

    [SerializeField]
    int spawn;

    [SerializeField]
    GameObject _clipboard;

    public TextMeshProUGUI _text;

    public List<GameObject> _singleCellOrganisms= new List<GameObject>();
    void Start()
    {

        _singleCellOrganisms = GameObject.FindGameObjectsWithTag("SingleCelledOrganism").ToList();
        StartCoroutine(SpawnFood());
    }

    IEnumerator SpawnFood()
    {

        while (m_roundInProgress)
        {

            int randomChance = Random.Range(1, 10);


            //30% chance to spawn super trait food
            if (randomChance > 7)
            {
                GameObject x = Instantiate(m_superFood, new Vector3(Random.Range(-spawn, spawn), Random.Range(-spawn, spawn), 0), Quaternion.identity);
                x.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                
            }
            //70% chance to spawn normal food
            else
            {
                GameObject x = Instantiate(m_food, new Vector3(Random.Range(-spawn, spawn), Random.Range(-spawn, spawn), 0), Quaternion.identity);
                x.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            }

            yield return new WaitForSeconds(0.2f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!_singleCellOrganisms.Any((x) => x != null))
        {
            _text.text = "Experiment Completed";
            _text.color = Color.green;
            m_roundInProgress = false;
            _clipboard.SetActive(true);
        }

    }

    public void GameOver()
    {
        _text.text = "Specimen Terminated";
        _text.color = Color.red;
        m_roundInProgress = false;
        _clipboard.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
