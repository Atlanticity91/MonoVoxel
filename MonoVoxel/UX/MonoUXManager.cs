using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Ressources;

namespace MonoVoxel.UX
{

    public class MonoUXManager {

        private SpriteBatch m_batch;

        public MonoUXManager( GraphicsDevice device ) {
            m_batch = new SpriteBatch( device );
        }

        public void Tick( GameTime game_time, MonoVoxelGame game ) { 
        }

        public void Draw( GameTime game_time, MonoVoxelRessourceManager ressources ) {
        }

    }

}
