using UnityEngine;

namespace Duel
{
    public class DuelInput
    {
        private readonly KeyCode _reloading;
        private readonly KeyCode _block;
        private readonly KeyCode _fire;

        public DuelInput()
        {
            _reloading = KeyCode.A;
            _block = KeyCode.S;
            _fire = KeyCode.D;
        }

        public DuelInput(KeyCode reloading, KeyCode block, KeyCode fire)
        {
            this._reloading = reloading;
            this._block = block;
            this._fire = fire;
        }

        public Actions? ReadAction()
        {
            if (Input.GetKeyDown(_reloading)) return Actions.Reloading;
            if (Input.GetKeyDown(_block)) return Actions.Block;
            if (Input.GetKeyDown(_fire)) return Actions.Fire;
            return null;
        }
    }
}