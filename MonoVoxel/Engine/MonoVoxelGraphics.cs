using Microsoft.Xna.Framework.Graphics;

namespace MonoVoxel.Engine {
    
    public sealed class MonoVoxelGraphics {

        private RasterizerState m_rasterizer;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoVoxelGraphics( ) {
            m_rasterizer = new RasterizerState( );

            m_rasterizer.CullMode = CullMode.CullCounterClockwiseFace;
        }

        /// <summary>
        /// Apply graphics configuration for 3d rendering.
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        public void Apply( GraphicsDevice device ) {
            device.SamplerStates[ 0 ] = SamplerState.PointClamp;
            device.RasterizerState = m_rasterizer;
        }

    }

}
