using Assets;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;

        public EnemyView MView => m_View;

        public readonly EnemyAsset EnemyAsset;

        public EnemyData(EnemyAsset asset)
        {
            EnemyAsset = asset;
        }

        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }
    }
}