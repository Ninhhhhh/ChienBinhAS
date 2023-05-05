using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtPickup : MonoBehaviour
{

    public int healthRestore = 20;
    // Start is called before the first frame update

    public Vector3 spinRotationSpeed = new Vector3(0, 100, 0);

    AudioSource pickUpSouce;

    private void Awake()
    {
        pickUpSouce = GetComponent<AudioSource>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
     
    }

    private void OnTriggerEnter2D(Collider2D collision)          
    {
        Damageable damage = collision.GetComponent<Damageable>();

        if (damage)
        {

            bool wasHealed = damage.Heal(healthRestore);
            
                if (wasHealed) 
                    if (pickUpSouce)
                    
                        AudioSource.PlayClipAtPoint(pickUpSouce.clip, gameObject.transform.position, pickUpSouce.volume);
                        Destroy(gameObject);
                    
                

        }
     }
} 
