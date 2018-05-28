using System;
using System.Collections.Generic;
using UnityEngine;
using XKTools;


namespace YW.Thomi
{
    /// <summary>
    /// 
    /// </summary>
    public class AIThomi : AIBase
    {
        #region Members

        XKTimer m_Timer = null;

        #endregion


        #region Inherited Manipulators

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            m_Timer = AddXKComponent<XKTimer>();
            m_Timer.OnEnd = OnEndTimer;

            StartTimer();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            base.Update();

            // basic test helper
            if (Input.GetKeyDown(KeyCode.A))
            {
                LaunchRandom();
            }

            // basic test helper
            if (Input.GetKeyDown(KeyCode.Q) && TeamId == 0)
            {
                LaunchBoldies(EAmount.Quarter);
            }

            // basic test helper
            if (Input.GetKeyDown(KeyCode.H) && TeamId == 0)
            {
                LaunchBoldies(EAmount.Half);
            }

            // basic test helper
            if (Input.GetKeyDown(KeyCode.T) && TeamId == 0)
            {
                LaunchBoldies(EAmount.ThreeQuarter);
            }

            // basic test helper
            if (Input.GetKeyDown(KeyCode.F) && TeamId == 0)
            {
                LaunchBoldies(EAmount.Full);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public override void OnBoldiLaunch(IHome from, IHome to)
        {
            base.OnBoldiLaunch(from, to);

            // don't care, I'm the one that launched the boldies
            if (from.TeamId == TeamId)
                return;

            // don't care, I'm not the target, though it may be a great idea to strike back at the offender
            if (to.TeamId != TeamId)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="home"></param>
        /// <param name="formerTeamId"></param>
        public override void OnHomeChangedOwner(IHome home, int formerTeamId)
        {
            base.OnHomeChangedOwner(home, formerTeamId);
        }

        #endregion


        #region Private Manipulators

        void StartTimer()
        {
            m_Timer.StartTimer(Lehmer.Range(2.0f, 5.0f));
        }

        void OnEndTimer()
        {
            LaunchRandom();
            //LaunchSpread();
            StartTimer();
        }

        void LaunchSpread()
        {
            // find a home which is mine
            IHome[] myHomes = m_Gameboard.GetHomes(TeamId, true);
            IHome[] theirHomes = m_Gameboard.GetHomes(TeamId, false);


            List<IHome> neutralBoldiesPerHome = new List<IHome>();
            List<IHome> enemiesBoldiesPerHome = new List<IHome>();

            foreach (var myHome in myHomes)
            {
                foreach (var theirHome in theirHomes)
                {
                    if (theirHome.TeamId == -1)
                    {
                        neutralBoldiesPerHome.Add(theirHome);
                    }

                    enemiesBoldiesPerHome.Add(theirHome);
                }

                foreach (var neutralBoldiesPerHom in neutralBoldiesPerHome)
                {
                    if (neutralBoldiesPerHom.BoldiCount != 0
                        && myHome.BoldiCount / neutralBoldiesPerHom.BoldiCount >= 2
                        && myHome.BoldiCount > neutralBoldiesPerHom.BoldiCount
                        || neutralBoldiesPerHom.BoldiCount == 0)
                    {
                        if (myHomes.Length > 0 && theirHomes.Length > 0)
                        {
                            IHome from = myHome;
                            IHome to = neutralBoldiesPerHom;
                            EAmount amount = (EAmount.ThreeQuarter);
                            LaunchBoldies(from, to, amount);
                        }
                    }
                }

                foreach (var enemiesBoldiesPerHom in enemiesBoldiesPerHome)
                {
                    if (enemiesBoldiesPerHom.BoldiCount != 0
                        && myHome.BoldiCount / enemiesBoldiesPerHom.BoldiCount >= 2
                        && myHome.BoldiCount > enemiesBoldiesPerHom.BoldiCount
                        || enemiesBoldiesPerHom.BoldiCount == 0)
                    {
                        if (myHomes.Length > 0 && theirHomes.Length > 0)
                        {
                            IHome from = myHome;
                            IHome to = enemiesBoldiesPerHom;
                            EAmount amount = (EAmount.ThreeQuarter);
                            LaunchBoldies(from, to, amount);
                        }
                    }
                }
            }
        }

        void LaunchRandom()
        {
            // find a home which is mine
            IHome[] myHomes = m_Gameboard.GetHomes(TeamId, true);
            IHome[] theirHomes = m_Gameboard.GetHomes(TeamId, false);
            
            
            List<int> neutralBoldiesPerHome = new List<int>();
            List<int> enemiesBoldiesPerHome = new List<int>();
            
            foreach (var myHome in myHomes)
            {
                AttackEnemy(myHome, myHomes, theirHomes);
                AttackNeutral(myHome, myHomes, theirHomes);    

                
                /*foreach (var theirHome in theirHomes)
                {
                    if (theirHome.TeamId == -1)
                    {
                        neutralBoldiesPerHome.Add(theirHome.BoldiCount);
                    }
                
                    enemiesBoldiesPerHome.Add(theirHome.BoldiCount);
                
                    if (enemiesBoldiesPerHome[enemiesBoldiesPerHome.Count -1] != 0 && myHome.BoldiCount / enemiesBoldiesPerHome[enemiesBoldiesPerHome.Count -1] >= 2 )
                    {
                        if (myHomes.Length > 0 && theirHomes.Length > 0)
                        {
                            IHome from = myHome;
                            IHome to = theirHome;    
                            EAmount amount = (EAmount.ThreeQuarter);
                            LaunchBoldies(from, to, amount); 
                        }
                    }
                }*/
            }
        }

        void AttackEnemy(IHome myHome, IHome[] myHomes, IHome[] theirHomes)
        {
                        
            List<IHome>     enemyHomes         =     SortEnemyHomes(theirHomes);
            List<float>     minusDistEnemies   =     new List<float>();
            IHome           choosenEnemyOne    =     null ;
                
            foreach (var enemyHome in enemyHomes)
            {
                if (enemyHome.BoldiCount != 0 
                    && Math.Abs(myHome.BoldiCount) / Math.Abs(enemyHome.BoldiCount) > 2 
                    && enemyHome.BoldiCount < myHome.BoldiCount
                    || enemyHome.BoldiCount == 0)
                {
                    minusDistEnemies.Add(enemyHome.Position.magnitude);
                }
            }
                
            float[] arrayMinusEnemiesDist = minusDistEnemies.ToArray();
            float minusEnemyDist = Mathf.Min(arrayMinusEnemiesDist);
                
            foreach (var enemyHome in enemyHomes)
            {
                if (minusEnemyDist == enemyHome.Position.magnitude)
                {
                    choosenEnemyOne = enemyHome;
                }
            }

            if (myHomes.Length > 0 && theirHomes.Length > 0 && choosenEnemyOne != null)
            {
                LaunchBoldies(myHome, choosenEnemyOne, (EAmount.Half));
            }
        }
        
        void AttackNeutral(IHome myHome, IHome[] myHomes, IHome[] theirHomes)
        {
            List<float>     minusDistNeutral     =     new List<float>();
            List<IHome>     neutralHome          =     SortNeutralHomes(theirHomes);
            IHome           choosenNeutralOne    =     null;

            foreach (var neutral in neutralHome)
            {
                if (neutral.BoldiCount <= 2)
                {
                    LaunchBoldies(myHome, neutral, (EAmount.Quarter));
                }
                else if (Math.Abs(myHome.BoldiCount) / Math.Abs(neutral.BoldiCount) > 2 && neutral.BoldiCount < myHome.BoldiCount)
                {
                    minusDistNeutral.Add(neutral.Position.magnitude);
                }
            }

            float[] arrayMinusNeutralDist = minusDistNeutral.ToArray();
            float minustNeutralDist = Mathf.Min(arrayMinusNeutralDist);
                
                
            foreach (var neutral in neutralHome)
            {
                if (minustNeutralDist == neutral.Position.magnitude)
                {
                    choosenNeutralOne = neutral;
                    minusDistNeutral.Remove(neutral.Position.magnitude);
                }
            }
                
            if (myHomes.Length > 0 && theirHomes.Length > 0 && choosenNeutralOne != null)
            {
                LaunchBoldies(myHome, choosenNeutralOne, (EAmount.Half));
            }
        }
        
        List<IHome> SortEnemyHomes(IHome[] theirHomes)
        {
            List<IHome> enemiesHome = new List<IHome>();
            
            foreach (var theirHome in theirHomes)
            {
                if (theirHome.TeamId != -1)
                {
                    enemiesHome.Add(theirHome);
                }
            }

            return enemiesHome; 
        }
        
        List<IHome> SortNeutralHomes(IHome[] theirHomes)
        {
            List<IHome> neutralHome = new List<IHome>();
            
            foreach (var theirHome in theirHomes)
            {
                if (theirHome.TeamId == -1)
                {
                    neutralHome.Add(theirHome);
                }
            }
            
            return neutralHome; 
        }
        
        
        void LaunchBoldies(EAmount amount)
        {
            // find a home which is mine
            IHome[] myHomes = m_Gameboard.GetHomes(TeamId, true);
            IHome[] theirHomes = m_Gameboard.GetHomes(TeamId, false);

            // launch boldies
            if (myHomes.Length > 0 && theirHomes.Length > 0)
                LaunchBoldies(myHomes[0], theirHomes[0], amount);
        }

        #endregion


        #region Public Accessors

        #endregion
    }
}