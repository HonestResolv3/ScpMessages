using Exiled.API.Features;
using System;

namespace ScpMessages
{
    public class ScpMessages : Plugin<Config>
    {
        public static ScpMessages ConfigRef { get; private set; }
        public EventHandlers EventHandler { get; private set; }
        public override string Name => "ScpMessages";
        public override string Author => "HonestResolv3";
        public override Version Version => new Version(1, 2, 0, 3);
        public override Version RequiredExiledVersion => new Version(2, 10, 0);

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
            if (ConfigRef.Config.EnableDebugStartupMessage)
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
            if (ConfigRef.Config.EnableDebugStartupMessage)
                Log.Info("Un-Loaded ScpMessages");
        }
    }
}
