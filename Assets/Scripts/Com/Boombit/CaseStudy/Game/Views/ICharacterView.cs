using Com.Boombit.CaseStudy.Game.Data;

namespace Com.Boombit.CaseStudy.Game.Views
{
    public interface ICharacterView
    {
        CharacterData CharacterData { get; }

        void Init(CharacterData data);
        
        void TakeDamage(float damage);
        void Die();
    }
}