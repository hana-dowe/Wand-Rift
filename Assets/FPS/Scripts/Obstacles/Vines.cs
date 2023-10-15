using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.OBSTACLES
{
    public class Vines : MonoBehaviour
    {
        // public Animator Animator;

        [Tooltip("The random hit damage effects")]
        public ParticleSystem[] RandomHitSparks;

        Destructable m_destructable;
        const string k_AnimOnDamagedParameter = "OnDamaged";

        void Start()
        {
            m_destructable = GetComponent<Destructable>();
            if (!m_destructable)
            {
                m_destructable = GetComponentInChildren<Destructable>();
            }

            m_destructable.ShouldTakeDamage += ShouldTakeDamage;
        }      

        void ShouldTakeDamage(float damage, GameObject damageSource) 
        {
            // only take damage if fire attack

            // hanatodo change name
            if (!damageSource.name.Contains("Turret")) {
                return;
            }

            if (RandomHitSparks.Length > 0)
            {
                int n = Random.Range(0, RandomHitSparks.Length - 1);
                RandomHitSparks[n].Play();
            }

           // Animator.SetTrigger(k_AnimOnDamagedParameter);

            m_destructable.TakeDamage(damage, damageSource);

        }
    }
}