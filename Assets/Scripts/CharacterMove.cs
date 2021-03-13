using System;
using System.Collections;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float speedForward;
    public float speedX;

    bool canGo, screenClick, enemyInRange;

    public bool isStarter;

    Animator anim;

    public Material whiteMaterial, clothesMaterial;
    public SkinnedMeshRenderer skinned;

    GameObject enemy;

    [SerializeField]
    float health;

    [SerializeField]
    float attackCoolDownTime, attack;

    float enemyAttackCoolDown, attackCoolDown;

    void Start()
    {
        enemyAttackCoolDown = 0;
        attackCoolDown = 0;
        anim = GetComponent<Animator>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        canGo = false;
        enemyInRange = false;
        if (!isStarter)
            skinned.sharedMaterial = whiteMaterial;
        else
            canGo = true;
        screenClick = false;
    }

    void Update()
    {
        if (!GameControll.Instance.Ended)
        {
            if (canGo)
                Move();
        }
        else
            FightWithEnemy();
    }

    private void FightWithEnemy()
    {
        anim.SetBool("Run", false);
        Vector3 locVel = Vector3.zero;
        if (enemyInRange)
            DealAndGetDamage();
        else if (enemy.GetComponent<EnemyHealth>().playerCloseEnemy < 5)
        {
            anim.SetBool("Run", true);
            locVel = (enemy.transform.position - transform.position).normalized * speedForward * Time.deltaTime;
        }
        transform.position += locVel;
        enemyAttackCoolDown += Time.deltaTime;
        attackCoolDown += Time.deltaTime;
    }

    private void DealAndGetDamage()
    {
        if (attackCoolDown > attackCoolDownTime)
        {
            attackCoolDown = 0;
            enemy.GetComponent<EnemyHealth>().enemyHealth -= attack;
            UIController.Instance.SetSliderValue(enemy.GetComponent<EnemyHealth>().enemyStartingHealth, enemy.GetComponent<EnemyHealth>().enemyHealth);
        }
        if(enemyAttackCoolDown > enemy.GetComponent<EnemyHealth>().enemyAttackCoolDown)
        {
            enemyAttackCoolDown = 0;
            health -= enemy.GetComponent<EnemyHealth>().enemyAttack;
            if (health < 1) {
                enemy.GetComponent<EnemyHealth>().playerCloseEnemy--;
                LeaveFromCrew();
            }
        }
    }

    private bool IsMouseInScreen()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width)
            return false;
        if (Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return false;
        return true;
    }
    void Move()//Character movement with mouse or touch.
    {
        Vector3 locVel = Vector3.zero;
        if (Input.GetMouseButton(0) && IsMouseInScreen())
            screenClick = true;
        else if (Input.GetMouseButtonUp(0))
        {
            screenClick = false;
            anim.SetBool("Run", false);
        }
        float x;
        x = Input.mousePosition.x;
        if (x > Screen.width)
            x = Screen.width;
        else if (x < 0)
            x = 0;
        if (screenClick)
        {
            locVel.x = (x - Screen.width / 2) * speedX * Time.deltaTime / Screen.width;
            anim.SetBool("Run", true);
            locVel.z = speedForward * Time.deltaTime;
        }
        transform.position += transform.TransformDirection(locVel);
    }
    private void OnTriggerEnter(Collider other)//When an object triggered. 
    {
        if (!canGo && other.gameObject.tag.Equals("Player"))
            JoinToCrew();
        else if (other.gameObject.tag.Equals("Obstacles"))
            LeaveFromCrew();
        else if (other.gameObject.tag.Equals("Finish"))
            FinishGame();
        else if (other.gameObject.tag.Equals("Enemy"))
            RangedEnemy();
    }

    private void RangedEnemy()
    {
        enemy.GetComponent<EnemyHealth>().playerCloseEnemy++;
        enemyInRange = true;
    }

    private void FinishGame()
    {
        UIController.Instance.SetSliderActive();
        GameControll.Instance.Ended = true;
    }

    private void LeaveFromCrew()
    {
        GameControll.Instance.LeftFromCrew(transform);
    }

    private void JoinToCrew()
    {
        gameObject.tag = "Player";
        canGo = true;
        skinned.sharedMaterial = clothesMaterial;
        GameControll.Instance.JoinedToCrew(transform);
    }
}