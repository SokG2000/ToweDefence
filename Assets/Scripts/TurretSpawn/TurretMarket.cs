using Turret;

namespace TurretSpawn
{
    public class TurretMarket
    {
        private TurretMarketAsset m_Asset;
        public TurretAsset ChosenTurret => m_Asset.TurretAssets[0];

        public TurretMarket(TurretMarketAsset asset)
        {
            m_Asset = asset;
        }
    }
}