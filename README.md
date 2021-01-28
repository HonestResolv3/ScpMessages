# ScpMessages
An SCP:SL Exiled plugin that plays messages with events that occur with the game

### Features
```
- Messages play for: 
  - Humans
  - SCPs
  
- Events messages play for: 
  - Door interactions
  - Damage dealth (through guns, grenades, teslas, etc.)
  - Medical items used
  
- Customizable Options:
  - Message playing chances
  - Messages themselves (Enabled/Disabled, Text shown, etc.)
  - Who receives messages
```

### Notes For Message Use
```
- %player
  - Who attacked you in damage_message variables
  - Who you attacked in attack_message variables
  
- %damage
  - How much damage you took in damage_messge variables
  - How much damage you dealt in attack_message variables

- %hitbox
  - The hitbox name of the area you shot in the human_gun_attack_message variable
  
- %health
  - HP you gained in heal_message variables
  
- %adrhealth
  - AHP you gain in the adrenaline_heal_message variable
```

### Config
```yaml
scp_messages:
  is_enabled: true
  damage_message_enabled: true
  door_message_enabled: true
  medical_item_message_enabled: true
  humans_receive_message: true
  scps_receive_message: true
  damage_message_chance: 100
  door_message_chance: 100
  medical_item_message_chance: 100
  fall_damage_message: You fell down and took %damage damage
  bullet_damage_message: You got shot by %player and took %damage damage
  tesla_damage_message: You got zapped by a tesla and took %damage damage
  grenade_damage_message: You got hit by %player's frag grenade and took %damage damage
  micro_hid_damage_message: You got zapped by %player and took %damage
  human_gun_attack_message: You shot %player in the %hitbox and dealt %damage damage
  human_grenade_attack_message: You hit %player with a frag grenade and dealt %damage damage
  human_micro_hid_attack_message: You zapped %player dealing %damage
  scp049_attack_message: You tapped %player killing them instantly, revive them as a zombie!
  scp0492_attack_message: You attacked %player dealing %damage damage
  scp096_attack_message: You ripped %player apart with your hands killing them instantly
  scp106_attack_message: You brought %player to your pocket dimension also dealing %damage damage
  scp173_attack_message: You snapped %player killing them instantly
  scp939_attack_message: You bit %player which wounded them dealing %damage damage
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