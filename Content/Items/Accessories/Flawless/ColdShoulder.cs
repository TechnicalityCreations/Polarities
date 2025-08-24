using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Polarities.Global;
using Terraria.Audio;
using Terraria.GameContent;
using static Terraria.ModLoader.ModContent;

namespace Polarities.Content.Items.Accessories.Flawless
{
    
    public class ColdShoulder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = (1);
            PolaritiesItem.IsFlawless.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.noUseGraphic = true;
            Item.damage = 50;
            Item.knockBack = 7f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 18f;
            Item.shoot = ProjectileType<ColdShoulderProjectile>();
            Item.width = 30;
            Item.height = 26;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.Green;
            Item.noMelee = true;
            Item.value = 5000;
        }
    }
    public class ColdShoulderProjectile : ModProjectile
    {
        //texture cacheing
        public static Asset<Texture2D> ChainTexture;

        public NPC attached;
        public int attachedTime = -1;
        
        private SpriteEffects effects = SpriteEffects.None;

        public override void Load()
        {
            ChainTexture = Request<Texture2D>(Texture + "_Chain");
        }

        public override void Unload()
        {
            ChainTexture = null;

        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 24;
            Projectile.height = 32;

            Projectile.aiStyle = 7;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 10;
            
        }

        private int pullTime = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            
            if (Projectile.localAI[0] == 0)
            {
                effects = Projectile.velocity.X > 0
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None;
                Projectile.localAI[0] = 1;
            }
            if (!Main.dedServ)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch).noGravity = true;
            }
            if (attached != null && attachedTime > 0)
            {
                Projectile.Center = attached.Center;
                attachedTime--;
                if (attached.life < 1) attachedTime = -1;
                if (!attached.boss) attached.position -= attached.velocity * Math.Min(1, attached.knockBackResist);
            }
            else if (attached != null && attached.life > 0 && pullTime < 5)
            {
                Projectile.friendly = true;
                if (attached.knockBackResist < 0.2f || attached.boss) player.velocity += player.Center.DirectionTo(Projectile.Center) * 5f;
                else attached.velocity += attached.Center.DirectionTo(player.Center) * 5f * Math.Min(1, attached.knockBackResist);
                pullTime++;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (attached == null)
            {
                SoundEngine.PlaySound(SoundID.Item27, target.Center);
                attached = target;
                attachedTime = 60;
                Projectile.friendly = false;
            }
        }

        // Amethyst Hook is 300, Static Hook is 600
        public override float GrappleRange()
        {
            return 500f;
        }

        public override bool? CanUseGrapple(Player player)
        {
            return player.ownedProjectileCounts[Type] < 2;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 2;
        }

        // default is 11, Lunar is 24
        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 18f;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 10f;
        }

        public override bool PreDrawExtras()
        {
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = playerCenter - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            while (distance > 16f && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= 16f;
                center += distToProj;
                distToProj = playerCenter - center;
                distance = distToProj.Length();

                //Draw chain
                Main.spriteBatch.Draw(ChainTexture.Value, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, 10, 16), Color.White, projRotation,
                    new Vector2(10 * 0.5f, 16 * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Main.spriteBatch.Draw(texture, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y),
                new Rectangle(0, 0, 24, 32), Color.White, Projectile.rotation,
                new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, effects, 0f);
            return false;
        }
    }
}
