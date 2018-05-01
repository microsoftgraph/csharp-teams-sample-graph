var grapchClient = {

    getChannels: function () {

        var teamId = $('#TeamId').val();
        var userId = $('#UserId').val();



        $.ajax({
            url: "/Home/",
            method: "POST",
            data: { 'team-id': teamId, 'user-id': userId },
            success: function (result) {
                $("#ChannelsResult").html(result);
            }

        });
    },

    





}