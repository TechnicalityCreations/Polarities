using Polarities.Content.Projectiles;
using Polarities.Global;
using Polarities.Core;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Accessories.Flawless
{
	public class BrokenWarpDrive : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = (1);
			PolaritiesItem.IsFlawless.Add(Type);
		}

		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 29;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 10);

			Item.rare = RarityType<MartianSaucerFlawlessRarity>();
		}

		private static bool ContainsID(Item[] arr, int id)
        {
			foreach (Item i in arr)
            {
				if (i.type == id) return true;
            }
			return false;
        }

		public override void Load()
		{
			On_TeleportPylonsSystem.HandleTeleportRequest += BypassTeleport;
		}

		public static void BypassTeleport(On_TeleportPylonsSystem.orig_HandleTeleportRequest orig, TeleportPylonsSystem sys, TeleportPylonInfo info, int playerIndex)
		{
			Player player = Main.player[playerIndex];
			if (ContainsID(player.inventory, ItemType<BrokenWarpDrive>()) || ContainsID(player.armor, ItemType<BrokenWarpDrive>()))
			{
				Vector2 position = info.PositionInTiles.ToWorldCoordinates(8f, 8f) - new Vector2(0f, (float)player.HeightOffsetBoost);
				player.Teleport(position, TeleportationStyleID.TeleportationPylon, (int)info.TypeOfPylon);
				player.velocity = Vector2.Zero;

				// spawn dust line above player
    				for (int i = 0; i < 100; i++)
				{
    					Vector2 dustPos = Vector2.Lerp(player.Center + new Vector2(0, -player.height / 2), player.Center + new Vector2(0, (player.height + Main.screenHeight) / -2), i / 100f);
	 				Dust.NewDustPerfect(dustPos, DustID.Electric).velocity = Vector2.Zero;
    				}
			}
   			else orig(sys, info, playerIndex);
		}
	}
}
