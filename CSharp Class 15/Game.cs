using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Class_15
{
    class Playerv1
    {
        private Allyv1 ally;
        private GameOverv1 gameOver;
        private Enemyv1 enemy;

        public Playerv1(Allyv1 ally, GameOverv1 gameOver, Enemyv1 enemy)
        {
            this.ally = ally;
            this.gameOver = gameOver;
            this.enemy = enemy;
        }

        private void Died()
        {
            ally.OnPlayerDead();
            gameOver.OnPlayerDead();
            enemy.OnPlayerDead();
        }
    }

    class Allyv1
    {
        public void OnPlayerDead()
        {

        }
    }

    class GameOverv1
    {
        public void OnPlayerDead()
        {

        }
    }

    class Enemyv1
    {
        public void OnPlayerDead()
        {

        }
    }

    //--------------------------------------------------

    delegate void PlayerDeadDelegate();

    class Playerv2
    {
        private List<PlayerDeadDelegate> playerDeadDelegates = new List<PlayerDeadDelegate>();

        public void Register(PlayerDeadDelegate playerDeadDelegate)
        {
            playerDeadDelegates.Add(playerDeadDelegate);
        }

        private void Died()
        {
            foreach (var playerDeadDelegate in playerDeadDelegates)
            {
                playerDeadDelegate();
            }
        }
    }

    class Allyv2
    {
        public Allyv2(Playerv2 player)
        {
            player.Register(OnPlayerDead);
        }

        private void OnPlayerDead()
        {

        }
    }

    class GameOverv2
    {
        public GameOverv2(Playerv2 player)
        {
            player.Register(OnPlayerDead);
        }

        private void OnPlayerDead()
        {

        }
    }

    class Enemyv2
    {
        public Enemyv2(Playerv2 player)
        {
            player.Register(OnPlayerDead);
        }

        private void OnPlayerDead()
        {

        }
    }

    //--------------------------------------------------

    class Playerv3
    {
        public event PlayerDeadDelegate PlayerDead;

        private void Died()
        {
            //PlayerDead(); // Hiba, ha nincs felirakozva senki

            // 1. kezelési mód:
            /*if (PlayerDead != null)
                PlayerDead();*/

            // 2. kezelési mód:
            PlayerDead?.Invoke();
        }
    }

    class Allyv3
    {
        public Allyv3(Playerv3 player)
        {
            player.PlayerDead += OnPlayerDead;
        }

        private void OnPlayerDead()
        {

        }
    }

    class GameOverv3
    {
        public GameOverv3(Playerv3 player)
        {
            player.PlayerDead += OnPlayerDead;
        }

        private void OnPlayerDead()
        {

        }
    }

    class Enemyv3
    {
        public Enemyv3(Playerv3 player)
        {
            player.PlayerDead += OnPlayerDead;
        }

        private void OnPlayerDead()
        {

        }
    }
}
