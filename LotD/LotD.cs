using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;
using static InfinityScript.GSCFunctions;

namespace LotD
{
    public class LotD : BaseScript
    {
        public LotD()
        {
            PreCacheItem("at4_mp");
            //PreCacheItem("throwingknife_rhand_mp");
            PreCacheItem("iw5_mk12spr_mp");
            PlayerConnected += LuckyPlayerConnected;
        }
        void LuckyPlayerConnected(Entity player)
        {
            player.SpawnedPlayer += () => OnPlayerSpawned(player);
            player.OnNotify("joined_team", entity =>
            {
                player.CloseInGameMenu();
                player.Notify("menuresponse", "changeclass", "axis_recipe1");
            });
            player.SetField("Tier", 0);
            player.SetField("WeaponSet", 0);
            player.SetClientDvar("cg_objectiveText", "Get a random gun based on weapon class for every kill. First to kill with every gun wins.");
        }
        public void OnPlayerSpawned(Entity player)
        {
            player.SetClientDvar("cg_objectiveText", "Get a random gun based on weapon class for every kill. First to kill with every gun wins.");
            player.SetPerk("specialty_marathon", true, true);
            player.SetPerk("specialty_quickdraw", true, true);
            player.DisableWeaponPickup();
            if (player.GetField<int>("Tier") == 0)
            {
                if (player.GetField<int>("WeaponSet") == 0)
                {
                    string Pistol = player.CurrentWeapon;
                    string RandomPistol = Pistol + "_mp_" + RandomPistolAttach();
                    player.SetField("CurrentLuckyWeapon", RandomPistol);
                    player.TakeWeapon(Pistol);
                    player.GiveWeapon(player.GetField<string>("CurrentLuckyWeapon"));
                    player.SetField("WeaponSet", 1);
                    AfterDelay(200, () =>
                        player.SwitchToWeaponImmediate(player.GetField<string>("CurrentLuckyWeapon")));
                }
                else if (player.GetField<int>("WeaponSet") == 1)
                {
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveWeapon(player.GetField<string>("CurrentLuckyWeapon"));
                    AfterDelay(200, () =>
                        player.SwitchToWeaponImmediate(player.GetField<string>("CurrentLuckyWeapon")));
                }
            }
            if (player.GetField<int>("Tier") == 0) return;
            string CurrentLuckyWeapon = player.GetField<string>("CurrentLuckyWeapon");
            player.TakeWeapon(player.CurrentWeapon);
            player.GiveWeapon(CurrentLuckyWeapon);
            AfterDelay(200, () =>
            {
                player.SwitchToWeaponImmediate(CurrentLuckyWeapon);
                /*
                AfterDelay(800, () =>
                {
                    if (player.CurrentWeapon != CurrentLuckyWeapon || player.CurrentWeapon == null)
                        player.SwitchToWeaponImmediate(CurrentLuckyWeapon);
                });
                */
            });
        }
        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            AfterDelay(100, () =>
                {
                    if (attacker != player)
                    {
                        attacker.SetField("Tier", attacker.GetField<int>("Tier") + 1);
                        DrawLuck(attacker);
                    }
                });
        }
        public void DrawLuck(Entity player)
        {
            if (player.GetField<int>("Tier") == 0) return;
            player.TakeWeapon(player.CurrentWeapon);
            string LuckyWeapon = LuckCalc(player);
            player.SetField("CurrentLuckyWeapon", LuckyWeapon);
            player.GiveWeapon(LuckyWeapon);
            AfterDelay(200, () =>
                {
                    player.SwitchToWeaponImmediate(LuckyWeapon);
                    AfterDelay(800, () =>
                        {
                            if (player.CurrentWeapon != LuckyWeapon || player.CurrentWeapon == null)
                                player.SwitchToWeaponImmediate(LuckyWeapon);
                        });
                });
        }
        public string LuckCalc(Entity player)
        {
            if (player.GetField<int>("Tier") == 1)
            {
                int? luck = new Random().Next(4);
                switch (luck)
                {
                    case 0:
                        return "iw5_spas12_mp_" + RandomShottyAttach();
                    case 1:
                        return "iw5_striker_mp_" + RandomShottyAttach();
                    case 2:
                        return "iw5_1887_mp";
                    case 3:
                        return "iw5_ksg_mp_" + RandomShottyAttach();
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 2)
            {
                int? luck = new Random().Next(6);
                switch (luck)
                {
                    case 0:
                        return "iw5_mp5_mp_" + RandomSMGAttach();
                    case 1:
                        return "iw5_m9_mp_" + RandomSMGAttach();
                    case 2:
                        return "iw5_p90_mp_" + RandomSMGAttach();
                    case 3:
                        return "iw5_pp90m1_mp_" + RandomSMGAttach();
                    case 4:
                        return "iw5_ump45_mp_" + RandomSMGAttach();
                    case 5:
                        return "iw5_mp7_mp_" + RandomSMGAttach();
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 3)
            {
                int? luck = new Random().Next(10);
                switch (luck)
                {
                    case 0:
                        return "iw5_acr_mp_" + RandomARAttach();
                    case 1:
                        return "iw5_type95_mp_" + RandomARAttach();
                    case 2:
                        return "iw5_m4_mp_" + RandomARAttach();
                    case 3:
                        return "iw5_ak47_mp_" + RandomARAttach();
                    case 4:
                        return "iw5_m16_mp_" + RandomARAttach();
                    case 5:
                        return "iw5_mk14_mp_" + RandomARAttach();
                    case 6:
                        return "iw5_g36c_mp_" + RandomARAttach();
                    case 7:
                        return "iw5_scar_mp_" + RandomARAttach();
                    case 8:
                        return "iw5_fad_mp_" + RandomARAttach();
                    case 9:
                        return "iw5_cm901_mp_" + RandomARAttach();
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 4)
            {
                int? luck = new Random().Next(5);
                switch (luck)
                {
                    case 0:
                        return "iw5_m60_mp_" + RandomLMGAttach();
                    case 1:
                        return "iw5_mk46_mp_" + RandomLMGAttach();
                    case 2:
                        return "iw5_pecheneg_mp_" + RandomLMGAttach();
                    case 3:
                        return "iw5_sa80_mp_" + RandomLMGAttach();
                    case 4:
                        return "iw5_mg36_mp_" + RandomLMGAttach();
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 5)
            {
                int? luck = new Random().Next(14);
                switch (luck)
                {
                    case 0:
                        return "iw5_barrett_mp_barrettscope_" + RandomSniperAttach();
                    case 1:
                        return "iw5_msr_mp_msrscope_" + RandomSniperAttach();
                    case 2:
                        return "iw5_rsass_mp_rsassscope_" + RandomSniperAttach();
                    case 3:
                        return "iw5_dragunov_mp_dragunovscope_" + RandomSniperAttach();
                    case 4:
                        return "iw5_as50_mp_as50scope_" + RandomSniperAttach();
                    case 5:
                        return "iw5_l96a1_mp_l96a1scope_" + RandomSniperAttach();
                    case 6:
                        return "iw5_mk14_mp_" + RandomARAttach();
                    case 7:
                        return "iw5_barrett_mp_acog_" + RandomSniperAttach();
                    case 8:
                        return "iw5_msr_mp_acog_" + RandomSniperAttach();
                    case 9:
                        return "iw5_rsass_mp_acog_" + RandomSniperAttach();
                    case 10:
                        return "iw5_dragunov_mp_acog_" + RandomSniperAttach();
                    case 11:
                        return "iw5_as50_mp_acog_" + RandomSniperAttach();
                    case 12:
                        return "iw5_l96a1_mp_acog_" + RandomSniperAttach();
                    case 13:
                        return "iw5_mk12spr_mp_acog_heartbeat";
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 6)
            {
                int? luck = new Random().Next(6);
                switch (luck)
                {
                    case 0:
                        return "iw5_mp5_mp_" + RandomSMGAttach();
                    case 1:
                        return "iw5_m9_mp_" + RandomSMGAttach();
                    case 2:
                        return "iw5_p90_mp_" + RandomSMGAttach();
                    case 3:
                        return "iw5_pp90m1_mp_" + RandomSMGAttach();
                    case 4:
                        return "iw5_ump45_mp_" + RandomSMGAttach();
                    case 5:
                        return "iw5_mp7_mp_" + RandomSMGAttach();
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 7)
            {
                int? luck = new Random().Next(10);
                switch (luck)
                {
                    case 0:
                        return "iw5_acr_mp_" + RandomARAttach();
                    case 1:
                        return "iw5_type95_mp_" + RandomARAttach();
                    case 2:
                        return "iw5_m4_mp_" + RandomARAttach();
                    case 3:
                        return "iw5_ak47_mp_" + RandomARAttach();
                    case 4:
                        return "iw5_m16_mp_" + RandomARAttach();
                    case 5:
                        return "iw5_mk14_mp_" + RandomARAttach();
                    case 6:
                        return "iw5_g36c_mp_" + RandomARAttach();
                    case 7:
                        return "iw5_scar_mp_" + RandomARAttach();
                    case 8:
                        return "iw5_fad_mp_" + RandomARAttach();
                    case 9:
                        return "iw5_cm901_mp_" + RandomARAttach();
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 8)
            {
                int? luck = new Random().Next(6);
                switch (luck)
                {
                    case 0:
                        return "iw5_m60_mp_" + RandomLMGAttach();
                    case 1:
                        return "iw5_mk46_mp_" + RandomLMGAttach();
                    case 2:
                        return "iw5_pecheneg_mp_" + RandomLMGAttach();
                    case 3:
                        return "iw5_sa80_mp_" + RandomLMGAttach();
                    case 4:
                        return "iw5_mg36_mp_" + RandomLMGAttach();
                    case 5:
                        return "iw5_m60jugg_mp_" + RandomLMGAttach() + "_camo08";
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 9)
            {
                int? luck = new Random().Next(6);
                switch (luck)
                {
                    case 0:
                        return "rpg_mp";
                    case 1:
                        return "javelin_mp";
                    case 2:
                        return "iw5_smaw_mp";
                    case 3:
                        return "m320_mp";
                    case 4:
                        return "xm25_mp";
                    case 5:
                        return "at4_mp";
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 10)
            {
                int? luck = new Random().Next(13);
                switch (luck)
                {
                    case 0:
                        return "iw5_barrett_mp_barrettscope_" + RandomSniperAttach();
                    case 1:
                        return "iw5_msr_mp_msrscope_" + RandomSniperAttach();
                    case 2:
                        return "iw5_rsass_mp_rsassscope_" + RandomSniperAttach();
                    case 3:
                        return "iw5_dragunov_mp_dragunovscope_" + RandomSniperAttach();
                    case 4:
                        return "iw5_as50_mp_as50scope_" + RandomSniperAttach();
                    case 5:
                        return "iw5_l96a1_mp_l96a1scope_" + RandomSniperAttach();
                    case 6:
                        return "iw5_barrett_mp_acog_" + RandomSniperAttach();
                    case 7:
                        return "iw5_msr_mp_acog_" + RandomSniperAttach();
                    case 8:
                        return "iw5_rsass_mp_acog_" + RandomSniperAttach();
                    case 9:
                        return "iw5_dragunov_mp_acog_" + RandomSniperAttach();
                    case 10:
                        return "iw5_as50_mp_acog_" + RandomSniperAttach();
                    case 11:
                        return "iw5_l96a1_mp_acog_" + RandomSniperAttach();
                    case 12:
                        return "iw5_mk12spr_mp_acog_heartbeat";
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 11)
            {
                int? luck = new Random().Next(6);
                switch (luck)
                {
                    case 0:
                        return "iw5_mp5_mp_" + RandomSMGAttach();
                    case 1:
                        return "iw5_m9_mp_" + RandomSMGAttach();
                    case 2:
                        return "iw5_p90_mp_" + RandomSMGAttach();
                    case 3:
                        return "iw5_pp90m1_mp_" + RandomSMGAttach();
                    case 4:
                        return "iw5_ump45_mp_" + RandomSMGAttach();
                    case 5:
                        return "iw5_mp7_mp_" + RandomSMGAttach();
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 12)
            {
                int? luck = new Random().Next(10);
                switch (luck)
                {
                    case 0:
                        return "iw5_acr_mp_" + RandomARAttach();
                    case 1:
                        return "iw5_type95_mp_" + RandomARAttach();
                    case 2:
                        return "iw5_m4_mp_" + RandomARAttach();
                    case 3:
                        return "iw5_ak47_mp_" + RandomARAttach();
                    case 4:
                        return "iw5_m16_mp_" + RandomARAttach();
                    case 5:
                        return "iw5_mk14_mp_" + RandomARAttach();
                    case 6:
                        return "iw5_g36c_mp_" + RandomARAttach();
                    case 7:
                        return "iw5_scar_mp_" + RandomARAttach();
                    case 8:
                        return "iw5_fad_mp_" + RandomARAttach();
                    case 9:
                        return "iw5_cm901_mp_" + RandomARAttach();
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 13)
            {
                int? luck = new Random().Next(4);
                switch (luck)
                {
                    case 0:
                        return "iw5_spas12_mp_" + RandomShottyAttach();
                    case 1:
                        return "iw5_striker_mp_" + RandomShottyAttach();
                    case 2:
                        return "iw5_1887_mp";
                    case 3:
                        return "iw5_ksg_mp_" + RandomShottyAttach();
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 14)
            {
                int? luck = new Random().Next(14);
                switch (luck)
                {
                    case 0:
                        return "iw5_barrett_mp_barrettscope_" + RandomSniperAttach();
                    case 1:
                        return "iw5_msr_mp_msrscope_" + RandomSniperAttach();
                    case 2:
                        return "iw5_rsass_mp_rsassscope_" + RandomSniperAttach();
                    case 3:
                        return "iw5_dragunov_mp_dragunovscope_" + RandomSniperAttach();
                    case 4:
                        return "iw5_as50_mp_as50scope_" + RandomSniperAttach();
                    case 5:
                        return "iw5_l96a1_mp_l96a1scope_" + RandomSniperAttach();
                    case 6:
                        return "iw5_mk14_mp_" + RandomARAttach();
                    case 7:
                        return "iw5_barrett_mp_acog_" + RandomSniperAttach();
                    case 8:
                        return "iw5_msr_mp_acog_" + RandomSniperAttach();
                    case 9:
                        return "iw5_rsass_mp_acog_" + RandomSniperAttach();
                    case 10:
                        return "iw5_dragunov_mp_acog_" + RandomSniperAttach();
                    case 11:
                        return "iw5_as50_mp_acog_" + RandomSniperAttach();
                    case 12:
                        return "iw5_l96a1_mp_acog_" + RandomSniperAttach();
                    case 13:
                        return "iw5_mk12spr_mp_acog_heartbeat";
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 15)
            {
                int? luck = new Random().Next(13);
                switch (luck)
                {
                    case 0:
                        return "iw5_barrett_mp_barrettscope_" + RandomSniperAttach();
                    case 1:
                        return "iw5_msr_mp_msrscope_" + RandomSniperAttach();
                    case 2:
                        return "iw5_rsass_mp_rsassscope_" + RandomSniperAttach();
                    case 3:
                        return "iw5_dragunov_mp_dragunovscope_" + RandomSniperAttach();
                    case 4:
                        return "iw5_as50_mp_as50scope_" + RandomSniperAttach();
                    case 5:
                        return "iw5_l96a1_mp_l96a1scope_" + RandomSniperAttach();
                    case 6:
                        return "iw5_barrett_mp_acog_" + RandomSniperAttach();
                    case 7:
                        return "iw5_msr_mp_acog_" + RandomSniperAttach();
                    case 8:
                        return "iw5_rsass_mp_acog_" + RandomSniperAttach();
                    case 9:
                        return "iw5_dragunov_mp_acog_" + RandomSniperAttach();
                    case 10:
                        return "iw5_as50_mp_acog_" + RandomSniperAttach();
                    case 11:
                        return "iw5_l96a1_mp_acog_" + RandomSniperAttach();
                    case 12:
                        return "iw5_mk12spr_mp_acog_heartbeat";
                }
                return "";
            }
            else if (player.GetField<int>("Tier") == 16)
            {
                return "iw5_44magnum_mp_akimbo_xmags";
            }
            else if (player.GetField<int>("Tier") == 17 || player.GetField<int>("Tier") == 18  || player.GetField<int>("Tier") == 19)
            {
                return "throwingknife_mp";
            }
            //else if (player.GetField<int>("Tier") == 18)
                //return "throwingknife_rhand_mp";
            else return "iw5_usp45_mp";
        }
        public string RandomARAttach()
        {
            int? attach = new Random().Next(30);
            switch (attach)
            {
                case 0:
                    return "reflex";
                case 1:
                    return "acog";
                case 2:
                    return "thermal";
                case 3:
                    return "shotgun";
                case 4:
                    return "heartbeat";
                case 5:
                    return "xmags";
                case 6:
                    return "eotech";
                case 7:
                    return "silencer";
                case 8:
                    return "hybrid";
                case 9:
                    return "";
                case 10:
                    return "reflex_shotgun";
                case 11:
                    return "reflex_heartbeat";
                case 12:
                    return "reflex_xmags";
                case 13:
                    return "reflex_silencer";
                case 14:
                    return "reflex_hybrid";
                case 15:
                    return "acog_shotgun";
                case 16:
                    return "acog_heartbeat";
                case 17:
                    return "acog_xmags";
                case 18:
                    return "acog_silencer";
                case 19:
                    return "acog_hybrid";
                case 20:
                    return "thermal_shotgun";
                case 21:
                    return "thermal_heartbeat";
                case 22:
                    return "thermal_xmags";
                case 23:
                    return "thermal_silencer";
                case 24:
                    return "thermal_hybrid";
                case 25:
                    return "eotech_shotgun";
                case 26:
                    return "eotech_heartbeat";
                case 27:
                    return "eotech_xmags";
                case 28:
                    return "eotech_silencer";
                case 29:
                    return "eotech_hybrid";
            }
            return "";
        }
        public string RandomSMGAttach()
        {
            int? attach = new Random().Next(25);
            switch (attach)
            {
                case 0:
                    return "reflexsmg";
                case 1:
                    return "acog";
                case 2:
                    return "thermalsmg";
                case 3:
                    return "xmags";
                case 4:
                    return "rof";
                case 5:
                    return "eotechsmg";
                case 6:
                    return "silencer";
                case 7:
                    return "hamrhybrid";
                case 8:
                    return "";
                case 9:
                    return "reflexsmg_xmags";
                case 10:
                    return "reflexsmg_rof";
                case 11:
                    return "reflexsmg_silencer";
                case 12:
                    return "reflexsmg_hamrhybrid";
                case 13:
                    return "acog_xmags";
                case 14:
                    return "acog_rof";
                case 15:
                    return "acog_silencer";
                case 16:
                    return "acog_hamrhybrid";
                case 17:
                    return "thermalsmg_xmags";
                case 18:
                    return "thermalsmg_rof";
                case 19:
                    return "thermalsmg_silencer";
                case 20:
                    return "thermalsmg_hamrhybrid";
                case 21:
                    return "eotechsmg_xmags";
                case 22:
                    return "eotechsmg_rof";
                case 23:
                    return "eotechsmg_silencer";
                case 24:
                    return "eotechsmg_hamrhybrid";
            }
            return "";
        }
        public string RandomLMGAttach()
        {
            int? attach = new Random().Next(30);
            switch (attach)
            {
                case 0:
                    return "reflexlmg";
                case 1:
                    return "acog";
                case 2:
                    return "grip";
                case 3:
                    return "thermal";
                case 4:
                    return "heartbeat";
                case 5:
                    return "xmags";
                case 6:
                    return "eotechlmg";
                case 7:
                    return "silencer";
                case 8:
                    return "rof";
                case 9:
                    return "";
                case 10:
                    return "reflexlmg_grip";
                case 11:
                    return "reflexlmg_heartbeat";
                case 12:
                    return "reflexlmg_xmags";
                case 13:
                    return "reflexlmg_silencer";
                case 14:
                    return "reflexlmg_rof";
                case 15:
                    return "acog_grip";
                case 16:
                    return "acog_heartbeat";
                case 17:
                    return "acog_xmags";
                case 18:
                    return "acog_silencer";
                case 19:
                    return "acog_rof";
                case 20:
                    return "thermallmg_grip";
                case 21:
                    return "thermallmg_heartbeat";
                case 22:
                    return "thermallmg_xmags";
                case 23:
                    return "thermallmg_silencer";
                case 24:
                    return "thermallmg_rof";
                case 25:
                    return "eotechlmg_grip";
                case 26:
                    return "eotechlmg_heartbeat";
                case 27:
                    return "eotechlmg_xmags";
                case 28:
                    return "eotechlmg_silencer";
                case 29:
                    return "eotechlmg_rof";
            }
            return "";
        }
        public string RandomShottyAttach()
        {
            int? attach = new Random().Next(15);
            switch (attach)
            {
                case 0:
                    return "reflex";
                case 1:
                    return "grip";
                case 2:
                    return "xmags";
                case 3:
                    return "eotech";
                case 4:
                    return "silencer03";
                case 5:
                    return "";
                case 6:
                    return "reflex_grip";
                case 7:
                    return "reflex_xmags";
                case 8:
                    return "reflex_silencer03";
                case 9:
                    return "eotech_grip";
                case 10:
                    return "eotech_xmags";
                case 11:
                    return "eotech_silencer03";
                case 12:
                    return "grip_xmags";
                case 13:
                    return "grip_silencer03";
                case 14:
                    return "xmags_silencer03";
            }
            return "";
        }
        public string RandomSniperAttach()
        {
            int? attach = new Random().Next(5);
            switch (attach)
            {
                case 0:
                    return "";
                case 1:
                    return "thermal";
                case 2:
                    return "heartbeat";
                case 3:
                    return "xmags";
                case 4:
                    return "silencer03";
            }
            return "";
        }
        public string RandomPistolAttach()
        {
            int? attach = new Random().Next(10);
            switch (attach)
            {
                case 0:
                    return "akimbo";
                case 1:
                    return "xmags";
                case 2:
                    return "tactical";
                case 3:
                    return "silencer02";
                case 4:
                    return "";
                case 5:
                    return "akimbo_xmags";
                case 6:
                    return "akimbo_silencer02";
                case 7:
                    return "xmags_tactical";
                case 8:
                    return "xmags_silencer02";
                case 9:
                    return "tactical_silencer02";
            }
            return "";
        }
        public string RandomMGPistolAttach()
        {
            int? attach = new Random().Next(6);
            switch (attach)
            {
                case 0:
                    return "reflexsmg";
                case 1:
                    return "akimbo";
                case 2:
                    return "xmags";
                case 3:
                    return "eotechsmg";
                case 4:
                    return "silencer02";
                case 5:
                    return "";
                case 6:
                    return "reflexsmg_akimbo";
                case 7:
                    return "reflexsmg_xmags";
                case 8:
                    return "reflexsmg_silencer02";
                case 9:
                    return "eotechsmg_akimbo";
                case 10:
                    return "eotechsmg_xmags";
                case 11:
                    return "eotechsmg_silencer02";
                case 12:
                    return "akimbo_xnags";
                case 13:
                    return "akimbo_silencer02";
            }
            return "";
        }
    }
}
