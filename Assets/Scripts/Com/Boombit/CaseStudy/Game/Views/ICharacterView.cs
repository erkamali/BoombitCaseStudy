using Com.Boombit.CaseStudy.Game.Data;
using Com.Boombit.CaseStudy.Game.Utilities;

namespace Com.Boombit.CaseStudy.Game.Views
{
    public interface ICharacterView
    {
        int             ID              { get; }
        CharacterData   CharacterData   { get; }

        void Init(CharacterData data, IGameManager gameManager);
        
        void TakeDamage(float damage);
        void Die();
    }
}