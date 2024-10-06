using Microsoft.Xna.Framework;

namespace MonoVoxel.Engine.Entities {
    
    public class MonoVoxelEntityPlayer : MonoVoxelEntity {

        private float m_speed;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoVoxelEntityPlayer( ) {
            m_speed = 2.0f;
        }

        /// <summary>
        /// Tick the player.
        /// </summary>
        /// <param name="game_time" >Current tick (Update) game time</param>
        /// <param name="game" >Current game instance</param>
        public override void Tick( GameTime game_time, MonoVoxelGame game ) {
            if ( !game.IsActive )
                return;

            var velocity  = (float)game_time.ElapsedGameTime.TotalSeconds * m_speed;
            var camera    = game.Engine.Camera;
            var mouse     = game.Inputs.GetAxis( 1, 0 );

            camera.Rotate( mouse * 0.002f );

            if ( game.Inputs.Evaluate( "MoveForward", true ) )
                camera.MoveForward( velocity );

            if ( game.Inputs.Evaluate( "MoveBackward", true ) )
                camera.MoveBackward( velocity );

            if ( game.Inputs.Evaluate( "MoveLeft", true ) )
                camera.MoveLeft( velocity );

            if ( game.Inputs.Evaluate( "MoveRight", true ) )
                camera.MoveRight( velocity );

            if ( game.Inputs.Evaluate( "MoveUp", true ) )
                camera.MoveUp( velocity );

            if ( game.Inputs.Evaluate( "MoveDown", true ) )
                camera.MoveDown( velocity );
        }

    }

}
