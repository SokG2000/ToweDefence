using Runtime;

namespace Field
{
    public class GridPointerController : IController
    {
        private GridHolder m_GridHolder;

        public GridPointerController(GridHolder mGridHolder)
        {
            m_GridHolder = mGridHolder;
        }

        public void OnStart()
        {
            
        }

        public void OnStop()
        {
            
        }

        public void Tick()
        {
            m_GridHolder.RayCastInGrid();
        }
    }
}