using Microsoft.Xna.Framework.Input;
using MonoVoxel.Inputs.Devices;

namespace MonoVoxel.Inputs.Queries {

    public struct MonoInputQuery {

        public int Device;
        public int Key;
        public MonoInputStates State;

        /// <summary>
        /// Constructor for Keys.
        /// </summary>
        /// <param name="key" >Query input key</param>
        /// <param name="state" >Query input state</param>
        public MonoInputQuery( Keys key, MonoInputStates state ) {
            Device = 0;
            Key    = (int)key;
            State  = state;
        }

        /// <summary>
        /// Constructor for mouse buttons.
        /// </summary>
        /// <param name="button" >Query input button</param>
        /// <param name="state" >Query input state</param>
        public MonoInputQuery( MonoInputMouseButtons button, MonoInputStates state ) {
            Device = 1;
            Key    = (int)button;
            State  = state;
        }

        /// <summary>
        /// Constructor for gamepad buttons.
        /// </summary>
        /// <param name="button" >Query input button</param>
        /// <param name="state" >Query input state</param>
        public MonoInputQuery( Buttons button, MonoInputStates state ) {
            Device = 2;
            Key    = (int)button;
            State  = state;
        }

    }

}
