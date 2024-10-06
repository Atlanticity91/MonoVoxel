using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoVoxel.Inputs.Devices {
    
    public sealed class MonoInputGamepad : MonoInputDevice {

        private PlayerIndex m_player_index;
        private GamePadDeadZone m_dead_zone_mode;
        private GamePadCapabilities m_capabilities;
        private GamePadState m_old_state;
        private GamePadState m_new_state;

        /// <summary>
        /// Constructor
        /// </summary>
        public MonoInputGamepad( PlayerIndex player_index ) {
            m_player_index   = player_index;
            m_dead_zone_mode = GamePadDeadZone.Circular;
        }

        /// <summary>
        /// Tick the keyboard, savind state and query the new.
        /// </summary>
        public void Tick( ) {
            m_capabilities = GamePad.GetCapabilities( m_player_index );

            if ( GetIsValid( ) ) {
                m_old_state = m_new_state;
                m_new_state = GamePad.GetState( m_player_index );
            }
        }

        /// <summary>
        /// Evaluate a key for query state.
        /// </summary>
        /// <param name="key" >Query key</param>
        /// <param name="state" >Query key state</param>
        /// <returns>True when key as state value</returns>
        public bool Evaluate( int key, MonoInputStates state ) {
            var result = false;

            if ( GetIsValid( ) ) {
                switch ( state ) {
                    case MonoInputStates.Pressed  : result = m_old_state.IsButtonUp( (Buttons)key )   && m_new_state.IsButtonDown( (Buttons)key ); break;
                    case MonoInputStates.Released : result = m_old_state.IsButtonDown( (Buttons)key ) && m_new_state.IsButtonUp( (Buttons)key ); break;
                    case MonoInputStates.Down     : result = m_old_state.IsButtonDown( (Buttons)key ) && m_new_state.IsButtonDown( (Buttons)key ); break;
                    case MonoInputStates.Up       : result = m_old_state.IsButtonUp( (Buttons)key )   && m_new_state.IsButtonUp( (Buttons)key ); break;

                    default : break;
                }
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
            value = GetAxis( axis_id );

            return GetIsValid( );
        }

        /// <summary>
        /// Get axis value
        /// </summary>
        /// <param name="axis" >Query axis index</param>
        /// <returns>Axis x and y value as vector</returns>
        public Vector2 GetAxis( int axis ) {
            var result = Vector2.Zero;

            if ( GetIsValid( ) ) {
                if ( axis == 0 )
                    result = m_new_state.ThumbSticks.Left - m_old_state.ThumbSticks.Left;
                else if ( axis == 1 )
                    result = m_new_state.ThumbSticks.Right - m_old_state.ThumbSticks.Right;
                else if ( axis == 2 ) {
                    result = new(
                        m_old_state.Triggers.Left - m_new_state.Triggers.Left,
                        m_old_state.Triggers.Right - m_new_state.Triggers.Right
                    );
                }
            }

            return result;
        }

        /// <summary>
        /// Get if a gamepad is valid aka is connected and it's a gamepad.
        /// </summary>
        /// <returns>True when is valid</returns>
        private bool GetIsValid( )
            => m_capabilities.IsConnected && m_capabilities.GamePadType == GamePadType.GamePad;

    }

}
