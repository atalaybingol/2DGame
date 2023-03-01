using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ScPlayerDM : MonoBehaviour
{
    private Sc_PlayerData Player1Data;

    //GUI Elements
    [SerializeField] private GameObject HealthBarObj;
    [SerializeField] private GameObject HealthBarTextObj;
    [SerializeField] private GameObject BrainCountObj;
    [SerializeField] private GameObject ScoreCountObj;

    private RectTransform HealthBarTransform;
    private TextMeshProUGUI HealthBarText;
    private TextMeshProUGUI BrainCountText;
    private TextMeshProUGUI ScoreCountText;

    public void IncrasePlayerScore(){ Player1Data.increaseScore();}
    public float getPlayerHealth() { return Player1Data.getCurrHealth(); }
    public float getPlayerMaxHealth() { return Player1Data.getMaxHealth(); }

    // Start is called before the first frame update
    void Start()
    {
        Player1Data = new Sc_PlayerData();

        HealthBarTransform = HealthBarObj.GetComponent<RectTransform>();
        HealthBarText = HealthBarTextObj.GetComponent<TextMeshProUGUI>();
        BrainCountText = BrainCountObj.GetComponent<TextMeshProUGUI>();
        ScoreCountText = ScoreCountObj.GetComponent<TextMeshProUGUI>();

        HealthBarText.text = Player1Data.getCurrHealth().ToString();
        BrainCountText.text = Player1Data.getBrainCount().ToString();
        ScoreCountText.text = Player1Data.getScore().ToString();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Player1Data.setHealth(Player1Data.getCurrHealth() + 10f);

            Debug.Log("Player Health" + Player1Data.getCurrHealth());
        }   
        
        if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Player1Data.setHealth(Player1Data.getCurrHealth() - 10f);

            Debug.Log("Player Health" + Player1Data.getCurrHealth());
        }
        
        if(Player1Data.getCurrHealth() <= Player1Data.getMinHealth())
        {
            this.gameObject.SetActive(false);

            Time.timeScale = 0f;
            //Destroy(this.gameObject);
        }

        float currentHealthPercentage = Player1Data.getCurrHealth() / 100;
        float healthBarWidth = currentHealthPercentage * 300; 
        HealthBarTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(healthBarWidth, HealthBarTransform.GetComponent<RectTransform>().sizeDelta.y);

        HealthBarText.text = Player1Data.getCurrHealth().ToString();
        BrainCountText.text = Player1Data.getBrainCount().ToString();
        ScoreCountText.text = Player1Data.getScore().ToString();
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
                //Destroy(this.gameObject);

                ScEnemyController enemyContr;
                enemyContr = col.gameObject.GetComponent<ScEnemyController>();
                
                //Player1Data.setHealth(Player1Data.getCurrHealth() - enemyContr.getEnemyHurtDamage());
                this.hurt(enemyContr.getEnemyHurtDamage());
                
        }
    }

    public void hurt(float amount)
    {
        Player1Data.setHealth(Player1Data.getCurrHealth() - amount);
        Debug.Log("Player Health" + Player1Data.getCurrHealth());

    }

    void OnTriggerEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Brain")
        {
            Player1Data.increaseBrainCount();
            Destroy(col.gameObject);
        }
    }
}
