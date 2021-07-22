using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Hints;
using Interactables.Interobjects.DoorUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScpMessages
{
    public class EventHandlers
    {
        readonly Random NumGen = new Random();

        readonly TwoTupleSO PlayerTargetData = new TwoTupleSO("player", null);
        readonly TwoTupleSO PlayerAttackerData = new TwoTupleSO("player", null);
        readonly TwoTupleSO DamageData = new TwoTupleSO("damage", null);
        readonly TwoTupleSO HitboxData = new TwoTupleSO("hitbox", null);
        readonly TwoTupleSO HealthData = new TwoTupleSO("health", null);
        readonly TwoTupleSO ArtificialData = new TwoTupleSO("adrhealth", null);

        readonly TwoTupleSOList AttackerDamage = new TwoTupleSOList();
        readonly TwoTupleSOList TargetDamage = new TwoTupleSOList();
        readonly TwoTupleSOList TargetHitboxDamage = new TwoTupleSOList();
        readonly TwoTupleSOList AttackerHitboxDamage = new TwoTupleSOList();
        readonly TwoTupleSOList Target = new TwoTupleSOList();
        readonly TwoTupleSOList Damage = new TwoTupleSOList();
        readonly TwoTupleSOList Artificial = new TwoTupleSOList();
        readonly TwoTupleSOList Health = new TwoTupleSOList();

        readonly TextHint Hint = new TextHint("", new HintParameter[]
        {
            new StringHintParameter("")
        }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f));

        readonly ScpMessages Plugin;

        readonly string AreaForFile = Path.Combine(Paths.Configs, "ScpMessages");
        readonly string FileName;

        Dictionary<string, bool> Players = new Dictionary<string, bool>();

        DoorLockMode mode;

        ItemCategory ItemCat;

        bool ItemInHandIsKeycard;

        public EventHandlers(ScpMessages Plugin)
        {
            this.Plugin = Plugin;
            FileName = Path.Combine(AreaForFile, "ScpMessages.txt");
        }

        public void OnDamage(HurtingEventArgs Hurt)
        {
            if (!Plugin.Config.DamageMessageEnabled)
                return;

            int Chance = NumGen.Next(0, 100);
            if (Chance > Plugin.Config.DamageMessageChance)
                return;

            if (Hurt.Target.IsHuman && Plugin.Config.HumansReceiveMessage)
                ProcessDamageHint(Hurt);

            else if (Hurt.Target.IsScp && Plugin.Config.ScpsReceiveMessage)
                ProcessDamageHint(Hurt);

            if (Hurt.Attacker.IsScp && Plugin.Config.ScpsReceiveMessage)
                ProcessScpDamageHint(Hurt);
        }

        public void OnShoot(ShotEventArgs Shoot)
        {
            if (!Plugin.Config.DamageMessageEnabled || !Plugin.Config.HumansReceiveMessage)
                return;

            ProcessShotHit(Shoot);
        }

        public void OnDoorInteract(InteractingDoorEventArgs Door)
        {
            int Chance = NumGen.Next(0, 100);
            if (!Plugin.Config.DoorMessageEnabled || (Chance > Plugin.Config.DoorMessageChance) || !Door.Door.TryGetComponent(out DoorNametagExtension _) 
                || (Door.Player.IsHuman && !Plugin.Config.HumansReceiveMessage) 
                || Door.Player.IsScp)
                return;

            mode = DoorLockUtils.GetMode((DoorLockReason)Door.Door.ActiveLocks);
            if (mode == DoorLockMode.FullLock)
            {
                ShowHintDisplay(Door.Player, Plugin.Config.FullLockdownMessage);
                return;
            }

            ItemInHandIsKeycard = ItemInHandIsKeycardForPlayer(Door.Player);
            if (Door.Player.IsBypassModeEnabled)
            {
                if (ItemInHandIsKeycard)
                    ShowHintDisplay(Door.Player, Plugin.Config.BypassDoorKeycardMessage);
                else
                    ShowHintDisplay(Door.Player, Plugin.Config.BypassDoorMessage);
                return;
            }

            if (!Door.IsAllowed)
            {
                if (!ItemInHandIsKeycard)
                    ShowHintDisplay(Door.Player, Plugin.Config.LockedDoorMessage);
                else
                    ShowHintDisplay(Door.Player, Plugin.Config.LockedDoorKeycardMessage);
            }
            else
                ShowHintDisplay(Door.Player, Plugin.Config.UnlockedDoorKeycardMessage);
        }

        public void OnMedicalItemUse(UsedMedicalItemEventArgs Med)
        {
            int Chance = NumGen.Next(0, 100);
            if (Chance > Plugin.Config.MedicalItemMessageChance || 
                !Plugin.Config.MedicalItemMessageEnabled ||
                !Plugin.Config.HumansReceiveMessage ||
                !CheckForDisplayToggle(Med.Player))
                return;

            string Message;
            switch (Med.Item)
            {
                case ItemType.Painkillers:
                    HealthData.Item2 = 5;
                    Health.AddTwoTupleSO(HealthData);
                    Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.PainkillerHealMessage, '%', Health);
                    ShowHintDisplay(Med.Player, Message);
                    break;
                case ItemType.Medkit:
                    HealthData.Item2 = 65;
                    Health.AddTwoTupleSO(HealthData);
                    Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.MedkitHealMessage, '%', Health);
                    ShowHintDisplay(Med.Player, Message);
                    break;
                case ItemType.Adrenaline:
                    ArtificialData.Item2 = 30;
                    Artificial.AddTwoTupleSO(ArtificialData);
                    Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.AdrenalineHealMessage, '%', Artificial);
                    ShowHintDisplay(Med.Player, Message);
                    break;
                case ItemType.SCP207:
                    HealthData.Item2 = 30;
                    Health.AddTwoTupleSO(HealthData);
                    Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp207HealMessage, '%', Health);
                    ShowHintDisplay(Med.Player, Message);
                    break;
                case ItemType.SCP500:
                    ShowHintDisplay(Med.Player, Plugin.Config.Scp500HealMessage);
                    break;
            }
        }

        public void OnConsoleCommandSent(SendingConsoleCommandEventArgs Con)
        {
            if (Con.Name.ToLowerInvariant().Contains("scpmsg"))
            {
                Con.IsAllowed = false;
                if (Players.ContainsKey(Con.Player.UserId) && Players.TryGetValue(Con.Player.UserId, out bool value))
                    Players[Con.Player.UserId] = !value;
                else
                    Players.Add(Con.Player.UserId, true);

                if (Players[Con.Player.UserId])
                    Con.ReturnMessage = "Hint messages related to ScpMessages are now enabled for you";
                else
                    Con.ReturnMessage = "Hint messages related to ScpMessages are now disabled for you";
            }
        }

        public void OnPlayerJoin(VerifiedEventArgs Ver)
        {
            if (Plugin.Config.EnableToggleMessageOnJoin)
            {
                if (CheckForDisplayToggle(Ver.Player))
                    Ver.Player.Broadcast(15, "ScpMessages is on for you, you will see messages at the bottom when you do certain actions\nTo disable, do <color=orange>.scpmsg</color> in your console (tilde (~) key)");
                else
                    Ver.Player.Broadcast(15, "ScpMessages is off for you, you will not see messages at the bottom when you do certain actions\nTo enable, do <color=orange>.scpmsg</color> in your console (tilde (~) key)");
            }
        }

        public void OnServerStart()
        {
            if (!Directory.Exists(AreaForFile))
                Directory.CreateDirectory(AreaForFile);
            if (!File.Exists(FileName))
                File.Create(FileName);

            try
            {
                string Text = File.ReadAllText(FileName);
                if (!string.IsNullOrWhiteSpace(Text))
                    Players = JsonConvert.DeserializeObject<Dictionary<string, bool>>(Text);
            }
            catch (Exception)
            {
                Players = new Dictionary<string, bool>();
            }
        }

        public void OnServerEnd()
        {
            if (!File.Exists(FileName))
                File.Create(FileName);
            else
            {
                try
                {
                    string JsonObject = JsonConvert.SerializeObject(Players, Formatting.Indented);
                    File.WriteAllText(FileName, JsonObject);
                }
                catch (Exception)
                {

                }
            }
        }

        public void ProcessDamageHint(HurtingEventArgs Hurt)
        {
            int Chance = NumGen.Next(0, 100);
            if (Chance > Plugin.Config.DamageMessageChance)
                return;

            DamageData.Item2 = Math.Round(Hurt.Amount);
            PlayerAttackerData.Item2 = Hurt.Attacker.Nickname;
            PlayerTargetData.Item2 = Hurt.Attacker.Nickname;

            Damage.AddTwoTupleSO(DamageData);
            AttackerDamage.AddTwoTupleSO(PlayerAttackerData, DamageData);
            TargetDamage.AddTwoTupleSO(PlayerTargetData, DamageData);

            if (Hurt.DamageType == DamageTypes.Falldown)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.FallDamageMessage, '%', Damage);
                ShowHintDisplay(Hurt.Target, Message);
            }

            else if (Hurt.DamageType == DamageTypes.Tesla)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.TeslaDamageMessage, '%', Damage);
                ShowHintDisplay(Hurt.Target, Message);
            }
            else if (Hurt.DamageType == DamageTypes.Grenade)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.GrenadeDamageMessage, '%', AttackerDamage);
                string Message2 = TokenReplacer.ReplaceAfterToken(Plugin.Config.HumanGrenadeAttackMessage, '%', TargetDamage);
                ShowHintDisplay(Hurt.Attacker, Message2);
                ShowHintDisplay(Hurt.Target, Message);
            }
            else if (Hurt.DamageType == DamageTypes.MicroHid)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.MicroHidDamageMessage, '%', AttackerDamage);
                string Message2 = TokenReplacer.ReplaceAfterToken(Plugin.Config.HumanMicroHidAttackMessage, '%', TargetDamage);
                ShowHintDisplay(Hurt.Attacker, Message2);
                ShowHintDisplay(Hurt.Target, Message);
            }
            else if (Hurt.DamageType == DamageTypes.Scp049)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp049DamageMessage, '%', AttackerDamage);
                ShowHintDisplay(Hurt.Target, Message);
            }
            else if (Hurt.DamageType == DamageTypes.Scp0492)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp0492DamageMessage, '%', AttackerDamage);
                ShowHintDisplay(Hurt.Target, Message);
            }
            else if (Hurt.DamageType == DamageTypes.Scp096)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp096DamageMessage, '%', AttackerDamage);
                ShowHintDisplay(Hurt.Target, Message);
            }
            else if (Hurt.DamageType == DamageTypes.Scp106)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp106DamageMessage, '%', AttackerDamage);
                ShowHintDisplay(Hurt.Target, Message);
            }
            else if (Hurt.DamageType == DamageTypes.Scp173)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp173DamageMessage, '%', AttackerDamage);
                ShowHintDisplay(Hurt.Target, Message);
            }
            else if (Hurt.DamageType == DamageTypes.Scp939)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp939DamageMessage, '%', AttackerDamage);
                ShowHintDisplay(Hurt.Target, Message);
            }
        }

        public void ProcessShotHit(ShotEventArgs Shot)
        {
            int Chance = NumGen.Next(0, 100);
            Player Target = new Player(Shot.Target);
            if (Chance > Plugin.Config.DamageMessageChance || Shot.Target == null)
                return;

            DamageData.Item2 = Math.Round(Shot.Damage);
            PlayerTargetData.Item2 = Target.Nickname;
            PlayerAttackerData.Item2 = Shot.Shooter.Nickname;
            HitboxData.Item2 = Shot.HitboxTypeEnum.ToString().ToLowerInvariant();
            AttackerDamage.AddTwoTupleSO(PlayerTargetData, DamageData);
            TargetHitboxDamage.AddTwoTupleSO(PlayerTargetData, HitboxData, DamageData);
            AttackerHitboxDamage.AddTwoTupleSO(PlayerAttackerData, HitboxData, DamageData);

            if (Shot.Shooter.IsHuman && Target.IsScp)
            {
                string Message;
                string Message2 = TokenReplacer.ReplaceAfterToken(Plugin.Config.HumanGunAttackScpMessage, '%', AttackerDamage);
                switch (Target.Role)
                {
                    case RoleType.Scp049:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp049AttackedMessage, '%', AttackerDamage);
                        ShowHintDisplay(Target, Message);
                        ShowHintDisplay(Shot.Shooter, Message2);
                        break;
                    case RoleType.Scp0492:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp0492AttackedMessage, '%', AttackerDamage);
                        ShowHintDisplay(Target, Message);
                        ShowHintDisplay(Shot.Shooter, Message2);
                        break;
                    case RoleType.Scp096:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp096AttackedMessage, '%', AttackerDamage);
                        ShowHintDisplay(Target, Message);
                        ShowHintDisplay(Shot.Shooter, Message2);
                        break;
                    case RoleType.Scp106:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp106AttackedMessage, '%', AttackerDamage);
                        ShowHintDisplay(Target, Message);
                        ShowHintDisplay(Shot.Shooter, Message2);
                        break;
                    case RoleType.Scp173:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp173AttackedMessage, '%', AttackerDamage);
                        ShowHintDisplay(Target, Message);
                        ShowHintDisplay(Shot.Shooter, Message2);
                        break;
                    case RoleType.Scp93953:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp93953AttackedMessage, '%', AttackerDamage);
                        ShowHintDisplay(Target, Message);
                        ShowHintDisplay(Shot.Shooter, Message2);
                        break;
                    case RoleType.Scp93989:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp93989AttackedMessage, '%', AttackerDamage);
                        ShowHintDisplay(Target, Message);
                        ShowHintDisplay(Shot.Shooter, Message2);
                        break;
                }
            }
            else if (Shot.Shooter.IsHuman && Target.IsHuman)
            {
                string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.HumanGunAttackMessage, '%', TargetHitboxDamage);
                string Message2 = TokenReplacer.ReplaceAfterToken(Plugin.Config.BulletDamageMessage, '%', AttackerHitboxDamage);
                ShowHintDisplay(Shot.Shooter, Message);
                ShowHintDisplay(Target, Message2);
            }
        }

        public void ProcessScpDamageHint(HurtingEventArgs Hurt)
        {
            int Chance = NumGen.Next(0, 100);
            if (Chance > Plugin.Config.DamageMessageChance || !Plugin.Config.ScpsReceiveMessage)
                return;

            string Message;
            PlayerTargetData.Item2 = Hurt.Target.Nickname;
            DamageData.Item2 = Math.Round(Hurt.Amount);
            Target.AddTwoTupleSO(PlayerTargetData);
            Damage.AddTwoTupleSO(DamageData);
            TargetDamage.AddTwoTupleSO(PlayerTargetData, DamageData);

                switch (Hurt.Attacker.Role)
                {
                    case RoleType.Scp049:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp049AttackMessage, '%', Target);
                        ShowHintDisplay(Hurt.Attacker, Message);
                        break;
                    case RoleType.Scp0492:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp0492AttackMessage, '%', TargetDamage);
                        ShowHintDisplay(Hurt.Attacker, Message);
                        break;
                    case RoleType.Scp096:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp096AttackMessage, '%', Target);
                        ShowHintDisplay(Hurt.Attacker, Message);
                        break;
                    case RoleType.Scp106:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp106AttackMessage, '%', TargetDamage);
                        ShowHintDisplay(Hurt.Attacker, Message);
                        break;
                    case RoleType.Scp173:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp173AttackMessage, '%', Target);
                        ShowHintDisplay(Hurt.Attacker, Message);
                        break;
                    case RoleType.Scp93953:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp93953AttackMessage, '%', TargetDamage);
                        ShowHintDisplay(Hurt.Attacker, Message);
                        break;
                    case RoleType.Scp93989:
                        Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp93989AttackMessage, '%', TargetDamage);
                        ShowHintDisplay(Hurt.Attacker, Message);
                        break;
                }
        }

        public void ShowHintDisplay(Player Ply, string Message)
        {
            if (Ply.HintDisplay == null || !CheckForDisplayToggle(Ply))
                return;

            Hint.Text = "\n\n\n\n\n\n\n\n" + Message;

            Ply.HintDisplay.Show(Hint);
        }

        public bool ItemInHandIsKeycardForPlayer(Player Ply)
        {
            ItemCat = Ply.Inventory.GetItemByID(Ply.Inventory.curItem)?.itemCategory ?? ItemCategory.None;
            return ItemCat == ItemCategory.Keycard;
        }

        public bool CheckForDisplayToggle(Player Ply)
        {
            try
            {
                if (!Players.TryGetValue(Ply.UserId, out bool value))
                    return false;
                return value;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
