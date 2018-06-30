/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

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

    public class FormOutput
    {
        // input: team, channel, app, clone params, input text, user id
        // groups: displayName, mailNickname, description, visibility

        public string Action { get; set; }
        public string ButtonLabel { get; set; } = "submit";

        public string UserUpn { get; set; }


        // Team list

        public bool ShowTeamOutput { get; set; } = false;
        public bool ShowTeamDropdown { get; set; } = false;

        public Team[] Teams { get; set; } // output
        public IEnumerable<SelectListItem> TeamItems => Teams.Select(t => new SelectListItem() { Text = t.displayName, Value = t.id });

        [Display(Name = "Team")]
        public string SelectedTeam { get; set; }  // input



    }
}