using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIProjectile : MonoBehaviour
{
    //cache projectile struct
    public ProjectileStruct projectileData;

    //initialize the projectile data
    public float projectileSpeed;
    public float projectileDmg;

    public float enemySpeedModifier;

    //rigidbody component
    Rigidbody rb;

    public IElementType type;

    public enum BulletElementType {
        NORMAL,
        FIRE,
        ICE,
        POISON,
    }

    public BulletElementType bulletElementType;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        projectileData = new ProjectileStruct(projectileSpeed, projectileDmg);
        UpdateAilement();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.forward * projectileSpeed - rb.velocity, ForceMode.Force);


    }

    //when the projectile hits a target, it will be destroyed
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemyAI")
        {
            //do damage to enemy
            other.GetComponent<IDoDamage>().DoDamage(projectileDmg);

            //
            other.GetComponent<Stats>().DoIceAilments(other.transform.gameObject, enemySpeedModifier);

            //disable the projectile
            transform.gameObject.SetActive(false);
        }
    }

    public IElementType UpdateAilement()
    {
        switch (bulletElementType) {
            case (BulletElementType.NORMAL):
                type = new Normal();
                return type;
            case (BulletElementType.FIRE):
                type = new Fire();
                return type;
            case (BulletElementType.ICE):
                type = new Ice();
                Debug.Log("ICE");
                return type;
            case (BulletElementType.POISON):
                type = new Fire();
                return type;
        }
        return type;
    }
}
