using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SpaceMAS.Interfaces;

namespace SpaceMAS.Models.Components
{
    public class BulletType
    {
        public float HealthChange, TravelSpeed;
        public IImpactEffect Effect;
        public Texture2D Texture;


        public BulletType(float healthChange, float travelSpeed, IImpactEffect effect, Texture2D texture)
        {
            HealthChange = healthChange;
            TravelSpeed = travelSpeed;
            Effect = effect;
            Texture = texture;
        }
    }
}
