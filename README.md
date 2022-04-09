# ScpMessages
An SCP:SL Exiled plugin that plays messages with events that occur with the game

# Notice
No more SCP: Secret Laboratory plugins will be created by me beyond this point, this project and all my other projects will be archived for reference only

### Features
- Messages play for: 
  - Humans
  - SCPs (Damage only)
  
- Events messages play for: 
  - Door interactions
  - Damage dealt (through guns, grenades, teslas, etc.)
  - Medical items used
  
- Customizable Options:
  - Message playing chances
  - Messages themselves (Enabled/Disabled, Text shown, etc.)
  - Who receives messages
  - Hitbox body part name translation
  - Debug and Join message options

- Convenience:
  - Users can choose if they want the messages to be played
  - The choice users make stay in following rounds

### Credits
- [RogerFK](https://github.com/RogerFK) (Code for replacing tokens (%player, etc) with other values)

### Console (Not Remote Admin) Command
```
- .scpmsg | Enables/Disables messages from being shown
```

### Notes For General Use
```
The text file where player User ID's are stored for remembering their message choice is located where your servers Configs folder is (I.e.: 7777-config.yml)

Please do not try to modify the formatting and/or content of the file or else the game will not be able to read the information properly

Also please add the provided Newtonsoft.Json.dll file into your servers Dependencies folder for this to properly work (If it is not there)
```

### Notes For Message Use
```
- %player
  - Who attacked you in damage_message, attack_message_scp, (scp)_attacked_message variables
  - Who you attacked in attack_message, attack_message_scp, (scp)_attacked_message variables
  
- %damage
  - How much damage you took in damage_messge variables
  - How much damage you dealt in attack_message variables

- %hitbox
  - The hitbox name of the area you shot in the human_gun_attack_message and bullet_damage_message variable
  
- %health
  - HP you gained in heal_message variables
  
- %adrhealth
  - AHP you gain in the adrenaline_heal_message variable
```

### Config
```yaml
scp_messages:
  is_enabled: true
  enable_debug_startup_message: true
  enable_toggle_message_on_join: true
  force_messages_enabled_on_join: true
  damage_message_enabled: true
  door_message_enabled: true
  medical_item_message_enabled: true
  humans_receive_message: true
  scps_receive_message: true
  damage_message_chance: 100
  door_message_chance: 100
  medical_item_message_chance: 100
  hitbox_translations:
    HEAD: head
    ARM: arm
    BODY: body
    LEG: leg
  fall_damage_message: You fell down and took %damage damage
  bullet_damage_message: You got shot by %player in the %hitbox and took %damage damage
  tesla_damage_message: You got zapped by a tesla and took %damage damage
  grenade_damage_message: You got hit by %player's frag grenade and took %damage damage
  micro_hid_damage_message: You got zapped by %player and took %damage
  human_gun_attack_message: You shot %player in the %hitbox and dealt %damage damage
  human_gun_attack_scp_message: You shot %player and dealt %damage damage
  human_grenade_attack_message: You hit %player with a frag grenade and dealt %damage damage
  human_micro_hid_attack_message: You zapped %player dealing %damage
  scp049_attack_message: You tapped %player killing them instantly, revive them as a zombie!
  scp0492_attack_message: You attacked %player dealing %damage damage
  scp096_attack_message: You ripped %player apart with your hands killing them instantly
  scp106_attack_message: You brought %player to your pocket dimension also dealing %damage damage
  scp173_attack_message: You snapped %player killing them instantly
  scp93953_attack_message: You bit %player which wounded them dealing %damage damage
  scp93989_attack_message: You bit %player which wounded them dealing %damage damage
  scp049_attacked_message: You were hit by %player and took %damage damage
  scp0492_attacked_message: You were hit by %player and took %damage damage
  scp096_attacked_message: You were hit by %player and took %damage damage
  scp106_attacked_message: You were hit by %player and took %damage damage
  scp173_attacked_message: You were hit by %player and took %damage damage
  scp93953_attacked_message: You were hit by %player and took %damage damage
  scp93989_attacked_message: You were hit by %player and took %damage damage
  scp049_damage_message: You got tapped by %player and died instantly
  scp0492_damage_message: You got attacked by %player and took %damage damage
  scp096_damage_message: You got ripped apart by %player and died instantly
  scp106_damage_message: You got attacked by %player and took %damage damage
  scp173_damage_message: You had your neck snapped by %player and died instantly
  scp939_damage_message: You got bit by %player and took %damage damage
  locked_door_message: You need a keycard to open this area
  locked_door_keycard_message: You need a better level keycard to open this area
  full_lockdown_message: This area is completely locked down
  unlocked_door_keycard_message: You held the keycard next to the reader
  bypass_door_message: You bypassed the reader
  bypass_door_keycard_message: You bypassed the reader, but did not need a keycard
  painkiller_heal_message: You took painkillers which gave you %health health and temporary HP regeneration
  medkit_heal_message: You used a medkit and gained %health HP
  adrenaline_heal_message: You injected some adrenaline which gave you %adrhealth AHP and temporary HP regeneration
  scp500_heal_message: You took SCP-500 which fully healed you and gave temporary health regeneration
  scp207_heal_message: You drank some SCP-207 which gave you %health HP, a speed boost, and infinite stamina. Watch your HP closely!
```
