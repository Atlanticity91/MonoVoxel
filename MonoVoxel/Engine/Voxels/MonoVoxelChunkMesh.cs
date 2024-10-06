using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoVoxel.Engine.Utils;

namespace MonoVoxel.Engine.Voxels {
    
    public sealed class MonoVoxelChunkMesh {

        private const int VertexCount = 12; // 12 vertices per block
        private const int IndexCount  = 18; // 18 indicies per block

        private MonoVoxelMesh<MonoVoxelChunkVertice> m_mesh;
        private int m_vertices;

        public MonoVoxelChunkMesh( GraphicsDevice device ) {
            m_mesh     = new MonoVoxelMesh<MonoVoxelChunkVertice>( device, MonoVoxelEngine.ChunkVolume * VertexCount, MonoVoxelEngine.ChunkVolume * IndexCount );
            m_vertices = 0;

            GenerateIndicies( );
        }

        private void GenerateIndicies( ) {
            var vertice_indicies = new int[ 6 ]{ 0, 1, 2, 2, 3, 0 };
            var stride = 0;
            var offset = 0;

            for ( int block_id = 0; block_id < MonoVoxelEngine.ChunkVolume; block_id++ ) {
                for ( int i = 0; i < 3; i++ ) {
                    vertice_indicies[ 0 ] = offset;
                    vertice_indicies[ 1 ] = offset + 1;
                    vertice_indicies[ 2 ] = offset + 2;
                    vertice_indicies[ 3 ] = offset + 2;
                    vertice_indicies[ 4 ] = offset + 3;
                    vertice_indicies[ 5 ] = offset;

                    m_mesh.SetIndices( stride, 6, vertice_indicies );

                    stride += 6;
                    offset += 4;
                }
            }
        }

        public void Push( int vertixe_count, MonoVoxelChunkVertice[] vertices, Matrix location ) {
            m_mesh.SetVertices( m_vertices, vertixe_count, vertices );

            m_vertices += vertixe_count;
        }

        public void Draw( GraphicsDevice device, Effect material, MonoVoxelChunk chunk, Matrix mvp ) {
            material.Parameters[ "WorldTransform" ].SetValue( chunk.Location * mvp );
            material.CurrentTechnique.Passes[ 0 ].Apply( );

            m_mesh.SetVertices( 0, chunk.VerticeCount, chunk.Geometry );
            m_mesh.Draw( device, chunk.VerticeCount );
        }

    }

}
