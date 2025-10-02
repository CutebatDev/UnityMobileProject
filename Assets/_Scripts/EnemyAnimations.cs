using UnityEngine;

namespace _Scripts
{
    public delegate void AnimationEvents();
    public class EnemyAnimations : MonoBehaviour
    {
        public event AnimationEvents OnEnemyDeath;
        public Animator animator;
        public void PlayDeathAnimation()
        {
            animator.SetTrigger("Dead");
        }

        // OnEnemyDeath is invoked when death animation is finished <- 
        private void DeathAnimationFinished()
        {
            OnEnemyDeath?.Invoke();
        }
    }
}
