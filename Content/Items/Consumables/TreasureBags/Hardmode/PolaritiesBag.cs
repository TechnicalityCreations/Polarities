﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.Events;
using Polarities.Content.Items.Accessories.ExpertMode.Hardmode;
using Polarities.Content.Items.Vanity.Hardmode;
using Polarities.Content.Items.Consumables.TreasureBags.Hardmode;
using Polarities.Content.Items.Tools.Hooks.Hardmode;
using Polarities.Content.Items.Materials.Hardmode;
using Polarities.Content.Items.Placeable.Relics;
using Polarities.Content.Items.Placeable.Trophies;
using Polarities.Content.Items.Weapons.Ranged.Flawless;
using Polarities.Content.Items.Weapons.Summon.Minions.Hardmode;
using Polarities.Content.NPCs.Bosses.Hardmode.Esophage;
using Polarities.Content.Items.Tools.Misc.Hardmode;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Consumables.TreasureBags.Hardmode
{
    public class PolaritiesBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("{$Mods.Polarities.ItemName.TreasureBag} ({$Mods.Polarities.NPCName.Esophage})");
            // Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");

            ItemID.Sets.BossBag[Type] = true;

            Item.ResearchUnlockCount = (3);
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ItemType<Accessories.Wings.ElectricWings>(), 15));
            itemLoot.Add(ItemDropRule.Common(ItemType<Weapons.Melee.Misc.EMP>(), 3));
            itemLoot.Add(ItemDropRule.Common(ItemType<Weapons.Ranged.Bows.Hardmode.ArcBolter>(), 3));
            itemLoot.Add(ItemDropRule.Common(ItemType<Weapons.Magic.Staffs.Hardmode.DiamagneticDischarge>(), 3));
            itemLoot.Add(ItemDropRule.Common(ItemType<Weapons.Magic.Books.Hardmode.MagneticStreams>(), 3));
            itemLoot.Add(ItemDropRule.Common(ItemType<Weapons.Summon.Minions.Hardmode.DipoleStaff>(), 3));
            itemLoot.Add(ItemDropRule.Common(ItemType<Materials.Hardmode.SmiteSoul>(), 1, 25, 40));
            itemLoot.Add(ItemDropRule.Common(ItemType<Items.Placeable.Bars.PolarizedBar>(), 1, 20, 35));
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Type].Value;
            Rectangle frame = texture.Frame();

            Vector2 vector = frame.Size() / 2f;
            Vector2 value = new Vector2(Item.width / 2 - vector.X, Item.height - frame.Height);
            Vector2 vector2 = Item.position - Main.screenPosition + vector + value;

            float num = Item.velocity.X * 0.2f;

            float num7 = Item.timeSinceItemSpawned / 240f + Main.GlobalTimeWrappedHourly * 0.04f;
            float globalTimeWrappedHourly = Main.GlobalTimeWrappedHourly;
            globalTimeWrappedHourly %= 4f;
            globalTimeWrappedHourly /= 2f;
            if (globalTimeWrappedHourly >= 1f)
            {
                globalTimeWrappedHourly = 2f - globalTimeWrappedHourly;
            }
            globalTimeWrappedHourly = globalTimeWrappedHourly * 0.5f + 0.5f;
            for (float num8 = 0f; num8 < 1f; num8 += 0.25f)
            {
                spriteBatch.Draw(texture, vector2 + Utils.RotatedBy(new Vector2(0f, 8f), (num8 + num7) * ((float)Math.PI * 2f)) * globalTimeWrappedHourly, frame, new Color(90, 70, 255, 50), num, vector, scale, 0, 0f);
            }
            for (float num9 = 0f; num9 < 1f; num9 += 0.34f)
            {
                spriteBatch.Draw(texture, vector2 + Utils.RotatedBy(new Vector2(0f, 4f), (num9 + num7) * ((float)Math.PI * 2f)) * globalTimeWrappedHourly, frame, new Color(140, 120, 255, 77), num, vector, scale, 0, 0f);
            }
            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight((int)((Item.position.X + Item.width) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 0.4f, 0.4f, 0.4f);
            if (Item.timeSinceItemSpawned % 12 == 0)
            {
                Dust dust2 = Dust.NewDustPerfect(Item.Center + new Vector2(0f, Item.height * -0.1f) + Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f) * (0.3f + Main.rand.NextFloat() * 0.5f), 279, (Vector2?)new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.3f - 1.5f), 127, default(Color), 1f);
                dust2.scale = 0.5f;
                dust2.fadeIn = 1.1f;
                dust2.noGravity = true;
                dust2.noLight = true;
                dust2.alpha = 0;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(lightColor, Color.White, 0.4f);
        }
    }
}

