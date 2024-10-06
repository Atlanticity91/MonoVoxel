using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Utils;
using System;
using System.Collections.Generic;

namespace MonoVoxel.Engine.Entities {
    
    public class MonoVoxelEntityManager {

        private List<MonoVoxelEntity> m_entities;

        public MonoVoxelEntityManager( ) {
            m_entities = new List<MonoVoxelEntity>( );
        }

        public E Spawm<E>( MonoVoxelGame game ) where E : MonoVoxelEntity, new( ) {
            var entity = new E( );

            if ( entity != null ) {
                entity.Spawn( game );

                m_entities.Add( entity );
            }

            return entity;
        }

        public void Destroy<E>( MonoVoxelGame game, E entity ) where E : MonoVoxelEntity {
            if ( entity != null )
                m_entities.Remove( entity );
        }

        public void Tick( GameTime game_time, MonoVoxelGame game ) {
            var dead_entities = new List<MonoVoxelEntity>( );

            foreach ( var entity in m_entities ) {
                if ( entity.IsAlive )
                    entity.Tick( game_time, game );
                else
                    dead_entities.Add( entity );
            }

            foreach ( var entity in dead_entities )
                m_entities.Remove( entity );
        }

        public void Draw( GraphicsDevice device, GameTime game_time, MonoVoxelCamera camera ) {
            foreach ( var entity in m_entities )
                entity.Draw( device, game_time, camera );
        }

        public E Find<E>( Predicate<MonoVoxelEntity> predicate ) where E : MonoVoxelEntity
            => (E)m_entities.Find( predicate );

    }
    
}
