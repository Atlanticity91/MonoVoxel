using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Ressources;

namespace MonoVoxel.UX {

    public class MonoUXManager {

        private SpriteBatch m_batch;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        public MonoUXManager( GraphicsDevice device ) {
            m_batch = new SpriteBatch( device );
        }

        /// <summary>
        /// Tick ui elements.
        /// </summary>
        /// <param name="game_time" >Current tick ( Update ) game time</param>
        /// <param name="game" >Current game instance</param>
        public void Tick( GameTime game_time, MonoVoxelGame game ) { 
        }

        /// <summary>
        /// Draw ui elements
        /// </summary>
        /// <param name="game_time" >Current draw game time</param>
        /// <param name="ressources" >Current ressource manager instance</param>
        public void Draw( GameTime game_time, MonoVoxelRessourceManager ressources ) {
        }

    }

}
