using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Entities;
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

        public MonoVoxelEngine( GraphicsDevice device ) {
            m_graphics = new MonoVoxelGraphics( );
            m_camera   = new MonoVoxelCamera( );
            m_grid     = new MonoVoxelChunkGrid( device );
            m_entities = new MonoVoxelEntityManager( );

            m_entities.Spawm<MonoVoxelEntityPlayer>( null );
        }

        public void Generate( ) {
            m_grid.GenerateChunks( );
        }

        public E Spawn<E>( MonoVoxelGame game ) where E : MonoVoxelEntity, new( )
            => m_entities.Spawm<E>( game );

        public void Destroy<E>( MonoVoxelGame game, E entity ) where E : MonoVoxelEntity
            => m_entities.Destroy( game, entity );

        public void Tick( GameTime game_time, MonoVoxelGame game ) {
            m_entities.Tick( game_time, game );
            m_camera.Tick( game.GraphicsDevice );
        }

        public void Draw( GameTime game_time, MonoVoxelGame game ) {
            m_graphics.Apply( game.GraphicsDevice );
            m_grid.Draw( game.GraphicsDevice, game.Ressources, m_camera );
            m_entities.Draw( game.GraphicsDevice, game_time, m_camera );
        }

        public E Find<E>( Predicate<MonoVoxelEntity> predicate ) where E : MonoVoxelEntity
            => m_entities.Find<E>( predicate );

    }

}
