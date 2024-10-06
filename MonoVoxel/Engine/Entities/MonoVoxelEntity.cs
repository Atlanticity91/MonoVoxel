using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Utils;

namespace MonoVoxel.Engine.Entities {
    
    public class MonoVoxelEntity {

        protected bool m_is_alive;
        protected MonoVoxelEntity m_parent;
        
        public bool IsAlive => m_is_alive;

        public MonoVoxelEntity( ) {
            m_is_alive = true;
            m_parent   = null;
        }

        public virtual void Spawn( MonoVoxelGame game ) { }

        public virtual void Kill( MonoVoxelGame game ) 
            => m_is_alive = false;

        public virtual void Tick( GameTime game_time, MonoVoxelGame game ) {
        }

        public virtual void Draw( GraphicsDevice device, GameTime game_time, MonoVoxelCamera camera ) { 
        }

    }

}
