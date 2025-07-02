using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Polarities.Core;
using Polarities.Content.Buffs.Hardmode;

namespace Polarities.Content.Items.Placeable.Blocks.Fractal
{
    public class DendriticEnergy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Type] = 58;
            ItemID.Sets.ItemNoGravity[Type] = true;
            ItemID.Sets.ItemIconPulse[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<DendriticEnergyTile>();
            Item.rare = ItemRarityID.Pink;
            Item.width = 50;
            Item.height = 42;
            Item.value = 3000;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.75f * Main.essScale);
        }
    }

    [LegacyName("DendriticEnergy")]
    public class DendriticEnergyTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileOreFinderPriority[Type] = 705;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 975;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(176, 239, 232), CreateMapEntryName());

            DustType = 84;
            
            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 180;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void FloorVisuals(Player player)
        {
            Vector2 oldCenter = player.Center;
            for (int i = 0; i < 256; i++)
            {
                Vector2 newPosition = player.position + Main.rand.NextVector2Circular(400, 400);
                if (ModUtils.IsPositionPlayerSafe(newPosition))
                {
                    player.Teleport(newPosition, TeleportationStyleID.DebugTeleport);
                    for (int j = 0; j < 20; j++)
                    {
                        Vector2 dustPos = Vector2.Lerp(oldCenter, player.Center, j / 20f);
                        Dust.NewDustPerfect(dustPos, DustID.Electric, Velocity: Vector2.Zero).noGravity = true;
                    }
                    break;
                }
            }
            base.FloorVisuals(player);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Player player = Main.player?[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];

            if (player == null)
            {
                noItem = true;
            }
            else if (FractalSubworld.Active && player.buffTime[player.FindBuffIndex(ModContent.BuffType<Fractalizing>())] < FractalSubworld.HARDMODE_DANGER_TIME)
            {
                fail = true;
            }
        }
    }
}
