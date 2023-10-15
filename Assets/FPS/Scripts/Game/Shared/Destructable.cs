using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public class Destructable : MonoBehaviour
    {
        Health m_Health;

        public UnityAction<float, GameObject>  ShouldTakeDamage;
        public bool OverrideOnDie = false;

        void Start()
        {
            m_Health = GetComponent<Health>();
            if (!m_Health)
            {
                m_Health = GetComponentInParent<Health>();
            }

            // Subscribe to damage & death actions
            m_Health.OnDie += OnDie;
            m_Health.OnDamaged += OnDamaged;
        }

        public void OnDamaged(float damage, GameObject damageSource)
        {
            // hanatodo only fire can burn vines

            ShouldTakeDamage(damage, damageSource);

            

            // damage reaction
            //m_Health.TakeDamage(damage, gameObject);
        }

        public void TakeDamage(float damage, GameObject damageSource) {
            m_Health.TakeDamage(damage, damageSource);
        }

        void OnDie()
        {
            // this will call the OnDestroy function
            if (!OverrideOnDie) {
                Destroy(gameObject);
            }
        }
    }
}