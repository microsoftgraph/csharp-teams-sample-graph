/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/


using System;
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
        public string ButtonLabel { get; set; } 

        public string UserUpn { get; set; } // aka /me

        [Display(Name = "Name")]
        public string NameInput { get; set; }

        [Display(Name = "Description")]
        public string DescriptionInput { get; set; }

        [Display(Name = "Message Body")]
        public string MessageBodyInput { get; set; }

        [Display(Name = "Display Name")]
        public String DisplayNameInput { get; set; }

        [Display(Name = "Mail Nickname")]
        public String MailNicknameInput { get; set; }

        public string SuccessMessage { get; set; }

        // Team list

        public bool ShowTeamOutput { get; set; } = false;
        public bool ShowTeamDropdown { get; set; } = false;
        public bool ShowGroupDropdown { get; set; } = false;

        public Team[] Teams { get; set; } // output
        public Group[] Groups { get; set; } // output

        [Display(Name = "Team")]
        public string SelectedTeam { get; set; }  // input
        [Display(Name = "Group")]
        public string SelectedGroup { get; set; }  // input


        // Group/team create
        public bool ShowDisplayNameInput { get; set; } = false;
        public bool ShowMailNicknameInput { get; set; } = false;


        // Channel list

        public bool ShowChannelOutput { get; set; } = false;
        public bool ShowChannelDropdown { get; set; } = false;
        public bool ShowNameInput { get; set; } = false;
        public bool ShowDescriptionInput { get; set; } = false;
        public bool ShowMessageBodyInput { get; set; } = false;

        public Channel[] Channels { get; set; } // output

        [Display(Name = "Channel")]
        public string SelectedChannel{ get; set; }  // input

        // app list

        public bool ShowAppOutput { get; set; } = false;
        public bool ShowAppDropdown { get; set; } = false;

        public TeamsApp[] Apps { get; set; } // output

        [Display(Name = "User principal name")]
        public string UpnInput { get; set; }
        public bool ShowUpnInput { get; set; } = false;

        [Display(Name = "App")]
        public string SelectedApp { get; set; }  // input
    }
}