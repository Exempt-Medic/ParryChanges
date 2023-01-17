using Modding;
using System;
using HutongGames.PlayMaker.Actions;
using HKMirror;
using HKMirror.Reflection.SingletonClasses;

namespace ParryChanges
{
    public class ParryChangesMod : Mod
    {
        private static ParryChangesMod? _instance;

        internal static ParryChangesMod Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"An instance of {nameof(ParryChangesMod)} was never constructed");
                }
                return _instance;
            }
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public ParryChangesMod() : base("ParryChanges")
        {
            _instance = this;
        }

        public override void Initialize()
        {
            Log("Initializing");

            On.HeroController.NailParry += OnParry;
            On.HutongGames.PlayMaker.Actions.SendMessage.OnEnter += OnSendMessage;

            Log("Initialized");
        }
        private void OnSendMessage(On.HutongGames.PlayMaker.Actions.SendMessage.orig_OnEnter orig, SendMessage self)
        {
            orig(self);

            if (self.Fsm.GameObject.name == "Knight" && self.Fsm.Name == "Nail Arts" && self.State.Name == "Regain Control" && self.functionCall.FunctionName == "RegainControl")
            {
                HeroControllerR.nailChargeTime = PlayerDataAccess.equippedCharm_26 ? 0.75f : 1.35f;
            }
        }
        private void OnParry(On.HeroController.orig_NailParry orig, HeroController self)
        {
            orig(self);

            self.parryInvulnTimer = PlayerDataAccess.equippedCharm_4 ? 0.75f : 0.25f;

            if (PlayerDataAccess.equippedCharm_26)
            {
                HeroControllerR.nailChargeTime = 0.01f;
            }
        }
    }
}
