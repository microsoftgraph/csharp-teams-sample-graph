/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/


using System.Collections.Generic;
using System.Linq;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{

    
    // View model to display a collection of one or more entities returned from the Microsoft Graph. 
    public class ResultsViewModel
    {
        // The list of entities to display.
        public IEnumerable<ResultsItem> Items { get; set; }
        public ResultsViewModel()
        {
            Items = Enumerable.Empty<ResultsItem>();
        }
    }
}