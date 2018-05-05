var grapchClient = {

    getChannels: function () {

        var teamId = $('#TeamId').val();
        var userId = $('#UserId').val();



        $.ajax({
            url: "/Home/GetChannels",
            method: "POST",
            data: { 'team-id': teamId, 'user-id': userId },
            success: function (result) {
                $("#ChannelsResult").html(result);
            }

        });
    },

    onSuccess: function (result) {
        $('#MyTeamResult').html(result);
        $('#MyTeamResult').show();
    }





}