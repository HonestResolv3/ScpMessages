using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace ScpMessages
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool EnableDebugStartupMessage { get; set; } = true;
        public bool EnableToggleMessageOnJoin { get; set; } = true;
        public bool DamageMessageEnabled { get; set; } = true;
        public bool DoorMessageEnabled { get; set; } = true;
        public bool MedicalItemMessageEnabled { get; set; } = true;
        public bool HumansReceiveMessage { get; set; } = true;
        public bool ScpsReceiveMessage { get; set; } = true;
        public uint DamageMessageChance { get; set; } = 100;
        public uint DoorMessageChance { get; set; } = 100;
        public uint MedicalItemMessageChance { get; set; } = 100;
        public Dictionary<string, string> HitboxTranslations { get; private set; } = new Dictionary<string, string>()
        {
            { "HEAD", "head" },
            { "ARM", "arm" },
            { "BODY", "body" },
            { "LEG", "leg" }
        };
        public string FallDamageMessage { get; set; } = "You fell down and took %damage damage";
        public string BulletDamageMessage { get; set; } = "You got shot by %player in the %hitbox and took %damage damage";
        public string TeslaDamageMessage { get; set; } = "You got zapped by a tesla and took %damage damage";
        public string GrenadeDamageMessage { get; set; } = "You got hit by %player's frag grenade and took %damage damage";
        public string MicroHidDamageMessage { get; set; } = "You got zapped by %player and took %damage damage";
        public string HumanGunAttackMessage { get; set; } = "You shot %player in the %hitbox and dealt %damage damage";
        public string HumanGunAttackScpMessage { get; set; } = "You shot %player and dealt %damage damage";
        public string HumanGrenadeAttackMessage { get; set; } = "You hit %player with a frag grenade and dealt %damage damage";
        public string HumanMicroHidAttackMessage { get; set; } = "You zapped %player dealing %damage damage";
        public string Scp049AttackMessage { get; set; } = "You tapped %player killing them instantly, revive them as a zombie!";
        public string Scp0492AttackMessage { get; set; } = "You attacked %player dealing %damage damage";
        public string Scp096AttackMessage { get; set; } = "You ripped %player apart with your hands killing them instantly";
        public string Scp106AttackMessage { get; set; } = "You brought %player to your pocket dimension also dealing %damage damage";
        public string Scp173AttackMessage { get; set; } = "You snapped %player killing them instantly";
        public string Scp93953AttackMessage { get; set; } = "You bit %player which wounded them dealing %damage damage";
        public string Scp93989AttackMessage { get; set; } = "You bit %player which wounded them dealing %damage damage";
        public string Scp049AttackedMessage { get; set; } = "You were hit by %player and took %damage damage";
        public string Scp0492AttackedMessage { get; set; } = "You were hit by %player and took %damage damage";
        public string Scp096AttackedMessage { get; set; } = "You were hit by %player and took %damage damage";
        public string Scp106AttackedMessage { get; set; } = "You were hit by %player and took %damage damage";
        public string Scp173AttackedMessage { get; set; } = "You were hit by %player and took %damage damage";
        public string Scp93953AttackedMessage { get; set; } = "You were hit by %player and took %damage damage";
        public string Scp93989AttackedMessage { get; set; } = "You were hit by %player and took %damage damage";
        public string Scp049DamageMessage { get; set; } = "You got tapped by %player and died instantly";
        public string Scp0492DamageMessage { get; set; } = "You got attacked by %player and took %damage damage";
        public string Scp096DamageMessage { get; set; } = "You got ripped apart by %player and died instantly";
        public string Scp106DamageMessage { get; set; } = "You got attacked by %player and took %damage damage";
        public string Scp173DamageMessage { get; set; } = "You had your neck snapped by %player and died instantly";
        public string Scp939DamageMessage { get; set; } = "You got bit by %player and took %damage damage";
        public string LockedDoorMessage { get; set; } = "You need a keycard to open this area";
        public string LockedDoorKeycardMessage { get; set; } = "You need a better level keycard to open this area";
        public string FullLockdownMessage { get; set; } = "This area is completely locked down";
        public string UnlockedDoorKeycardMessage { get; set; } = "You held the keycard next to the reader";
        public string BypassDoorMessage { get; set; } = "You bypassed the reader";
        public string BypassDoorKeycardMessage { get; set; } = "You bypassed the reader, but did not need a keycard";
        public string PainkillerHealMessage { get; set; } = "You took painkillers which gave you %health health and temporary HP regeneration";
        public string MedkitHealMessage { get; set; } = "You used a medkit and gained %health HP";
        public string AdrenalineHealMessage { get; set; } = "You injected some adrenaline which gave you %adrhealth AHP and temporary HP regeneration";
        public string Scp500HealMessage { get; set; } = "You took SCP-500 which fully healed you and gave temporary health regeneration";
        public string Scp207HealMessage { get; set; } = "You drank some SCP-207 which gave you %health HP, a speed boost, and infinite stamina. Watch your HP closely!";
    }
}
