using Game.Gameplay.Monsters.Character;

namespace Game.Gameplay.Monsters.MonstersIntegrations.LittleGirl
{
    public class LittleGirlAnimationsController : AMonsterAnimationsController
    {
        private readonly int m_isMovingAnimationKey = UnityEngine.Animator.StringToHash("IsMoving");
        
        public void SetIsMoving(bool a_isMoving)
        {
            Animator.SetBool(m_isMovingAnimationKey, a_isMoving);
        }
    }
}
