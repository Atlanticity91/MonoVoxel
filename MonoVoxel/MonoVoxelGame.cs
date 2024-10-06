using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoVoxel.Engine;
using MonoVoxel.Engine.Ressources;
using MonoVoxel.Inputs;
using MonoVoxel.UX;

namespace MonoVoxel {

    public class MonoVoxelGame : Game {

        private GraphicsDeviceManager m_graphics;
        private MonoVoxelRessourceManager m_ressources;
        private MonoInputManager m_inputs;
        private MonoVoxelEngine m_engine;
        private MonoUXManager m_ux;
        private Color m_refresh;

        public MonoVoxelRessourceManager Ressources => m_ressources;
        public MonoInputManager Inputs => m_inputs;
        public MonoVoxelEngine Engine => m_engine;
        public MonoUXManager UX => m_ux;

        public MonoVoxelGame( ) {
            m_graphics = new GraphicsDeviceManager( this );

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            m_graphics.PreferredBackBufferWidth = 1280;
            m_graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize( ) {
            m_inputs  = new MonoInputManager( );
            m_refresh = new Color( 0.1f, 0.16f, 0.25f, 1.0f );

            base.Initialize( );
        }

        protected override void LoadContent( ) {
            m_ressources = new MonoVoxelRessourceManager( Content );
            m_engine     = new MonoVoxelEngine( GraphicsDevice );
            m_ux         = new MonoUXManager( GraphicsDevice );

            m_inputs.Register( "MoveForward", new MonoInputQuery[] { new MonoInputQuery( Keys.W, MonoInputStates.Down ) } );
            m_inputs.Register( "MoveBackward", new MonoInputQuery[] { new MonoInputQuery( Keys.S, MonoInputStates.Down ) } );
            m_inputs.Register( "MoveLeft", new MonoInputQuery[] { new MonoInputQuery( Keys.A, MonoInputStates.Down ) } );
            m_inputs.Register( "MoveRight", new MonoInputQuery[] { new MonoInputQuery( Keys.D, MonoInputStates.Down ) } );
            m_inputs.Register( "MoveUp", new MonoInputQuery[] { new MonoInputQuery( Keys.Space, MonoInputStates.Down ) } );
            m_inputs.Register( "MoveDown", new MonoInputQuery[] { new MonoInputQuery( Keys.Q, MonoInputStates.Down ) } );

            m_ressources.LoadTexture( "Terrain", "textures/terrain" );

            m_ressources.LoadMaterial( "Block", "materials/chunk_block" );
            m_ressources.LoadMaterial( "Skybox", "materials/skybox" );

            m_ressources.CreateBlocks(
                new( MonoVoxelBlockManager.UV( 1, 0 ) ),
                new( MonoVoxelBlockManager.UV( 2, 0 ) ),
                new( MonoVoxelBlockManager.UV( 3, 0 ) ),
                new( 
                    MonoVoxelBlockManager.UV( 6, 0 ),
                    MonoVoxelBlockManager.UV( 6, 0 ),
                    MonoVoxelBlockManager.UV( 5, 0 ),
                    MonoVoxelBlockManager.UV( 5, 0 ),
                    MonoVoxelBlockManager.UV( 5, 0 ),
                    MonoVoxelBlockManager.UV( 5, 0 )
                ),
                new(
                    new( 0.1875f, 0.0f, 0.25f, 0.1875f ), // top
                    new( 0.125f, 0.0f, 0.1875f, 0.125f ), // bottom
                    new( 0.125f, 0.0f, 0.1875f, 0.125f ), 
                    new( 0.125f, 0.0f, 0.1875f, 0.125f ),
                    new( 0.125f, 0.0f, 0.1875f, 0.125f ),
                    new( 0.125f, 0.0f, 0.1875f, 0.125f )
                )
            );

            m_engine.Generate( );
        }

        protected override void Update( GameTime game_time ) {
            m_inputs.Tick( );
            m_engine.Tick( game_time, this );
            m_ux.Tick( game_time, this );
        }

        protected override void Draw( GameTime game_time ) {
            GraphicsDevice.Clear( m_refresh );

            m_engine.Draw( game_time, this );
            m_ux.Draw( game_time, m_ressources );
        }

    }

}
