using UnityEngine;

public class FoodScript : MonoBehaviour
{
    public int m_foodAmount;
    
    void Start()
    {
        m_foodAmount = Random.Range(1,10);
        transform.localScale = new Vector3( m_foodAmount, m_foodAmount);
    }
    
    public void UpdateFoodAmount()
    {
        transform.localScale = new Vector3(m_foodAmount, m_foodAmount);
        if (m_foodAmount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int GetFood(bool t_isOffSpring)
    {
        int chompOfFood;

        if (t_isOffSpring)
        {
            chompOfFood = 1;
        }
        else
        {
            chompOfFood = Random.Range(3, 5);

            if (chompOfFood > m_foodAmount)
            {
                chompOfFood = m_foodAmount;
            }
        }

        m_foodAmount -= chompOfFood;

        UpdateFoodAmount();

        return chompOfFood;
    }
}
