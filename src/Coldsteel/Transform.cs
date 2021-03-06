﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coldsteel
{
    public class Transform : GameObjectComponent
    {
        /// <summary>
        /// Gets or sets the position relative to a parent game object.
        /// </summary>
        public Vector2 LocalPosition { get; set; } = Vector2.Zero;

        /// <summary>
        /// Gets the position of the parent. If none then the origin.
        /// </summary>
        private Vector2 ParentPosition
        {
            get
            {
                return this.GameObject?.Parent?.GetComponent<Transform>().Position ?? Vector2.Zero;
            }
        }

        /// <summary>
        /// Gets or sets the position of a GameObject relative to the Parent's Transform position or the origin.
        /// </summary>
        public Vector2 Position
        {
            get { return this.LocalPosition + this.ParentPosition; }
            set { this.LocalPosition = value - this.ParentPosition; }
        }

        /// <summary>
        /// Gets or sets the Rotation of a GameObject (in Radians).
        /// </summary>
        public float Rotation { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the Scale of the GameObject.
        /// </summary>
        public float Scale { get; set; } = 1f;
    }
}
