using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using XKTools;


namespace YW.Thomi
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class Thominator : AIBase
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


        //-----------------SORT ENEMY HOME-----------------//
        /// <summary>
        /// Sort all the enemy homes among theirHomes
        /// </summary>
        /// <param name="theirHomes"></param>
        /// <returns></returns>
        List<IHome> SortEnemyHomes(IHome[] theirHomes)
        {
            List<IHome> enemyHomes = new List<IHome>();

            foreach (IHome theirHome in theirHomes)
            {
                if (theirHome.TeamId != -1)
                {
                    enemyHomes.Add(theirHome);
                }
            }

            return enemyHomes;
        }

        //-----------------SORT NEUTRAL HOME-----------------//
        /// <summary>
        /// Sort all the neutral homes among theirHomes
        /// </summary>
        /// <param name="theirHomes"></param>
        /// <returns></returns>
        List<IHome> SortNeutralHomes(IHome[] theirHomes)
        {
            List<IHome> neutralHomes = new List<IHome>();

            foreach (IHome theirHome in theirHomes)
            {
                if (theirHome.TeamId == -1)
                {
                    neutralHomes.Add(theirHome);
                }
            }

            return neutralHomes;
        }

        //-----------------LAUNCHER-----------------//
        /// <summary>
        /// Here is the brain of the Team take the crucial decision and summon the appropriate deadly functions
        /// </summary>
        void LaunchForGlory()
        {
            IHome[] myHomes = m_Gameboard.GetHomes(TeamId, true);
            IHome[] theirHomes = m_Gameboard.GetHomes(TeamId, false);
            List<IHome> enemyHomes = SortEnemyHomes(theirHomes);

            foreach (IHome myHome in myHomes)
            {
                AttackEnemy(myHome, myHomes, theirHomes);

                if (myHomes.Length >= 4 || CheckFreeHomes(theirHomes))
                {
                    if (enemyHomes.Count > 2)
                    {
                        Debug.Log("AttackNeutral" + myHomes.Length);
                        AttackNeutral(myHome, myHomes, theirHomes);
                    }
                }

                if (CheckForBigHome(myHomes))
                {
                    Debug.Log("BigHome");
                    SpreadCuzItsFun();
                }

                if (CompareTotalBoldiesBetweenMyHomesAndTheirHomes(myHomes, theirHomes) && myHomes.Length > 2)
                {
                    Debug.Log("AttackLessPowerful");
                    AttackTheLessPowerful(theirHomes, myHome);
                }
            }
        }

        //-----------------COMPARE-----------------//
        /// <summary>
        /// Compare if i have a bigger team than one of the team in the battle field 
        /// </summary>
        /// <param name="myHomes"></param>
        /// <param name="theirHomes"></param>
        /// <returns></returns>
        bool CompareTotalBoldiesBetweenMyHomesAndTheirHomes(IHome[] myHomes, IHome[] theirHomes)
        {
            List<IHome> enemyHomes = SortEnemyHomes(theirHomes);
            List<int> EnemyTeamIdHome = new List<int>();
            bool biggerThanThem = false;
            int countMine = 0;
            int countTheir = 1000;

            foreach (IHome myHome in myHomes)
            {
                countMine += myHome.BoldiCount;
            }

            foreach (IHome enemyHome in enemyHomes)
            {
                EnemyTeamIdHome.Add(enemyHome.TeamId);
            }

            List<int> disinctHome = EnemyTeamIdHome.Distinct().ToList();

            foreach (int home in disinctHome)
            {
                foreach (IHome enemyHome in enemyHomes)
                {
                    if (enemyHome.TeamId == home)
                    {
                        if (enemyHome.BoldiCount < countTheir)
                        {
                            countTheir = enemyHome.BoldiCount;
                        }
                    }
                }
            }

            if (countMine > countTheir)
            {
                biggerThanThem = true;
            }

            return biggerThanThem;
        }

        /// <summary>
        /// Evaluate if i have a home with more than 50 boldies
        /// </summary>
        /// <param name="myHomes"></param>
        /// <returns></returns>
        bool CheckForBigHome(IHome[] myHomes)
        {
            bool BigHomes = false;

            foreach (IHome myHome in myHomes)
            {
                if (myHome.BoldiCount > 50)
                {
                    BigHomes = true;
                }
            }

            return BigHomes;
        }

        /// <summary>
        /// Check if there is a neutral home with less than 2 boldies in the battle field
        /// </summary>
        /// <param name="theirHomes"></param>
        /// <returns></returns>
        bool CheckFreeHomes(IHome[] theirHomes)
        {
            bool FreeHomes = false;
            List<IHome> neutralFreeHomes = SortNeutralHomes(theirHomes);

            foreach (IHome nFreeHomes in neutralFreeHomes)
            {
                if (nFreeHomes.BoldiCount < 2)
                {
                    FreeHomes = true;
                }
            }

            return FreeHomes;
        }

        /// <summary>
        /// Attack Enemy depend on the boldies number and the magnitude of the enemy
        /// </summary>
        /// <param name="myHome"></param>
        /// <param name="myHomes"></param>
        /// <param name="theirHomes"></param>
        void AttackEnemy(IHome myHome, IHome[] myHomes, IHome[] theirHomes)
        {
            List<IHome> enemyHomes = SortEnemyHomes(theirHomes);
            List<float> minDistEnemy = new List<float>();
            IHome choosenEnemyOne = null;

            foreach (IHome enemyHome in enemyHomes)
            {
                if (enemyHome.BoldiCount != 0
                    && Math.Abs(myHome.BoldiCount) / Math.Abs(enemyHome.BoldiCount) > 2
                    && enemyHome.BoldiCount < myHome.BoldiCount
                    || enemyHome.BoldiCount == 0)
                {
                    minDistEnemy.Add(enemyHome.Position.magnitude);
                }
            }

            float[] arrayMinusEnemiesDist = minDistEnemy.ToArray();
            float minusEnemyDist = Mathf.Min(arrayMinusEnemiesDist);

            foreach (IHome enemyHome in enemyHomes)
            {
                if (minusEnemyDist == enemyHome.Position.magnitude)
                {
                    choosenEnemyOne = enemyHome;
                }
            }

            if (myHomes.Length > 0 && theirHomes.Length > 0 && choosenEnemyOne != null)
            {
                LaunchBoldies(myHome, choosenEnemyOne, (EAmount.ThreeQuarter));
            }
        }

        /// <summary>
        /// Attack Enemy depend on the boldies number and the magnitude of the enemy
        /// </summary>
        /// <param name="myHome"></param>
        /// <param name="myHomes"></param>
        /// <param name="theirHomes"></param>
        void AttackNeutral(IHome myHome, IHome[] myHomes, IHome[] theirHomes)
        {
            List<float> minusDistNeutral = new List<float>();
            List<IHome> neutralHome = SortNeutralHomes(theirHomes);
            IHome choosenNeutralOne = null;

            foreach (IHome neutral in neutralHome)
            {
                if (neutral.BoldiCount <= 2)
                {
                    LaunchBoldies(myHome, neutral, (EAmount.Quarter));
                }
                else if (Math.Abs(myHome.BoldiCount) / Math.Abs(neutral.BoldiCount) > 2 &&
                         neutral.BoldiCount < myHome.BoldiCount)
                {
                    minusDistNeutral.Add(neutral.Position.magnitude);
                }
            }

            float[] arrayMinusNeutralDist = minusDistNeutral.ToArray();
            float minustNeutralDist = Mathf.Min(arrayMinusNeutralDist);

            foreach (IHome neutral in neutralHome)
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

        /// <summary>
        /// Be conquerant spread everywhere in order to dominate the battle field
        /// </summary>
        void SpreadCuzItsFun()
        {
            // find a home which is mine
            IHome[] myHomes = m_Gameboard.GetHomes(TeamId, true);
            IHome[] theirHomes = m_Gameboard.GetHomes(TeamId, false);

            List<IHome> enemyHomes = SortEnemyHomes(theirHomes);

            foreach (IHome myHome in myHomes)
            {
                if (myHome.BoldiCount > 49)
                {
                    foreach (IHome enemyHome in enemyHomes)
                    {
                        if (enemyHome.BoldiCount != 0 && myHome.BoldiCount / enemyHome.BoldiCount >= 2)
                        {
                            if (myHomes.Length > 0 && theirHomes.Length > 0)
                            {
                                IHome from = myHome;
                                IHome to = enemyHome;
                                EAmount amount = (EAmount.Half);
                                LaunchBoldies(from, to, amount);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the the less powerfull team and attack it
        /// </summary>
        /// <param name="theirHomes"></param>
        /// <param name="myHome"></param>
        void AttackTheLessPowerful(IHome[] theirHomes, IHome myHome)
        {
            int temp = 10000;
            int lessPowerful = 0;
            List<IHome> enemyHomes = SortEnemyHomes(theirHomes);
            List<int> EnemyTeamIdHome = new List<int>();

            foreach (IHome enemyHome in enemyHomes)
            {
                EnemyTeamIdHome.Add(enemyHome.TeamId);
            }

            List<int> disinctHome = EnemyTeamIdHome.Distinct().ToList();

            foreach (int home in disinctHome)
            {
                foreach (IHome enemyHome in enemyHomes)
                {
                    if (enemyHome.TeamId == home)
                    {
                        if (enemyHome.BoldiCount < temp)
                        {
                            temp = enemyHome.BoldiCount;
                            lessPowerful = enemyHome.TeamId;
                        }
                    }
                }
            }

            foreach (IHome enemyHome in enemyHomes)
            {
                if (enemyHome.TeamId == lessPowerful)
                {
                    LaunchBoldies(myHome, enemyHome, (EAmount.Half));
                }
            }

            Debug.Log(temp);
            Debug.Log(lessPowerful);
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