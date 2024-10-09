using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoVoxel.Engine.Utils {
    
    public class MonoVoxelCamera {

        private Matrix m_projection;
        private Matrix m_view;
        private Matrix m_world;
        private Matrix m_cache;

        private Vector3 m_up;
        private Vector3 m_right;
        private Vector3 m_forward;

        private float m_fov;
        private float m_near;
        private float m_far;
        private float m_aspect;
        private float m_yaw;
        private float m_pitch;
        private float m_pitch_min;
        private float m_pitch_max;
        private Vector3 m_location;

        public Vector3 Up        => m_up;
        public Vector3 Right     => m_right;
        public Vector3 Forward   => m_forward;
        public float Near        => m_near;
        public float Far         => m_far;
        public float Yaw         => m_yaw;
        public float Pitch       => m_pitch;
        public Vector3 Location  => m_location;
        public Matrix Projection => m_projection;
        public Matrix View       => m_view;
        public Matrix World      => m_world;
        public Matrix Cache      => m_cache;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoVoxelCamera( ) { 
            m_projection = Matrix.Identity;
            m_view       = Matrix.Identity;
            m_world      = Matrix.Identity;

            m_up      = new Vector3( 0.0f, 1.0f, 0.0f );
            m_right   = new Vector3( 1.0f, 0.0f, 0.0f );
            m_forward = new Vector3( 0.0f, 0.0f, -1.0f );

            m_fov       = MathHelper.ToRadians( 50 );
            m_near      = 0.1f;
            m_far       = 100.0f;
            m_aspect    = 0.0f;
            m_yaw       = 0.0f;
            m_pitch     = 0.0f;
            m_pitch_min = -MathHelper.ToRadians( 89 );
            m_pitch_max =  MathHelper.ToRadians( 89 );

            m_location = new Vector3(
                ( MonoVoxelEngine.GridSize * MonoVoxelEngine.ChunkSize ) / 2,
                ( MonoVoxelEngine.GridSize * MonoVoxelEngine.ChunkSize ),
                ( MonoVoxelEngine.GridSize * MonoVoxelEngine.ChunkSize ) / 2
            );
        }

        /// <summary>
        /// Rotate pitch.
        /// </summary>
        /// <param name="delta_y" >Delta pitch in radians</param>
        public void RotatePitch( float delta_y ) {
            m_pitch -= delta_y;
            m_pitch = Math.Clamp( m_pitch, m_pitch_min, m_pitch_max );
        }

        /// <summary>
        /// Rotate yaw.
        /// </summary>
        /// <param name="delta_x" >Delta yaw in radians</param>
        public void RotateYaw( float delta_x )
            => m_yaw += delta_x;

        /// <summary>
        /// Rotate pitch and yaw.
        /// </summary>
        /// <param name="rotation" >Rotation for pitch and yaw, vector in radians</param>
        public void Rotate( Vector2 rotation ) {
            RotateYaw( rotation.X );
            RotatePitch( rotation.Y );
        }

        /// <summary>
        /// Rotate pitch and yaw.
        /// </summary>
        /// <param name="yaw" >Delta yaw in radians</param>
        /// <param name="pitch" >Delta pitch in radians</param>
        public void Rotate( float yaw, float pitch ) {
            RotateYaw( yaw );
            RotatePitch( pitch );
        }

        /// <summary>
        /// Move to the left.
        /// </summary>
        /// <param name="velocity" >Movement velocity</param>
        public void MoveLeft( float velocity )
            => m_location -= m_right * velocity;

        /// <summary>
        /// Move to the right.
        /// </summary>
        /// <param name="velocity" >Movement velocity</param>
        public void MoveRight( float velocity )
            => m_location += m_right * velocity;

        /// <summary>
        /// Move up.
        /// </summary>
        /// <param name="velocity" >Movement velocity</param>
        public void MoveUp( float velocity )
            => m_location += m_up * velocity;

        /// <summary>
        /// Move down
        /// </summary>
        /// <param name="velocity" >Movement velocity</param>
        public void MoveDown( float velocity )
            => m_location -= m_up * velocity;

        /// <summary>
        /// Move forward.
        /// </summary>
        /// <param name="velocity" >Movement velocity</param>
        public void MoveForward( float velocity )
            => m_location += m_forward * velocity;

        /// <summary>
        /// Move backward.
        /// </summary>
        /// <param name="velocity" >Movement velocity</param>
        public void MoveBackward( float velocity )
            => m_location -= m_forward * velocity;

        /// <summary>
        /// Update forward vector.
        /// </summary>
        private void UpdateForward( ) {
            m_forward.X = MathF.Cos( m_yaw ) * MathF.Cos( m_pitch );
            m_forward.Y = MathF.Sin( m_pitch );
            m_forward.Z = MathF.Sin( m_yaw ) * MathF.Cos( m_pitch );

            m_forward.Normalize( );
        }

        /// <summary>
        /// Update right vector.
        /// </summary>
        private void UpdateRight( ) {
            m_right = Vector3.Cross( m_forward, new Vector3( 0.0f, 1.0f, 0.0f ) );
            m_right.Normalize( );
        }

        /// <summary>
        /// Update up vector.
        /// </summary>
        private void UpdateUp( ) {
            m_up = Vector3.Cross( m_right, m_forward );
            m_up.Normalize( );
        }

        /// <summary>
        /// Tick camera, update matricies.
        /// </summary>
        /// <param name="device" >Current graphics device instance</param>
        public void Tick( GraphicsDevice device ) {
            m_aspect = (float)device.PresentationParameters.BackBufferWidth / (float)device.PresentationParameters.BackBufferHeight;

            UpdateForward( );
            UpdateRight( );
            UpdateUp( );

            m_projection = Matrix.CreatePerspectiveFieldOfView( m_fov, m_aspect, m_near, m_far );
            m_view       = Matrix.CreateLookAt( m_location, m_location + m_forward, m_up );
            m_world      = Matrix.CreateTranslation( Vector3.Zero );

            Matrix.Multiply( ref m_world, ref m_view, out m_cache );
            Matrix.Multiply( ref m_cache, ref m_projection, out m_cache );
        }

        public Vector2 GetAspect( ) {
            return new(
                2.0f * MathF.Atan( MathF.Tan( m_fov * 0.5f ) * m_aspect ),
                m_fov
            );
        }

        public Vector2 GetHalfAspect( )
            => GetAspect( ) * 0.5f;

    }

}
