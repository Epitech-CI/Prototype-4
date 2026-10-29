using System;
using Random = UnityEngine.Random;

namespace Duel
{
    public class DuelRules
    {
        public int PlayerAmmo { get; private set; }
        public int EnemyAmmo { get; private set; }

        public Actions EnemyAction()
        {
            Actions action = (Random.Range(0, CanFire(false) ? 3 : 2)) switch
                {
                    0 => Actions.Reloading,
                    1 => Actions.Block,
                    2 => Actions.Fire,
                    _ => throw new ArgumentOutOfRangeException()
                }
                ;
            UpdateAmmo(action, false);
            return action;
        }

        public Round ResolveRound(Actions player, Actions enemy)
        {
            return (player, enemy) switch
            {
                (Actions.Fire, Actions.Reloading) => Round.PlayerWins,
                (Actions.Fire, Actions.Nothing) => Round.PlayerWins,
                (Actions.Reloading, Actions.Fire) => Round.EnemyWins,
                (Actions.Nothing, Actions.Fire) => Round.EnemyWins,
                (Actions.Fire, Actions.Fire) => Round.Draw,
                (Actions.Block, Actions.Fire) => Round.Blocked,
                (Actions.Fire, Actions.Block) => Round.Blocked,
                (Actions.Reloading, Actions.Reloading) => Round.Reload,
                _ => Round.Continue
            };
        }


        public bool CanFire(bool isPlayer)
        {
            if (isPlayer) return PlayerAmmo > 0;
            return EnemyAmmo > 0;
        }

        public void UpdateAmmo(Actions action, bool isPlayer)
        {
            int delta = action switch { Actions.Reloading => 1, Actions.Fire => -1, _ => 0 };
            if (CanFire(isPlayer) == false && action == Actions.Fire) return;
            if (isPlayer) PlayerAmmo += delta;
            else EnemyAmmo += delta;
        }

        public bool IsGameOver(Round round)
        {
            return round is Round.PlayerWins or Round.EnemyWins or Round.Draw;
        }
    }
}