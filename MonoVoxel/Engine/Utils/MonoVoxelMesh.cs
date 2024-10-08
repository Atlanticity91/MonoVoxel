﻿using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace MonoVoxel.Engine.Utils {

    public class MonoVoxelMesh<T> where T : struct {

        private VertexBuffer m_vertices;
        private IndexBuffer m_indices;

        public VertexBuffer Vertex => m_vertices;
        public IndexBuffer Index   => m_indices;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        /// <param name="vertex_count" >Mesh vertex count</param>
        /// <param name="index_count" >Mesh index count</param>
        public MonoVoxelMesh( GraphicsDevice device, int vertex_count, int index_count ) {
            m_vertices = new VertexBuffer( device, typeof( T ), vertex_count, BufferUsage.WriteOnly );
            m_indices  = new IndexBuffer( device, IndexElementSize.ThirtyTwoBits, index_count, BufferUsage.WriteOnly );
        }

        /// <summary>
        /// Set mesh vertices.
        /// </summary>
        /// <param name="vertices" >Array of vertices</param>
        public void SetVertices( T[] vertices )
            => m_vertices.SetData( vertices );

        /// <summary>
        /// Set part of mesh vertices.
        /// </summary>
        /// <param name="start" >Count of vertices before</param>
        /// <param name="count" >Count of vertices to update</param>
        /// <param name="vertices" >Array of vertices</param>
        public void SetVertices( int start, int count, T[] vertices )
            => m_vertices.SetData( start * Marshal.SizeOf<T>( ), vertices, 0, count, 0 );

        /// <summary>
        /// Set mesh vertex indicies.
        /// </summary>
        /// <param name="indices" >Array of vertice index.</param>
        public void SetIndices( int[] indices )
            => m_indices.SetData( indices );

        /// <summary>
        /// Set mesh vertex indicies.
        /// </summary>
        /// <param name="start" >Count of vertex indicies before</param>
        /// <param name="count" >Count of vertex indicies to update</param>
        /// <param name="indices" >Array of vertice index.</param>
        public void SetIndices( int start, int count, int[] indices )
            => m_indices.SetData( start * Marshal.SizeOf<int>( ), indices, 0, count );

        /// <summary>
        /// Draw the mesh.
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        public void Draw( GraphicsDevice device ) {
            var triangle_count =  m_indices.IndexCount / 3;

            Draw( device, triangle_count );
        }

        /// <summary>
        /// Draw the mesh.
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        /// <param name="triangle_count" >Count of triangle of the mesh to draw</param>
        public void Draw( GraphicsDevice device, int triangle_count ) {
            if ( triangle_count < 1 )
                return;

            device.Indices = m_indices;

            device.SetVertexBuffer( m_vertices );
            device.DrawIndexedPrimitives( PrimitiveType.TriangleList, 0, 0, triangle_count );
        }

    }

}
