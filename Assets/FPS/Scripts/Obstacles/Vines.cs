using Unity.FPS.Game;
using UnityEngine;
using System.Collections;

namespace Unity.FPS.OBSTACLES
{
    public class Vines : MonoBehaviour
    {
        Health m_Health;
        // public Animator Animator;

        [Tooltip("The random hit damage effects")]
        public ParticleSystem[] RandomHitSparks;

        public GameObject Mesh;
        public GameObject FireEffects;

        Destructable m_destructable;
        const string k_AnimOnDamagedParameter = "OnDamaged";

        void Start()
        {
            m_Health = GetComponent<Health>();
            m_Health.OnDie += OnDie;

            m_destructable = GetComponent<Destructable>();
            if (!m_destructable)
            {
                m_destructable = GetComponentInChildren<Destructable>();
            }


            FireEffects.SetActive(false);

            m_destructable.OverrideOnDie = true;
            m_destructable.ShouldTakeDamage += ShouldTakeDamage;
        }      

        void ShouldTakeDamage(float damage, GameObject damageSource) 
        {
            // only take damage if fire attack
            if (!damageSource.name.Contains("Fire")) {
                return;
            }

            if (RandomHitSparks.Length > 0)
            {
                int n = Random.Range(0, RandomHitSparks.Length - 1);
                RandomHitSparks[n].Play();
            }

           // Animator.SetTrigger(k_AnimOnDamagedParameter);
           FireEffects.SetActive(true);

            m_destructable.TakeDamage(damage, damageSource);

        }

        IEnumerator BurnVines()
        {
            yield return new WaitForSeconds(0.3f);
            Mesh.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            Destroy(gameObject);
        }

        void OnDie()
        {
            StartCoroutine(BurnVines());
        }


    }
}