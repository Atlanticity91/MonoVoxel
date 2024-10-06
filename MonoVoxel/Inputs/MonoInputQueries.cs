using System.Collections.Generic;

namespace MonoVoxel.Inputs {
    
    public class MonoInputQueries {

        private bool m_consumed;
        private List<MonoInputQuery> m_queries;

        public MonoInputQueries( MonoInputQuery[] queries ) {
            m_consumed = false;
            m_queries  = new List<MonoInputQuery>( queries );
        }

        public void Add( MonoInputQuery[] queries ) {
            if ( queries != null )
                m_queries.AddRange( queries );
        }

        public void Tick( ) {
            m_consumed = false;
        }

        public bool Evaluate( List<MonoInputDevice> devices, bool consume ) {
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
