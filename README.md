# Pokemon-G-Version
General info:
Most of the characters sprites are based on characters from main games and come from this artis's site:
https://www.deviantart.com/aveontrainer/gallery/67900303/overworld

Soundtrack is a compilation of music scores from various other games, here is the full list:
- main menu: https://www.youtube.com/watch?v=fkHYtV7RFMQ&ab_channel=EsportsMusic-poweredbyLeaguepedia
- first scene (laboratory): https://www.youtube.com/watch?v=0tIB6iOprJE&list=PL-KJ_3GwS8EvyVuBDG41hZM3fg4ANjIEp&index=36&t=303s&ab_channel=xXSilentAgent47Xx
- second scene (main town): https://www.youtube.com/watch?v=eI7kh4GBPPg&list=PL-KJ_3GwS8EvyVuBDG41hZM3fg4ANjIEp&index=19&ab_channel=SlayerSantiago
- third scene (first floor): https://www.youtube.com/watch?v=oQjmY7aAIhs&list=PL-KJ_3GwS8EvyVuBDG41hZM3fg4ANjIEp&index=35&ab_channel=xXSilentAgent47Xx
- Trevor's battle: https://www.youtube.com/watch?v=kpA4Izw2UL0&list=PL-KJ_3GwS8EvyVuBDG41hZM3fg4ANjIEp&index=37&ab_channel=xXSilentAgent47Xx
- fourth scene (second floor): https://www.youtube.com/watch?v=0GqzHWiq_Bs&list=PL-KJ_3GwS8EvyVuBDG41hZM3fg4ANjIEp&index=33&ab_channel=xXSilentAgent47Xx
- Tanisha's battle: https://www.youtube.com/watch?v=vGbs5bWig5E&list=PL-KJ_3GwS8EvyVuBDG41hZM3fg4ANjIEp&index=34&ab_channel=xXSilentAgent47Xx
- fifth scene (bedroom): https://www.youtube.com/watch?v=xvqnyfSo6PU&t=362s&ab_channel=Anhtique
- mother's battle: https://www.youtube.com/watch?v=O8R2q_2XPkc&list=PL-KJ_3GwS8EvyVuBDG41hZM3fg4ANjIEp&index=17&ab_channel=SuperRayman001
- sixth scene (wardrobe): https://www.youtube.com/watch?v=SMdFj47cszM&list=PL-KJ_3GwS8EvyVuBDG41hZM3fg4ANjIEp&index=9&ab_channel=RuslanMart
- Jaynus's battle: https://www.youtube.com/watch?v=EOI-LKc_Kz8&list=PL-KJ_3GwS8EvyVuBDG41hZM3fg4ANjIEp&index=18&ab_channel=%E0%BC%84%E0%BD%91%F0%9D%95%AF%F0%9D%96%86%F0%9D%96%8E%F0%9D%96%93%F0%9D%96%98%F0%9D%96%91%F0%9D%96%8A%F0%9D%96%8E%F0%9D%96%8B%E0%BD%8C%E0%BF%90
Tilesets were taken from google graphic, they don't come from one person. 


Initial balance patch (for fans of original series games)
New type chart: bug is super effective against fairy, grass resists fairy, poison is super effective against water, ice resists ground and flying (because.....reasons)
/*                           NOR  FIR   WAT   GRS   ELE   BUG   POI   GRO   FLY   PSY   DAR   GHO   FIG   ROC   ICE   STE   DRA   FAI */
        /*NOR*/ new float[] { 1f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0f,   1f,   0.5f, 1f,   0.5f, 1f,   1f   },
        /*FIR*/ new float[] { 1f, 0.5f, 0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 2f,   2f,   0.5f, 1f   },
        /*WAT*/ new float[] { 1f, 2f,   0.5f, 0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 1f   },
        /*GRS*/ new float[] { 1f, 0.5f, 2f,   0.5f, 1f,   0.5f, 0.5f, 2f,   0.5f, 1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 0.5f, 1f   },
        /*ELE*/ new float[] { 1f, 1f,   2f,   0.5f, 0.5f, 1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f   },
        /*BUG*/ new float[] { 1f, 0.5f, 1f,   2f,   1f,   1f,   0.5f, 1f,   0.5f, 2f,   2f,   0.5f, 0.5f, 1f,   1f,   0.5f, 1f,   2f   },
        /*POI*/ new float[] { 1f, 1f,   2f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 1f,   0.5f, 1f,   0f,   1f,   2f   },
        /*GRO*/ new float[] { 1f, 2f,   1f,   0.5f, 2f,   0.5f, 2f,   1f,   0f,   1f,   1f,   1f,   1f,   2f,   0.5f, 2f,   1f,   1f   },
        /*FLY*/ new float[] { 1f, 1f,   1f,   2f,   0.5f, 2f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 0.5f, 0.5f, 1f,   1f   },
        /*PSY*/ new float[] { 1f, 1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 0f,   1f,   2f,   1f,   1f,   0.5f, 1f,   1f   },
        /*DAR*/ new float[] { 1f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 2f,   0.5f, 1f,   1f,   1f,   1f,   0.5f },
        /*GHO*/ new float[] { 0f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 2f,   1f,   1f,   1f,   1f,   1f,   1f   },
        /*FIG*/ new float[] { 2f, 1f,   1f,   1f,   1f,   0.5f, 0.5f, 1f,   0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f,   2f,   1f,   0.5f },
        /*ROC*/ new float[] { 1f, 2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 2f,   1f,   1f,   1f,   0.5f, 1f,   2f,   0.5f, 1f,   1f   },
        /*ICE*/ new float[] { 1f, 0.5f, 0.5f, 2f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0.5f, 2f,   1f   },
        /*STE*/ new float[] { 1f, 0.5f, 0.5f, 1f,   0.5f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   0.5f, 1f,   2f   },
        /*DRA*/ new float[] { 1f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 2f,   0f   },
        /*FAI*/ new float[] { 1f, 0.5f, 1f,   0.5f, 1f,   1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   2f,   1f,   1f,   0.5f, 2f,   1f   }

Pokemon with changed types:
- Gyarados: Wat/Fly -> Wat/Dar
- Florges: Fai -> Fai/Grs
- Psudyck, Golduck: Wat -> Wat/Psy
- Eelektros: Ele -> Ele/Psn
- Lugia: Psy/Fly -> Wat/Fly
- Cofagrigus: Gho -> Gho/Roc

Reworked moves:
- Haze: is now a special dark type move with 60 base power and 100 accuracy which lowers accuracy
- Sandstorm: is now a special ground type move with 110 base power and 80 accuracy with a chance to lower accuracy
- Wild charge: no longers causes recoil damage
- Frost breath: no longer does damage, now applies the freeze status on demand
- Solarbeam: power changed to 80 from 120, no longer needs a turn to charge, now has a slight chance to cause a burn
- Giga Impact: power changed to 95, no longer needs to recharge on next turn
- Phantom force: no longer takes 2 turns to deal damage
- Dig: no longer takes 2 turns to deal damage
- Tri Attack: now only causes the freeze effect
- Venoshock: power changed to 120, accuracy to 85, can cause poisoning, no longer deals double damage against poisoned targets
- Outrage: power changed to 110, accuracy lowered to 90, no longer locks the user into using only this move, can now cause self confusion
- Dynamic punch: accuracy buffed to 100, power lowered to 90, confusion chance lowered to 50

Plus a bunch of other buffs to pokemon base stats and move power
