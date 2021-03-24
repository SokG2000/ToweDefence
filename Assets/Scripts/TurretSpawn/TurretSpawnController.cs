using Field;
using Runtime;
using Turret;
using UnityEngine;
using Grid = Field.Grid;

namespace TurretSpawn
{
    public class TurretSpawnController: IController
    {
        private Grid m_Grid;
        private TurretMarket m_Market;

        public TurretSpawnController(Grid grid, TurretMarket market)
        {
            m_Grid = grid;
            m_Market = market;
        }

        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            if (m_Grid.HaveSelectedNode() && Input.GetMouseButtonDown(0))
            {
                Node selectedNode = m_Grid.GetSelectedNode();
                if (selectedNode.isOccupied  || !m_Grid.CanOccupyNode(selectedNode) )
                {
                    return;
                }
                SpawnTurret(m_Market.ChosenTurret, selectedNode);
            }
        }

        private void SpawnTurret(TurretAsset asset, Node node)
        {
            TurretView view = Object.Instantiate(asset.ViewPrefab);
            TurretData data = new TurretData(asset, node);
            
            data.AttachView(view);

            //node.isOccupied = true; // TryOccupy?
            //m_Grid.UpdatePathfinding();
            //m_Grid.UpdateOccupationAvailability();
            m_Grid.TryOccupyNode(node);
        }
    }
}