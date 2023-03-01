using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScEnemyController : MonoBehaviour
{
    //GUI Elements
    [SerializeField] private GameObject HealthBarObj;
    private RectTransform HealthBarTransform;

    [SerializeField] private GameObject PlayerGO;
    [SerializeField] private GameObject BrainGO;
    private Transform PlayerT;
    private Vector2 HomePos;
    private Vector2 MovementDir;
    UnityEngine.AI.NavMeshAgent agent;

    private float TargetDist;
    [SerializeField] private float followRange = 8f;

    private Animator enemyZombieAnim;

    [SerializeField] private float enemyHurtDamage = 40f;

    [SerializeField] private float max_health = 100f;
	[SerializeField] private float min_health = 0f;
	[SerializeField] private float curr_health = 60f;

    void Start()
    {
        PlayerT = PlayerGO.transform;
        HomePos = new Vector2(this.transform.position.x, this.transform.position.y);

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        enemyZombieAnim = this.GetComponent<Animator>();

        HealthBarTransform = HealthBarObj.GetComponent<RectTransform>();

    }

    void Update()
    {
        TargetDist = Vector2.Distance(PlayerT.transform.position, this.transform.position);
        if(TargetDist <= followRange)
        {
            agent.SetDestination(PlayerT.position);
        }
        else
        {
            agent.SetDestination(HomePos);
        }
        MovementDir = agent.velocity / agent.speed;

        float magn = Mathf.Sqrt(MovementDir.x * MovementDir.x + MovementDir.y * MovementDir.y);
        if(magn > 0.015f)
        {
            enemyZombieAnim.SetBool("BoolIs_Idle", false);
            enemyZombieAnim.SetFloat("MovX", MovementDir.x);
            enemyZombieAnim.SetFloat("MovY", MovementDir.y);
        }

        float currentHealthPercentage = curr_health / 100;
        float healthBarWidth = currentHealthPercentage * 30; 
        HealthBarTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(healthBarWidth, HealthBarTransform.GetComponent<RectTransform>().sizeDelta.y);
        Debug.Log("enemy helathbr" + currentHealthPercentage);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            float damage = col.gameObject.GetComponent<ScHandGunBullet>().getBulletDamage();
            this.setHealth(this.getCurrHealth() - damage);
            Debug.Log("Enemy Health" + this.getCurrHealth());

            Destroy(col.gameObject);

            if(this.getCurrHealth() <= 0f)
            {
                float chance = Random.Range(0.0f, 100.0f);
                if(chance > 50f)
                    Instantiate(BrainGO, this.transform.position, Quaternion.identity);
                
                    
                //GameObject brain = Instantiate(BrainGO, transform.position, Quaternion.identity);
                PlayerGO.GetComponent<ScPlayerDM>().IncrasePlayerScore();

                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Destroy(col.gameObject);
            //Destroy(this.gameObject);

        }
    }

    public float getEnemyHurtDamage(){return enemyHurtDamage;}

    public float getMinHealth(){ return this.min_health; }
	public float getCurrHealth(){ return this.curr_health; }

	public void setHealth(float HealthAmount)
	{
		if(HealthAmount > this.max_health)
		{
			this.curr_health = this.max_health;
		}else
		{
			this.curr_health = HealthAmount;
		}
	}
}
