using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Polarities.Core;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Armor.Ranged.Hardmode.Pieces
{
    [AutoloadEquip(EquipType.Head)]
    public class ShroomiteDartHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.AaronsHelmet);
            Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 5);
        }
    }

    public partial class TruffleShroomiteSell : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            if (npc.type == NPCID.Truffle &&
                (ModUtils.Contains(Main.LocalPlayer.inventory, ItemID.ShroomiteHeadgear)
              || ModUtils.Contains(Main.LocalPlayer.inventory, ItemID.ShroomiteMask)
              || ModUtils.Contains(Main.LocalPlayer.inventory, ItemID.ShroomiteHelmet)
              || ModUtils.Contains(Main.LocalPlayer.armor, ItemID.ShroomiteHeadgear)
              || ModUtils.Contains(Main.LocalPlayer.armor, ItemID.ShroomiteMask)
              || ModUtils.Contains(Main.LocalPlayer.armor, ItemID.ShroomiteHelmet)
				)
			)
            {
                int firstFreeIndex = items.Length - 1;
                for (int i = items.Length - 1; i > -1; i--)
                {
                    if (items[i] != null)
                    {
                        firstFreeIndex = i + 1;
                        break;
                    }
                }
                if (firstFreeIndex < items.Length) items[firstFreeIndex] = new Item(ItemType<ShroomiteDartHelmet>());
            }
        }
    }
}

