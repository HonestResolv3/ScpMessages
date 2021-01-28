using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Hints;
using Interactables.Interobjects.DoorUtils;
using System;

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

        DoorLockMode mode;

        ItemCategory ItemCat;

        bool ItemInHandIsKeycard;

        public EventHandlers(ScpMessages Plugin)
        {
            this.Plugin = Plugin;
        }

        public void OnDamage(HurtingEventArgs Hurt)
        {
            if (!Plugin.Config.DamageMessageEnabled)
                return;

            int Chance = NumGen.Next(0, 100);
            if (Chance > Plugin.Config.DamageMessageChance)
                return;

            if (Hurt.Target.IsHuman && Plugin.Config.HumansReceiveMessage)
            {
                ProcessDamageHint(Hurt);
            }

            else if (Hurt.Target.IsScp && Plugin.Config.ScpsReceiveMessage)
            {
                ProcessDamageHint(Hurt);
            }

            if (Hurt.Attacker.IsScp && Plugin.Config.ScpsReceiveMessage)
            {
                ProcessScpDamageHint(Hurt);
            }
        }

        public void OnShoot(ShotEventArgs Shoot)
        {
            if (!Plugin.Config.DamageMessageEnabled || !Plugin.Config.HumansReceiveMessage)
                return;

            ProcessHumanShotHint(Shoot);
        }

        public void OnDoorInteract(InteractingDoorEventArgs Door)
        {
            if (!Plugin.Config.DoorMessageEnabled || !Door.Door.TryGetComponent(out DoorNametagExtension _) ||
                (Door.Player.IsHuman && !Plugin.Config.HumansReceiveMessage) || Door.Player.IsScp)
                return;

            int Chance = NumGen.Next(0, 100);
            if (Chance > Plugin.Config.DoorMessageChance)
                return;

            ItemInHandIsKeycard = ItemInHandIsKeycardForPlayer(Door.Player);
            if (Door.Player.IsBypassModeEnabled)
            {
                if (ItemInHandIsKeycard)
                    ShowHintDisplay(Door.Player, Plugin.Config.BypassDoorKeycardMessage);
                else
                    ShowHintDisplay(Door.Player, Plugin.Config.BypassDoorMessage);
                return;
            }

            mode = DoorLockUtils.GetMode((DoorLockReason)Door.Door.ActiveLocks);
            if (mode == DoorLockMode.FullLock)
            {
                ShowHintDisplay(Door.Player, Plugin.Config.FullLockdownMessage);
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
            {
                ShowHintDisplay(Door.Player, Plugin.Config.UnlockedDoorKeycardMessage);
            }
        }

        public void OnMedicalItemUse(UsedMedicalItemEventArgs Med)
        {
            if (!Plugin.Config.MedicalItemMessageEnabled || !Plugin.Config.HumansReceiveMessage)
                return;

            int Chance = NumGen.Next(0, 100);
            if (Chance > Plugin.Config.MedicalItemMessageChance)
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

        public void ProcessDamageHint(HurtingEventArgs Hurt)
        {
            DamageData.Item2 = Math.Round(Hurt.Amount);
            PlayerAttackerData.Item2 = Hurt.Attacker;
            PlayerTargetData.Item2 = Hurt.Target;

            Damage.AddTwoTupleSO(DamageData);
            AttackerDamage.AddTwoTupleSO(PlayerAttackerData, DamageData);
            TargetDamage.AddTwoTupleSO(PlayerTargetData, DamageData);

            try
            {
                if (Hurt.DamageType == DamageTypes.Falldown)
                {
                    string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.FallDamageMessage, '%', Damage);
                    ShowHintDisplay(Hurt.Target, Message);
                }
                else if (Hurt.DamageType.isWeapon)
                {

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
                    string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp049DamageMessage, '%', Damage);
                    ShowHintDisplay(Hurt.Target, Message);
                }
                else if (Hurt.DamageType == DamageTypes.Scp0492)
                {
                    string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp0492DamageMessage, '%', AttackerDamage);
                    ShowHintDisplay(Hurt.Target, Message);
                }
                else if (Hurt.DamageType == DamageTypes.Scp096)
                {
                    string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp096DamageMessage, '%', Damage);
                    ShowHintDisplay(Hurt.Target, Message);
                }
                else if (Hurt.DamageType == DamageTypes.Scp106)
                {
                    string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp106DamageMessage, '%', AttackerDamage);
                    ShowHintDisplay(Hurt.Target, Message);
                }
                else if (Hurt.DamageType == DamageTypes.Scp173)
                {
                    string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp173DamageMessage, '%', Damage);
                    ShowHintDisplay(Hurt.Target, Message);
                }
                else if (Hurt.DamageType == DamageTypes.Scp939)
                {
                    string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp939DamageMessage, '%', AttackerDamage);
                    ShowHintDisplay(Hurt.Target, Message);
                }
            }
            catch (Exception)
            {
                // When you show a new hint to an NPC, it ends up being a NullReferenceException
                // Though, their HintDisplay property is not null. It occurs with the .Show() method
            }
        }

        public void ProcessHumanShotHint(ShotEventArgs Shot)
        {
            if (Shot.Target == null)
                return;

            Player Target = new Player(Shot.Target);
            DamageData.Item2 = Math.Round(Shot.Damage);
            PlayerTargetData.Item2 = Target.Nickname;
            PlayerAttackerData.Item2 = Shot.Shooter.Nickname;
            HitboxData.Item2 = Shot.HitboxTypeEnum.ToString().ToLowerInvariant();
            TargetHitboxDamage.AddTwoTupleSO(PlayerTargetData, HitboxData, DamageData);
            AttackerHitboxDamage.AddTwoTupleSO(PlayerAttackerData, HitboxData, DamageData);
            string Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.HumanGunAttackMessage, '%', TargetHitboxDamage);
            string Message2 = TokenReplacer.ReplaceAfterToken(Plugin.Config.BulletDamageMessage, '%', AttackerDamage);
            ShowHintDisplay(Shot.Shooter, Message);
            ShowHintDisplay(Target, Message2);
        }

        public void ProcessScpDamageHint(HurtingEventArgs Hurt)
        {
            if (!Hurt.Attacker.IsScp)
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
                case RoleType.Scp93989:
                    Message = TokenReplacer.ReplaceAfterToken(Plugin.Config.Scp939AttackMessage, '%', TargetDamage);
                    ShowHintDisplay(Hurt.Attacker, Message);
                    break;
            }
        }

        public void ShowHintDisplay(Player Ply, string Message)
        {
            if (Ply.HintDisplay == null)
                return;

            Hint.Text = "\n\n\n\n\n\n\n\n" + Message;
            Ply.HintDisplay.Show(Hint);
        }

        public bool ItemInHandIsKeycardForPlayer(Player Ply)
        {
            ItemCat = Ply.Inventory.GetItemByID(Ply.Inventory.curItem)?.itemCategory ?? ItemCategory.None;
            return ItemCat == ItemCategory.Keycard;
        }
    }
}
