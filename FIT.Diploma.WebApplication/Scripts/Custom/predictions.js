$('#league-select').on('change', function () {
    var selectedLeague = $("#league-select option:selected").val();
    refreshPage(selectedLeague, -1, 1)
});

$('#season-select').on('change', function () {
    var selectedLeague = $("#league-select option:selected").val();
    var selectedSeason = $("#season-select option:selected").val();
    var selectedType = $("#predictions-select option:selected").val();
    refreshPage(selectedLeague, selectedSeason, selectedType)
});

$('#predictions-select').on('change', function () {
    var selectedLeague = $("#league-select option:selected").val();
    var selectedSeason = $("#season-select option:selected").val();
    var selectedType = $("#predictions-select option:selected").val();
    refreshPage(selectedLeague, selectedSeason, selectedType)
});

function refreshPage(_selectedLeagueId, _selectedSeasonId, _type) {
    window.location.href = '/Home/Predictions?selectedLeagueId=' + _selectedLeagueId
        + '&selectedSeasonId=' + _selectedSeasonId + '&type=' + _type;
}