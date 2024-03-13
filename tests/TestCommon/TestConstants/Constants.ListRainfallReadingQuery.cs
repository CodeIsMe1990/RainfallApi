using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCommon.TestConstants;

/// <summary>
/// Partial class containing constants related to the list rainfall reading query.
/// </summary>
public static partial class Constants
{
    /// <summary>
    /// Constants related to the list rainfall reading query.
    /// </summary>
    public static class ListRainfallReadingQuery
    {
        /// <summary>
        /// The lower bound for the count parameter in the list rainfall reading query.
        /// </summary>
        public static readonly int CountLowerBound = 1;

        /// <summary>
        /// The upper bound for the count parameter in the list rainfall reading query.
        /// </summary>
        public static readonly int CountUpperBound = 100;
    }
}