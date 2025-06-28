using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Polarities.Core;

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
            Item.CloneDefaults(ItemID.ShroomiteHelmet);
            Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        }
    }
}

