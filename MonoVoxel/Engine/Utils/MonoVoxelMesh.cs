using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace MonoVoxel.Engine.Utils {

    public class MonoVoxelMesh<T> where T : struct {

        private VertexBuffer m_vertices;
        private IndexBuffer m_indices;

        public VertexBuffer Vertex => m_vertices;
        public IndexBuffer Index => m_indices;

        public MonoVoxelMesh( GraphicsDevice device, int vertex_count, int index_count ) {
            m_vertices = new VertexBuffer( device, typeof( T ), vertex_count, BufferUsage.WriteOnly );
            m_indices  = new IndexBuffer( device, IndexElementSize.ThirtyTwoBits, index_count, BufferUsage.WriteOnly );
        }

        public void SetVertices( T[] vertices )
            => m_vertices.SetData( vertices );

        public void SetVertices( int start, int count, T[] vertices )
            => m_vertices.SetData( start * Marshal.SizeOf<T>( ), vertices, 0, count, 0 );

        public void SetIndices( int[] indices )
            => m_indices.SetData( indices );

        public void SetIndices( int start, int count, int[] indices )
            => m_indices.SetData( start * Marshal.SizeOf<int>( ), indices, 0, count );

        public void Draw( GraphicsDevice device ) {
            var triangle_count =  m_indices.IndexCount / 3;

            Draw( device, triangle_count );
        }

        public void Draw( GraphicsDevice device, int triangle_count ) {
            if ( triangle_count < 1 )
                return;

            device.Indices = m_indices;

            device.SetVertexBuffer( m_vertices );
            device.DrawIndexedPrimitives( PrimitiveType.TriangleList, 0, 0, triangle_count );
        }

    }

}
