﻿using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Biomes.Fractal
{   public class FractalBiome : ModBiome
    {
        public override bool IsBiomeActive(Player player)
        {
            return FractalSubworld.Active;
        }

        public override float GetWeight(Player player)
        {
            return 0.6f;
        }

        public override string MapBackground => "Polarities/Content/Biomes/Fractal/FractalMapBackground";
        public override string BackgroundPath => MapBackground;
        public override string BestiaryIcon => "Polarities/Content/Biomes/Fractal/FractalBestiaryIcon";

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/FractalPalace");

        public override ModWaterStyle WaterStyle => GetInstance<FractalWaterStyle>();
        public override int BiomeTorchItemType => ItemType<Items.Placeable.Furniture.Fractal.FractalTorch>();

        public override void Load()
        {
            if (!Main.dedServ)
            {
                SkyManager.Instance["Polarities:FractalDimension"] = new FractalSubworldSky();
                Filters.Scene["Polarities:FractalDimension"] = new Filter(new FractalScreenShaderData("FilterFractal"), EffectPriority.VeryLow);
                Terraria.On_Main.DrawBlack += Main_DrawBlack;
            }
        }

        private static void Main_DrawBlack(Terraria.On_Main.orig_DrawBlack orig, Main self, bool force)
        {
            if (FractalSubworld.Active)
            {
                force = true;
            }
            orig(self, force);
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("Polarities:FractalDimension", isActive);
        }
    }
}