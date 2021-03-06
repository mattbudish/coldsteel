﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Coldsteel
{
    public class ColdsteelComponent : DrawableGameComponent
    {
        private IColdsteelInitializer _initializer;

        private GameStageManager _gameStageManager;

        /// <summary>
        /// Gets the GameStageManager instance used for this Game.
        /// </summary>
        public GameStageManager GameStageManager { get { return _gameStageManager; } }

        /// <summary>
        /// Construct a new ColdsteelComponent with the provided game and IColdsteelInitializer.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="initializer"></param>
        public ColdsteelComponent(Game game, IColdsteelInitializer initializer) 
            : base(game)
        {
            _initializer = initializer;
        }

        /// <summary>
        /// Should be invoked by MonoGame during its intialization/gameloop.
        /// </summary>
        public override void Initialize()
        {
            var input = new Input();            
            _initializer.InitializeControls(input);
            var stages = new GameStageRegistry();
            _initializer.RegisterStages(stages);
            if (!stages.Keys.Any())
                throw new InvalidOperationException("At least 1 GameStage must be registered when the RegisterStages method is invoked on the IColdsteelInitializer");
            base.Initialize();
            _gameStageManager = new GameStageManager(input, stages);
            _gameStageManager.Initialize(new MonoGameResourceFactory(this.Game));
        }

        /// <summary>
        /// Should be invoked by MonoGame during its intialization/gameloop.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_gameStageManager.FirstUpdate)
                Game.ResetElapsedTime();

            _gameStageManager.Update(new GameTimeWrapper(gameTime));
        }

        /// <summary>
        /// Should be invoked by MonoGame during its intialization/gameloop.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            _gameStageManager.Render(new GameTimeWrapper(gameTime));
        }
    }
}
