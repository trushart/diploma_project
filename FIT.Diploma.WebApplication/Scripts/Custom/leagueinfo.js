$('#league-select').on('change', function () {
    var selectedLeague = $("#league-select option:selected").val();
    refreshPage(selectedLeague, -1)
});

$('#season-select').on('change', function () {
    var selectedLeague = $("#league-select option:selected").val();
    var selectedSeason = $("#season-select option:selected").val();
    refreshPage(selectedLeague, selectedSeason)
});

function refreshPage(_selectedLeagueId, _selectedSeasonId) {
    window.location.href = '/Home/LeagueInformation?selectedLeagueId=' + _selectedLeagueId + '&selectedSeasonId=' + _selectedSeasonId;
}

function GameRowClicked(_gameId, _team1Id, _team2Id) {
    var selectedSeason = $("#season-select option:selected").val();
    $.ajax({
        //contentType: 'application/json; charset=utf-8',
        //dataType: 'json',
        type: "POST",
        url: "/Home/GetGameStats",
        data: {
            seasonId: selectedSeason,
            gameId: _gameId,
            team1Id: _team1Id,
            team2Id: _team2Id
        },
        success: function (data) {
            $("#game-stats-div").html(data.ViewString);
            $('#link-to-game-stats').click();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}