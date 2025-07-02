using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Polarities.Core;
using Polarities.Global;
using Polarities.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Weapons.Ranged.Guns.PreHardmode
{
	public class Parallaxian : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Shoots bullets at the cursor from the third dimension");
		}

		public override void SetDefaults()
		{
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = AmmoID.Bullet;

            Item.width = 44;
            Item.height = 24;
            Item.useTime = 7;
            Item.useAnimation = 6;

            Item.shoot = ProjectileType<ParallaxianProjectile>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item11;
            Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 10f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ProjectileType<ParallaxianProjectile>();

            //position = player.MountedCenter + (player.MountedCenter - Main.MouseWorld).SafeNormalize(Vector2.Zero).RotatedByRandom(MathHelper.PiOver4) * -100;
            //velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * velocity.Length() * Main.rand.NextFloat(0.5f, 1.5f);

            position = Main.MouseWorld + Main.rand.NextVector2Circular(30, 30);
            velocity = position.DirectionTo(Main.MouseWorld + Main.rand.NextVector2Circular(45, 45)) * velocity.Length() * Main.rand.NextFloat(0.5f, 1.5f);

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }

	public class ParallaxianProjectile : ModProjectile
	{
        public override string Texture => "Polarities/Content/Projectiles/CallShootProjectile";

        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Parallax Bullet");
		}

        int trailLength;
		public override void SetDefaults()
		{
			Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;

            Projectile.GetGlobalProjectile<PolaritiesProjectile>().usesGeneralHitCooldowns = true;
            Projectile.GetGlobalProjectile<PolaritiesProjectile>().generalHitCooldownTime = 100;

            trailLength = Main.rand.Next(3, 10);
		}

		public override void AI()
		{
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */
        {
            return Math.Abs(trailLength * 2 - 1 - Projectile.timeLeft) < 8;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float motion = 23 - Projectile.timeLeft;

			hitbox.X += (int)(Projectile.velocity.X * motion);
			hitbox.Y += (int)(Projectile.velocity.Y * motion);
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = texture.Frame();

			for (int i = -trailLength; i <= trailLength; i++)
			{
                float fullTrailSize = trailLength * 2 - 1;

                float motion = fullTrailSize - Projectile.timeLeft - i;
				float alpha = Math.Max(0, 1 - (fullTrailSize - Projectile.timeLeft - i) * (fullTrailSize - Projectile.timeLeft - i) / fullTrailSize / fullTrailSize) * (8 - i) / 16f;
				float scale = (1 + Projectile.ai[0] * (fullTrailSize - Projectile.timeLeft - i) / fullTrailSize) * (8 - i) / 16f;

				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity * motion, frame, Color.White * alpha, Projectile.velocity.ToRotation(), new Vector2(1, 1), new Vector2(Projectile.velocity.Length(), scale), SpriteEffects.None, 0f);
			}

			return false;
		}
	}
}
