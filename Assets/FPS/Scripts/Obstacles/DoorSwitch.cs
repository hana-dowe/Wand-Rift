using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.OBSTACLES
{
    public class DoorSwitch : MonoBehaviour
    {
        // public Animator Animator;

        [Tooltip("The random hit damage effects")]
        public ParticleSystem[] RandomHitSparks;
        public GameObject ElectricityEffects;

        Health m_Health;
        Destructable m_destructable;
        const string k_AnimOnDamagedParameter = "OnDamaged";

        public GameObject door;
        void Start()
        {
             m_Health = GetComponent<Health>();
            if (!m_Health)
            {
                m_Health = GetComponentInParent<Health>();
            }

            ElectricityEffects.SetActive(false);

            // Subscribe to damage & death actions
            m_Health.OnDie += OnDie;

             m_destructable = GetComponent<Destructable>();
            if (!m_destructable)
            {
                m_destructable = GetComponentInChildren<Destructable>();
            }

            m_destructable.OverrideOnDie = true;
            m_destructable.ShouldTakeDamage += ShouldTakeDamage;
        }      

        void ShouldTakeDamage(float damage, GameObject damageSource) 
        {
            // only take damage if electricity attack
            if (!damageSource.name.Contains("Electricity")) {
                return;
            }

            if (RandomHitSparks.Length > 0)
            {
                int n = Random.Range(0, RandomHitSparks.Length - 1);
                RandomHitSparks[n].Play();
            }

           // Animator.SetTrigger(k_AnimOnDamagedParameter);
           ElectricityEffects.SetActive(true);

            m_Health.TakeDamage(damage, damageSource);

            //Destroy(door);

        }

        void OnDie()
        {
            // this will call the OnDestroy function
            Destroy(door);
        }
    }

}