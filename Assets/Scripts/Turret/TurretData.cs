using Field;
using Turret.Weapon;

namespace Turret
{
    public class TurretData
    {
        private TurretView m_View;
        private Node m_Node;
        private TurretAsset m_Asset;
        private ITurretWeapon m_Weapon;

        public ITurretWeapon Weapon => m_Weapon;

        public TurretView MView => m_View;
        public Node MNode => m_Node;

        public readonly TurretView EnemyAsset;

        public TurretData(TurretAsset asset, Node node)
        {
            m_Asset = asset;
            m_Node = node;
        }

        public void AttachView(TurretView view)
        {
            m_View = view;
            m_View.AttachData(this);

            m_Weapon = m_Asset.WeaponAsset.GetWeapon(view);
        }
    }
}