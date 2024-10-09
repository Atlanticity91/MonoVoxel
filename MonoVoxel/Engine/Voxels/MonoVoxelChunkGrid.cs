using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Ressources;
using MonoVoxel.Engine.Utils;
using MonoVoxel.Engine.Voxels.Generators;
using System.Collections.Generic;

namespace MonoVoxel.Engine.Voxels {

    public sealed class MonoVoxelChunkGrid {

        private MonoVoxelChunkMesh m_mesh;
        private MonoVoxelChunk[] m_chunks;
        private List<MonoVoxelChunkGenerator> m_generators;
        private byte[] m_voxels;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        public MonoVoxelChunkGrid( GraphicsDevice device ) {
            m_mesh       = new MonoVoxelChunkMesh( device );
            m_chunks     = new MonoVoxelChunk[ MonoVoxelEngine.GridVolume ];
            m_generators = new List<MonoVoxelChunkGenerator> {
                new MonoVoxelChunkBasic( ) 
            };
            m_voxels = new byte[ MonoVoxelEngine.GridVolume * MonoVoxelEngine.ChunkVolume ];
        }

        /// <summary>
        /// Generate world chunks.
        /// </summary>
        /// <param name="block_manager" >Current block manager instance</param>
        public void GenerateChunks( MonoVoxelBlockManager block_manager ) {
            foreach ( var generator in m_generators )
                generator.Generate( ref m_chunks, ref m_voxels );

            BuildGeometry( block_manager );
        }

        /// <summary>
        /// Rebuild voxels geometry.
        /// </summary>
        /// <param name="block_manager" >Current block manager instance</param>
        private void BuildGeometry( MonoVoxelBlockManager block_manager ) {
            foreach ( var chunk in m_chunks )
                chunk.BuildGeometry( block_manager, m_voxels );
        }

        /// <summary>
        /// Draw the grid.
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        /// <param name="ressources" >Current ressource manager instance</param>
        /// <param name="camera" >Current camera instance</param>
        public void Draw( GraphicsDevice device, MonoVoxelRessourceManager ressources, MonoVoxelCamera camera ) {
            var material = ressources.GetMaterial( "Block" );
            var texture  = ressources.GetTexture( "Terrain" );

            material.Parameters[ "Texture" ].SetValue( texture );

            foreach ( var chunk in m_chunks ) {
                if ( chunk.VerticeCount > 0 && chunk.GetCanDraw( camera ) )
                    m_mesh.Draw( device, material, chunk, camera.Cache );
            }

        }

    }

}
