using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Utils;
using System;
using System.Collections.Generic;

namespace MonoVoxel.Engine.Entities {
    
    public class MonoVoxelEntityManager {

        private List<MonoVoxelEntity> m_entities;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoVoxelEntityManager( ) 
            => m_entities = new List<MonoVoxelEntity>( );

        // <summary>
        /// Spawn an entity.
        /// </summary>
        /// <typeparam name="E" >Type of the entity to spawn based on MonoVoxelEntity</typeparam>
        /// <param name="game" >Current game instance</param>
        /// <returns>Newly entity</returns>
        public E Spawm<E>( MonoVoxelGame game ) where E : MonoVoxelEntity, new( ) {
            var entity = new E( );

            if ( entity != null ) {
                entity.Spawn( game );

                m_entities.Add( entity );
            }

            return entity;
        }

        /// <summary>
        /// Desroy an entity.
        /// </summary>
        /// <typeparam name="E" >Type of the entity to destroy based on MonoVoxelEntity</typeparam>
        /// <param name="game" >Current game instance</param>
        /// <param name="entity" >Entity to destroy</param>
        public void Destroy<E>( MonoVoxelGame game, E entity ) where E : MonoVoxelEntity {
            if ( entity != null )
                m_entities.Remove( entity );
        }

        /// <summary>
        /// Tick all entities.
        /// </summary>
        /// <param name="game_time" >Current tick (Update) game time</param>
        /// <param name="game" >Current game instance</param>
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

        /// <summary>
        /// Draw all entitis.
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        /// <param name="game_time" >Current draw game time</param>
        /// <param name="camera" >Current rendering camera</param>
        public void Draw( GraphicsDevice device, GameTime game_time, MonoVoxelCamera camera ) {
            foreach ( var entity in m_entities )
                entity.Draw( device, game_time, camera );
        }

        /// <summary>
        /// Find an entity.
        /// </summary>
        /// <typeparam name="E" >Type of the entity to query based on MonoVoxelEntity</typeparam>
        /// <param name="predicate" >Predicate for the searh</param>
        /// <returns>Entity thah match the predicate</returns>
        public E Find<E>( Predicate<MonoVoxelEntity> predicate ) where E : MonoVoxelEntity
            => (E)m_entities.Find( predicate );

    }
    
}
