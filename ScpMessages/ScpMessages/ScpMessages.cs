using Exiled.API.Features;

namespace ScpMessages
{
    public class ScpMessages : Plugin<Config>
    {
        public static ScpMessages ConfigRef { get; private set; }
        public EventHandlers EventHandler { get; private set; }
        public override string Name => "ScpMessages";
        public override string Author => "TruthfullyHonest";

        public ScpMessages()
        {
            ConfigRef = this;
        }

        public override void OnEnabled()
        {
            if (EventHandler == null)
                EventHandler = new EventHandlers(this);

            Exiled.Events.Handlers.Player.Hurting += EventHandler.OnDamage;
            Exiled.Events.Handlers.Player.Shot += EventHandler.OnShoot;
            Exiled.Events.Handlers.Player.MedicalItemUsed += EventHandler.OnMedicalItemUse;
            Exiled.Events.Handlers.Player.InteractingDoor += EventHandler.OnDoorInteract;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandler.OnConsoleCommandSent;
            Exiled.Events.Handlers.Server.RestartingRound += EventHandler.OnServerEnd;
            Exiled.Events.Handlers.Player.Verified += EventHandler.OnPlayerJoin;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandler.OnServerStart;
            Log.Info("Loaded ScpMessages");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandler.OnServerStart;
            Exiled.Events.Handlers.Player.Verified -= EventHandler.OnPlayerJoin;
            Exiled.Events.Handlers.Server.RestartingRound -= EventHandler.OnServerEnd;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandler.OnConsoleCommandSent;
            Exiled.Events.Handlers.Player.InteractingDoor -= EventHandler.OnDoorInteract;
            Exiled.Events.Handlers.Player.MedicalItemUsed -= EventHandler.OnMedicalItemUse;
            Exiled.Events.Handlers.Player.Shot -= EventHandler.OnShoot;
            Exiled.Events.Handlers.Player.Hurting -= EventHandler.OnDamage;

            EventHandler = null;
            Log.Info("Un-Loaded ScpMessages");
        }
    }
}
