using System.Collections.Generic;
using MonoVoxel.Inputs.Devices;

namespace MonoVoxel.Inputs.Queries {

    public class MonoInputQueries {

        private bool m_consumed;
        private List<MonoInputQuery> m_queries;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queries" >Array of default input queries</param>
        public MonoInputQueries( MonoInputQuery[] queries ) {
            m_consumed = false;
            m_queries  = new List<MonoInputQuery>( queries );
        }

        /// <summary>
        /// Add input query.
        /// </summary>
        /// <param name="queries" >Array of new input query.</param>
        public void Add( MonoInputQuery[] queries ) {
            if ( queries != null )
                m_queries.AddRange( queries );
        }

        /// <summary>
        /// Tick the query, reset query consumtion.
        /// </summary>
        public void Tick( )
            => m_consumed = false;

        /// <summary>
        /// Evaluate the input query.
        /// </summary>
        /// <param name="devices" >Array of input device</param>
        /// <param name="consume" >Consumtion value for the input query</param>
        /// <returns>True when query is </returns>
        public bool Evaluate( MonoInputDevice[] devices, bool consume ) {
            var result = false;

            if ( !m_consumed ) {
                foreach ( var query in m_queries ) {
                    result = devices[ query.Device ].Evaluate( query.Key, query.State );

                    if ( result ) {
                        if ( consume )
                            m_consumed = consume;

                        break;
                    }
                }
            }

            return result;
        }

    }

}
