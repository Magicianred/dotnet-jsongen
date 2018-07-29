using System;
using System.Collections.Generic;

namespace model
{
    public class Model2
    {
        /// <summary>
        /// test data 1
        /// </summary>
        [Length(3)]
        public string member1 { get; set; }
        /// <summary>
        /// test data 2
        /// </summary>
        [Require]
        [Length(3)]
        public string member2 { get; set; }
    }
}
