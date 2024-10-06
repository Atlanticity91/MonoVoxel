using Microsoft.Xna.Framework;
using MonoVoxel.Inputs.Devices;

namespace MonoVoxel.Inputs {

    public sealed class MonoInputGamepadManager : MonoInputDevice {

        private const int MaxGamepad = (int)PlayerIndex.Four + 1;

        private MonoInputGamepad[] m_gamepads;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoInputGamepadManager( ) { 
            m_gamepads = new MonoInputGamepad[ MaxGamepad ];

            for ( int player_id = 0; player_id < MaxGamepad; player_id++ )
                m_gamepads[ player_id ] = new MonoInputGamepad( (PlayerIndex)player_id );
        }

        /// <summary>
        /// Tick gamepad manager.
        /// </summary>
        public void Tick( ) {
            foreach ( var gamepad in m_gamepads )
                gamepad.Tick( );
        }

        /// <summary>
        /// Evaluate a key for query state.
        /// </summary>
        /// <param name="key" >Query key</param>
        /// <param name="state" >Query key state</param>
        /// <returns>True when key as state value</returns>
        public bool Evaluate( int key, MonoInputStates state ) {
            var result = false;

            foreach ( var gamepad in m_gamepads ) {
                result = gamepad.Evaluate( key, state );

                if ( result )
                    break;
            }

            return result;
        }

        /// <summary>
        /// Evaluate axis value.
        /// </summary>
        /// <param name="axis_id" >Index of the axis</param>
        /// <param name="value" >Out axis value as vector</param>
        /// <returns>True for evaluation success</returns>
        public bool Evaluate( int axis_id, out Vector2 value ) {
            var result = false;

            value = Vector2.Zero;

            foreach ( var gamepad in m_gamepads ) {
                result = gamepad.Evaluate( axis_id, out value );

                if ( result )
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get axis value
        /// </summary>
        /// <param name="axis" >Query axis index</param>
        /// <returns>Axis x and y value as vector</returns>
        public Vector2 GetAxis( int axis ) {
            var result = Vector2.Zero;

            foreach ( var gamepad in m_gamepads ) {
                result = gamepad.GetAxis( axis );

                if ( result != Vector2.Zero )
                    break;
            }

            return result;
        }

        
    }

}
