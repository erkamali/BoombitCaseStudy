using Com.Boombit.CaseStudy.Data;

namespace Com.Boombit.CaseStudy.Views
{
    public interface ICharacterView
    {
        CharacterData CharacterData { get; }

        void Init(CharacterData data);
        
        void TakeDamage(float damage);
        void Die();
    }
}