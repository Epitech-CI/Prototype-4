using UnityEngine;

namespace Duel
{
    public class DuelInput
    {
        public Actions? ReadAction()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                return Actions.Reloading;
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                return Actions.Block;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                return Actions.Fire;
            }

            return null;
        }
    }
}