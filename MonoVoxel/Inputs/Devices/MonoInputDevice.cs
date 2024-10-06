using Microsoft.Xna.Framework;

namespace MonoVoxel.Inputs.Devices {

    public enum MonoInputStates {

        Pressed = 0,
        Released,
        Down,
        Up

    }

    public interface MonoInputDevice {

        /// <summary>
        /// Tick the current device.
        /// </summary>
        public void Tick( );

        /// <summary>
        /// Evaluate a key for query state.
        /// </summary>
        /// <param name="key" >Query key</param>
        /// <param name="state" >Query key state</param>
        /// <returns>True when key as state value</returns>
        public bool Evaluate( int key, MonoInputStates state );

        /// <summary>
        /// Evaluate axis value.
        /// </summary>
        /// <param name="axis_id" >Index of the axis</param>
        /// <param name="value" >Out axis value as vector</param>
        /// <returns>True for evaluation success</returns>
        public bool Evaluate( int axis_id, out Vector2 value );

        /// <summary>
        /// Get axis value
        /// </summary>
        /// <param name="axis" >Query axis index</param>
        /// <returns>Axis x and y value as vector</returns>
        public Vector2 GetAxis( int axis );

    }

}
