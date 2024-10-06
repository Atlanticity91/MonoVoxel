namespace MonoVoxel.Inputs.Queries {
    
    public struct MonoInputAxis {

        public int Device;
        public int AxisID;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device" >Index of the device</param>
        /// <param name="axis_id" >Index of the device axis</param>
        public MonoInputAxis( int device, int axis_id ) {
            Device = device;
            AxisID = axis_id;
        }

    }

}
