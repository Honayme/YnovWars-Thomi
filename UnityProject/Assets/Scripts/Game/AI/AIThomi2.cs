using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using XKTools;


namespace YW.Thomi
{
    /// <summary>
    /// 
    /// </summary>
    public class AIThomiQuiFaitDeLaMerde : AIBase
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
                LaunchForGlory();
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
            LaunchForGlory();
            //LaunchSpread();
            StartTimer();
        }



        //use the begining spread in the fisrt IA to be a fuckin conquerant
        //Take in case the total number of boldies to attack 
        //-----------------SORT ENEMIE HOME-----------------//
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
        
        //-----------------SORT NEUTRAL HOME-----------------//
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
        

        void LaunchForGlory()
        {
            IHome[] myHomes = m_Gameboard.GetHomes(TeamId, true);
            IHome[] theirHomes = m_Gameboard.GetHomes(TeamId, false);
            
            foreach (var myHome in myHomes)
            {
                AttackEnemy(myHome, myHomes, theirHomes);

                if (myHomes.Length > 4 || CheckFreeHomes(theirHomes))
                {
                    AttackNeutral(myHome, myHomes, theirHomes);    
                }

                if (CheckForBigHome(myHomes))
                {
                    SpreadCuzItsFun();
                }
                  
            }
        }

        bool CompareTotalBoldiesBetweenMyHomesAndTheirHomes(IHome[] myHomes, IHome[] theirHomes)
        {
            bool BiggerThanThem = false;
            int CountMine = 0;
            int CountTheir = 0; 

            foreach (var myHome in myHomes)
            {
                CountMine += myHome.BoldiCount; 
            }

            foreach (var theirHome in theirHomes)
            {
                CountTheir += theirHome.BoldiCount; 
            }

            if (CountMine > CountTheir)
            {
                BiggerThanThem = true; 
            }
            return BiggerThanThem; 
        }

        bool CheckForBigHome(IHome[] myHomes)
        {
            bool BigHomes = false;
            
            foreach (var myHome in myHomes)
            {
                if (myHome.BoldiCount > 50)
                {
                    BigHomes = true;
                }
            }

            return BigHomes; 
        }
        
        bool CheckFreeHomes(IHome[] theirHomes)
        {
            bool FreeHomes                 =     false;
            List<IHome> neutralFreeHomes   =     SortNeutralHomes(theirHomes);

            foreach (var nFreeHomes in neutralFreeHomes)
            {
                if (nFreeHomes.BoldiCount < 2)
                {
                    FreeHomes = true; 
                }
            }

            return FreeHomes; 
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
                
            Debug.Log("Smaller==="+minusEnemyDist);

            foreach (var enemyHome in enemyHomes)
            {
                if (minusEnemyDist == enemyHome.Position.magnitude)
                {
                    Debug.Log("Smaller=======" + minusEnemyDist);
                    choosenEnemyOne = enemyHome;
                }
            }

            if (myHomes.Length > 0 && theirHomes.Length > 0 && choosenEnemyOne != null)
            {
                Debug.Log("@@@@ENEMY@@@@      " + choosenEnemyOne.BoldiCount);
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
                    Debug.Log(neutral.BoldiCount);
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
        
        void SpreadCuzItsFun()
        {
            // find a home which is mine
            IHome[] myHomes = m_Gameboard.GetHomes(TeamId, true);
            IHome[] theirHomes = m_Gameboard.GetHomes(TeamId, false);
            
            List<int> enemiesBoldiesPerHome = new List<int>();
            
            foreach (var myHome in myHomes)
            {
                if (myHome.BoldiCount > 49)
                {
                    foreach (var theirHome in theirHomes)
                    {
                        if (theirHome.TeamId != -1)
                        {
                            enemiesBoldiesPerHome.Add(theirHome.BoldiCount);
                        }
                
                        if (enemiesBoldiesPerHome[enemiesBoldiesPerHome.Count -1] != 0 && myHome.BoldiCount / enemiesBoldiesPerHome[enemiesBoldiesPerHome.Count -1] >= 2 )
                        {
                            if (myHomes.Length > 0 && theirHomes.Length > 0)
                            {
                                IHome from = myHome;
                                IHome to = theirHome;    
                                EAmount amount = (EAmount.Half);
                                LaunchBoldies(from, to, amount); 
                            }
                        }
                    }    
                }
                
            }
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