using Microsoft.Xna.Framework;
using MonoVoxel.Inputs.Devices;
using System.Collections.Generic;

namespace MonoVoxel.Inputs.Queries {
    
    public sealed class MonoInputAxisQueries {

        private List<MonoInputAxis> m_queries;

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="queries" >Default input axis queries</param>
        public MonoInputAxisQueries( MonoInputAxis[] queries ) 
            => m_queries = new List<MonoInputAxis>( queries );

        /// <summary>
        /// Add input axis query.
        /// </summary>
        /// <param name="queries" >Input axis queries to add</param>
        public void Add( MonoInputAxis[] queries ) {
            if ( queries != null )
                m_queries.AddRange( queries );
        }

        /// <summary>
        /// Evaluate the input axis value.
        /// </summary>
        /// <param name="devices" >Device array.</param>
        /// <returns>Axis value for x and y as vector</returns>
        public Vector2 Evaluate( MonoInputDevice[] devices ) {
            var value = Vector2.Zero;

            foreach ( var query in m_queries ) {
                if ( devices[ query.Device ].Evaluate( query.AxisID, out value ) )
                    break;
            }

            return value;
        }

    }

}
