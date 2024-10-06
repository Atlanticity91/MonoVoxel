using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoVoxel.Engine.Ressources {

    public sealed class MonoVoxelRessourceManager {

        private ContentManager m_content;
        private Dictionary<string, Model> m_meshes;
        private Dictionary<string, Texture2D> m_textures;
        private Dictionary<string, int> m_sprites;
        private Dictionary<string, Effect> m_materials;
        private MonoVoxelBlockManager m_blocks;

        public MonoVoxelBlockManager BlockManager => m_blocks;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content" >Current content manager instance</param>
        public MonoVoxelRessourceManager( ContentManager content ) {
            m_content   = content;
            m_meshes    = new Dictionary<string, Model>( );
            m_textures  = new Dictionary<string, Texture2D>( );
            m_sprites   = new Dictionary<string, int>( );
            m_materials = new Dictionary<string, Effect>( );
            m_blocks    = new MonoVoxelBlockManager( );
        }

        /// <summary>
        /// Load a mesh.
        /// </summary>
        /// <param name="name" >Name of the mesh</param>
        /// <param name="path" >Path to mesh file</param>
        public void LoadMesh( string name, string path ) {
            if ( !string.IsNullOrEmpty( name ) && !string.IsNullOrEmpty( path ) && !m_meshes.ContainsKey( name) ) {
                var model = m_content.Load<Model>( path );

                if ( model != null )
                    m_meshes.Add( name, model );
            }
        }

        /// <summary>
        /// Load a texture.
        /// </summary>
        /// <param name="name" >Name of the texture</param>
        /// <param name="path" >Path to the texture</param>
        public void LoadTexture( string name, string path ) {
            if ( !string.IsNullOrEmpty( name ) && !string.IsNullOrEmpty( path ) && !m_textures.ContainsKey( name ) ) {
                var texture = m_content.Load<Texture2D>( path );

                if ( texture != null )
                    m_textures.Add( name, texture );
            }
        }

        /// <summary>
        /// Load a material.
        /// </summary>
        /// <param name="name" >Name of the material</param>
        /// <param name="path" >Path to the material</param>
        public void LoadMaterial( string name, string path ) {
            if ( !string.IsNullOrEmpty( name ) && !string.IsNullOrEmpty( path ) && !m_textures.ContainsKey( name ) ) {
                var material = m_content.Load<Effect>( path );

                if ( material != null )
                    m_materials.Add( name, material );
            }
        }

        /// <summary>
        /// Add a block.
        /// </summary>
        /// <param name="block" >New block</param>
        public void AddBlock( MonoVoxelBlock block )
            => m_blocks.Add( block );

        /// <summary>
        /// Add blcoks.
        /// </summary>
        /// <param name="blocks" >Array of new blocks.</param>
        public void AddBlocks( params MonoVoxelBlock[] blocks ) {
            foreach ( var block in blocks )
                m_blocks.Add( block );
        }

        /// <summary>
        /// Get a texture by name.
        /// </summary>
        /// <param name="name" >Name of the texture</param>
        /// <returns>Query texture</returns>
        public Texture2D GetTexture( string name ) {
            var texture = (Texture2D)null;

            if ( m_textures.ContainsKey( name ) )
                texture = m_textures[ name ];

            return texture;
        }

        /// <summary>
        /// Get a material by name.
        /// </summary>
        /// <param name="name" >Name of the material</param>
        /// <returns>Query material</returns>
        public Effect GetMaterial( string name ) {
            var material = (Effect)null;

            if ( m_materials.ContainsKey( name ) )
                material = m_materials[ name ];

            return material;
        }

        /// <summary>
        /// Get a block by index.
        /// </summary>
        /// <param name="block_id" >Query block index</param>
        /// <returns>Query block</returns>
        public MonoVoxelBlock? GetBlock( int block_id )
            => m_blocks.Get( block_id );

    }

}
