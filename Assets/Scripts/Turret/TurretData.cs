using Field;

namespace Turret
{
    public class TurretData
    {
        private TurretView m_View;
        private Node m_Node;
        private TurretAsset TurretAsset;
        
        public TurretView MView => m_View;
        public Node MNode => m_Node;

        public readonly TurretView EnemyAsset;

        public TurretData(TurretAsset asset, Node node)
        {
            TurretAsset = asset;
            m_Node = node;
        }

        public void AttachView(TurretView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }
    }
}