using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Entities;
using MonoVoxel.Engine.Ressources;
using MonoVoxel.Engine.Utils;
using MonoVoxel.Engine.Voxels;
using System;

namespace MonoVoxel.Engine {

    public class MonoVoxelEngine {

        // --- CHUNK ---
        public const int ChunkSize   = 32;
        public const int ChunkArea   = ChunkSize * ChunkSize;
        public const int ChunkVolume = ChunkArea * ChunkSize;

        // --- GRID ---
        public const int GridSize   = 2;
        public const int GridArea   = GridSize * GridSize;
        public const int GridVolume = GridArea * GridSize;

        private MonoVoxelGraphics m_graphics;
        private MonoVoxelCamera m_camera;
        private MonoVoxelChunkGrid m_grid;
        private MonoVoxelEntityManager m_entities;

        public MonoVoxelCamera Camera => m_camera;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        public MonoVoxelEngine( GraphicsDevice device ) {
            m_graphics = new MonoVoxelGraphics( );
            m_camera   = new MonoVoxelCamera( );
            m_grid     = new MonoVoxelChunkGrid( device );
            m_entities = new MonoVoxelEntityManager( );

            m_entities.Spawm<MonoVoxelEntityPlayer>( null );
        }

        /// <summary>
        /// Generate a voxel world.
        /// </summary>
        /// <param name="block_manager" >Current block manager instance</param>
        public void Generate( MonoVoxelBlockManager block_manager ) {
            m_grid.GenerateChunks( block_manager );
        }

        /// <summary>
        /// Spawn an entity.
        /// </summary>
        /// <typeparam name="E" >Type of the entity to spawn based on MonoVoxelEntity</typeparam>
        /// <param name="game" >Current game instance</param>
        /// <returns>Newly entity</returns>
        public E Spawn<E>( MonoVoxelGame game ) where E : MonoVoxelEntity, new( )
            => m_entities.Spawm<E>( game );

        /// <summary>
        /// Desroy an entity.
        /// </summary>
        /// <typeparam name="E" >Type of the entity to destroy based on MonoVoxelEntity</typeparam>
        /// <param name="game" >Current game instance</param>
        /// <param name="entity" >Entity to destroy</param>
        public void Destroy<E>( MonoVoxelGame game, E entity ) where E : MonoVoxelEntity
            => m_entities.Destroy( game, entity );

        /// <summary>
        /// Tick the engine aka tick voxels and entitird.
        /// </summary>
        /// <param name="game_time" >Current tick (Update) game time</param>
        /// <param name="game" >Current game instance</param>
        public void Tick( GameTime game_time, MonoVoxelGame game ) {
            m_entities.Tick( game_time, game );
            m_camera.Tick( game.GraphicsDevice );
        }

        /// <summary>
        /// Draw the engine aka draw voxels and entity.
        /// </summary>
        /// <param name="game_time" >Current draw game time</param>
        /// <param name="game" >Current game instance</param>
        public void Draw( GameTime game_time, MonoVoxelGame game ) {
            m_graphics.Apply( game.GraphicsDevice );
            m_grid.Draw( game.GraphicsDevice, game.Ressources, m_camera );
            m_entities.Draw( game.GraphicsDevice, game_time, m_camera );
        }

        /// <summary>
        /// Find an entity.
        /// </summary>
        /// <typeparam name="E" >Type of the entity to query based on MonoVoxelEntity</typeparam>
        /// <param name="predicate" >Predicate for the searh</param>
        /// <returns>Entity thah match the predicate</returns>
        public E Find<E>( Predicate<MonoVoxelEntity> predicate ) where E : MonoVoxelEntity
            => m_entities.Find<E>( predicate );

    }

}
