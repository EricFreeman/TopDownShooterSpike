using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TopDownShooterSpike.State;

namespace TopDownShooterSpike.Managers
{
    public interface IGameStateStack
    {
        void Push(GameState gameState);
        void Push<T>() where T : GameState;
        GameState Pop();
    }

    public class GameStateStack : DrawableGameComponent, IGameStateStack
    {
        private readonly IServiceContainer _container;
        private readonly IList<GameState> _gameStates;

        public GameStateStack(IServiceContainer container, Game game) : base(game)
        {
            _container = container;
            _gameStates = new List<GameState>();

            game.Components.Add(this);
            DrawOrder = 1;
            UpdateOrder = 1;
        }

        #region Game Component Methods

        public override void Initialize()
        {

        }


        public override void Update(GameTime gameTime)
        {

            var last = _gameStates.Count - 1;
            _gameStates[last].Update(gameTime);
            
        }

        public override void Draw(GameTime gameTime)
        {

            var last = _gameStates.Count - 1;
            _gameStates[last].Draw(gameTime);
        }

        #endregion

        #region Game State Stack Operations

        public void Push(GameState gameState)
        {
            gameState.Initialize();
            _gameStates.Add(gameState);
        }

        public GameState Pop()
        {
            var last = _gameStates.Count - 1;
            var state = _gameStates[last];
            _gameStates.RemoveAt(last);

            state.TearDown();

            return state;
        }

        public void Push<T>() where T : GameState
        {
            var state = _container.Create<T>();

            this.Push(state);
        }

        #endregion

    }

}
