using System.Collections.Generic;
using UnityEngine;
using XKTools;


namespace YW.NicoJ
{
    /// <summary>
    /// 
    /// </summary>
    public class AITester : AIBase
    {
        #region Members

        XKTimer             m_Timer             = null;

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
            StartTimer();
        }

        void LaunchRandom()
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