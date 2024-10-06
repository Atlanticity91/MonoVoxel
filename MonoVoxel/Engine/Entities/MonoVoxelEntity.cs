using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Utils;

namespace MonoVoxel.Engine.Entities {
    
    public class MonoVoxelEntity {

        protected bool m_is_alive;
        protected MonoVoxelEntity m_parent;
        
        public bool IsAlive => m_is_alive;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoVoxelEntity( ) {
            m_is_alive = true;
            m_parent   = null;
        }

        /// <summary>
        /// Spawn entity, called when entity is created by Spawn<E> function.
        /// </summary>
        /// <param name="game" >Current game instance</param>
        public virtual void Spawn( MonoVoxelGame game ) { }

        /// <summary>
        /// Kill entity, called when entity is destroyed after this call of Destroy<E>.
        /// </summary>
        /// <param name="game" >Current game instance</param>
        public virtual void Kill( MonoVoxelGame game ) 
            => m_is_alive = false;

        /// <summary>
        /// Tick entity.
        /// </summary>
        /// <param name="game_time" >Current tick (Update) game time</param>
        /// <param name="game" >Current game instance</param>
        public virtual void Tick( GameTime game_time, MonoVoxelGame game ) { }

        /// <summary>
        /// Draw entity.
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        /// <param name="game_time" >Current draw game time</param>
        /// <param name="camera" >Current rendering camera</param>
        public virtual void Draw( GraphicsDevice device, GameTime game_time, MonoVoxelCamera camera ) { }

    }

}
