using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Ressources;
using MonoVoxel.Engine.Utils;
using MonoVoxel.Engine.Voxels.Generators;
using System.Collections.Generic;

namespace MonoVoxel.Engine.Voxels {

    public sealed class MonoVoxelChunkGrid {

        private MonoVoxelChunkMesh m_mesh;
        private MonoVoxelChunk[] m_chunks;
        private bool m_rebuild;
        private List<MonoVoxelChunkGenerator> m_generators;
        private byte[] m_voxels;

        public MonoVoxelChunkGrid( GraphicsDevice device ) {
            m_mesh       = new MonoVoxelChunkMesh( device );
            m_chunks     = new MonoVoxelChunk[ MonoVoxelEngine.GridVolume ];
            m_rebuild    = true;
            m_generators = new List<MonoVoxelChunkGenerator> {
                new MonoVoxelChunkBasic( ) 
            };
            m_voxels = new byte[ MonoVoxelEngine.GridVolume * MonoVoxelEngine.ChunkVolume ];
        }
        
        public void GenerateChunks( ) {
            foreach ( var generator in m_generators )
                generator.Generate( ref m_chunks, ref m_voxels );
        }

        private void RebuildGeometry( MonoVoxelBlockManager block_manager ) {
            foreach ( var chunk in m_chunks )
                chunk.BuildGeometry( block_manager, m_voxels );

            m_rebuild = false;
        }

        private void DrawChunks( GraphicsDevice device, Effect material, Matrix mvp ) {
            foreach ( var chunk in m_chunks ) {
                if ( chunk.VerticeCount > 0 )
                    m_mesh.Draw( device, material, chunk, mvp );
            }
        }

        public void Draw( GraphicsDevice device, MonoVoxelRessourceManager ressources, MonoVoxelCamera camera ) {
            var material = ressources.GetMaterial( "Block" );
            var texture  = ressources.GetTexture( "Terrain" );

            material.Parameters[ "Diffuse" ].SetValue( texture );

            if ( m_rebuild )
                RebuildGeometry( ressources.BlockManager );

            DrawChunks( device, material, camera.Cache );

        }

    }

}
