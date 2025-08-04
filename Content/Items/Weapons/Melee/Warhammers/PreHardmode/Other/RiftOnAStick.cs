using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.Projectiles;
using System;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Melee.Warhammers.PreHardmode.Other
{
    public class RiftOnAStick : WarhammerBase
    {
        public override int HammerLength => 48;
        public override int HammerHeadSize => 16;
        public override int DefenseLoss => 20;
        public override int DebuffTime => 600;
        public override float SwingTime => 20f;
        public override float SwingTilt => 0.1f;

        public override void SetStaticDefaults()
        {
			Item.staff[Type] = true;
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.SetWeaponValues(34, 14, 0);
            Item.DamageType = DamageClass.Melee;

            Item.width = 48;
            Item.height = 48;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = WarhammerUseStyle;

            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Blue;
        }

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useStyle = 5;
				Item.shootSpeed = 1f;
				Item.shoot = ProjectileType<RiftBeam>();
				Item.noMelee = true;

				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.UseSound = SoundID.Item92;
			}
			else
			{
				Item.useStyle = WarhammerUseStyle;
				Item.shoot = 0;
				Item.noMelee = false;
				Item.noUseGraphic = false;
			}
			return base.CanUseItem(player);
		}
	}

	public class RiftBeam : ModProjectile
	{
		public override string Texture => "Polarities/Content/NPCs/Bosses/PreHardmode/RiftDenizen/ChordRay";

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.alpha = 0;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
		}

		public override void AI()
		{
			Projectile.ai[0] = 2400f;
			Projectile.Center = Main.player[Projectile.owner].Center + Projectile.velocity * 64f;
			Main.player[Projectile.owner].direction = Projectile.velocity.X > 0 ? 1 : -1;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.ai[0], 8, ref point);
		}

		float timer = 0;
		public override bool PreDraw(ref Color lightColor)
		{
			float projScale = (float)(5 + Math.Sin(timer++ / 1.5f)) / 10f;
			int projOffset = 8;

			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Rectangle frame = texture.Frame();
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity * projScale * projOffset, frame, Color.White * (1 - Projectile.alpha / 255f), Projectile.velocity.ToRotation(), new Vector2(0, 16), new Vector2(Projectile.ai[0] - projScale * projOffset * 2, projScale), SpriteEffects.None, 0f);

			texture = Request<Texture2D>("Polarities/Content/NPCs/Bosses/PreHardmode/RiftDenizen/ChordRayCap").Value;
			frame = texture.Frame();
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity * projScale * projOffset, frame, Color.White * (1 - Projectile.alpha / 255f), Projectile.velocity.ToRotation(), new Vector2(32, 16), projScale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity * (Projectile.ai[0] - projScale * projOffset), frame, Color.White * (1 - Projectile.alpha / 255f), Projectile.velocity.ToRotation() + MathHelper.Pi, new Vector2(32, 16), projScale, SpriteEffects.None, 0f);

			return false;
		}
	}
}

