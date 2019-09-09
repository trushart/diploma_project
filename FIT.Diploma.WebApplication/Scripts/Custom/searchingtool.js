$(document).ready(function () {
    _minDate = $('#date-range-picker').attr("data-mindate");
    _maxDate = $('#date-range-picker').attr("data-maxdate");

    $('#date-range-picker').daterangepicker({
        locale: {
            format: 'DD.MM.YYYY'
        },
        minDate: _minDate,
        maxDate: _maxDate
    });

    $("#search-button").on("click", function (e) {
        _startDate = $('#date-range-picker').data('daterangepicker').startDate;
        _endDate = $('#date-range-picker').data('daterangepicker').endDate;
        selectedResultFormat = $("#result-format-select option:selected").val();     
        selectedRangeType = $("#range-select option:selected").val();  
        conditions = GetConditions();
        conditionsToSend = [];

        //alert("_startDate: " + _startDate + "   _endDate: " + _endDate + "  selectedResultFormat: " + selectedResultFormat);
        conditionsTxt = "conditions:\n";
        for (index = 0; index < conditions.length; ++index) {
            condition = conditions[index]
            conditionsTxt += "[" + condition.condNumber + "] condTypeId: " + condition.condTypeId + "  selectedValue: " + condition.selectedValue + "\n";
            conditionsToSend.push({
                ConditionTypeId: condition.condTypeId,
                TotalType: condition.totalType,
                SelectedCondition: condition.selectedValue,
            });
        }
        //alert(conditionsTxt);

        var input = {
            RangeTypeId: selectedRangeType,
            DateFrom: _startDate,
            DateTo: _endDate,
            ResultFormatId: selectedResultFormat,
            Conditions: conditionsToSend
        };

        input = JSON.stringify({ 'input': input });

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/Home/GetSearchingToolResult",
            data: input,
            success: function (data) {
                $("#searching-tool-result").html(data.ViewString);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        })
    });

    $("#add-condition").on("click", function (e) {
        $('#link-to-add-condition').click();
    });
});

function GetConditions() {
    conditionTableRaws = [];
    $('#dynamic-table > tbody  > tr').each(function () {
        _condTypeId = $(this).attr("data-condTypeId");
        _condNumber = $(this).attr("data-condNumber");
        _selectedValue = $(this).attr("data-selected-value"); 
        _totalTypeId = $(this).attr("data-total-type");
        if (_condNumber != -1) {
            conditionTableRaws.push({
                condTypeId: _condTypeId,
                condNumber: _condNumber,
                selectedValue: _selectedValue,
                totalType: _totalTypeId
            });
        }        
        //alert("condTypeId: " + _condTypeId + "  condNumber" + _condNumber + "  selectedValue: " + _selectedValue);
    });
    return conditionTableRaws;
}

function DeleteCondition(condNumber) {
    //alert("DeleteCondition number: " + condNumber);    
    conditions = GetConditions().filter(function (obj) {
        return obj.condNumber != condNumber;
    });
    UpdateConditions(conditions);
}

function AddCondition() {
    selectedTeamId = $("#team-selected option:selected").val();   
    selectedCondTypeId = $("#condition-type option:selected").val();
    selectedTotalTypeId = $("#total-type option:selected").val(); 
    selectedTotalNumber = parseInt($("#total-number option:selected").val());
    selectedGamePlace = $("#game-place-selected option:selected").val();
    selectedGameResult = $("#game-result-selected option:selected").val();
    selectedBtts = $('#btts-yes').prop('checked');
    newCondition = {};

    if (selectedCondTypeId == 0) { //GameTotal
        if (selectedTotalTypeId == 2) selectedTotalNumber += 1;
        newCondition = {
            condTypeId: selectedCondTypeId,
            selectedValue: selectedTotalNumber,
            totalType: selectedTotalTypeId
        }
    }
    else if (selectedCondTypeId == 1) { //TeamTotal
        if (selectedTotalTypeId == 2) selectedTotalNumber += 1;
        newCondition = {
            condTypeId: selectedCondTypeId,
            selectedValue: selectedTotalNumber,
            totalType: selectedTotalTypeId
        }
    }
    else if (selectedCondTypeId == 2) { //Btts
        newCondition = {
            condTypeId: selectedCondTypeId,
            selectedValue: selectedBtts,
            totalType: 0
        }
    }
    else if (selectedCondTypeId == 3) { //Team
        newCondition = {
            condTypeId: selectedCondTypeId,
            selectedValue: selectedTeamId,
            totalType: 0
        }
    }
    else if (selectedCondTypeId == 4) { //GameResult
        newCondition = {
            condTypeId: selectedCondTypeId,
            selectedValue: selectedGameResult,
            totalType: 0
        }
    }
    else if (selectedCondTypeId == 5) { //GamePlace
        newCondition = {
            condTypeId: selectedCondTypeId,
            selectedValue: selectedGamePlace,
            totalType: 0
        }
    }
    else { //Error
        alert("Error appears: unknown type of conditions was selected!!!");
    }

    //alert("selectedTeamId: " + selectedTeamId + "  selectedCondTypeId: " + selectedCondTypeId + "  selectedTotalTypeId: " + selectedTotalTypeId
    //    + "  selectedTotalNumber: " + selectedTotalNumber + "  selectedBtts: " + selectedBtts);

    conditions = GetConditions();
    //newCondition.condNumber = 1 + parseInt(conditions[conditions.length - 1].condNumber);
    conditions.push(newCondition);
    UpdateConditions(conditions);
}

function UpdateConditions(conditions) {
    conditionsToSend = [];

    conditionsTxt = "conditions:\n";
    for (index = 0; index < conditions.length; ++index) {
        condition = conditions[index]
        conditionsTxt += "[" + condition.condNumber + "] condTypeId: " + condition.condTypeId + "  selectedValue: " + condition.selectedValue + "\n";
        conditionsToSend.push({
            ConditionTypeId: condition.condTypeId,
            TotalType: condition.totalType,
            SelectedCondition: condition.selectedValue,
        });
    }
    //alert(conditionsTxt);

    conditionsToSend = JSON.stringify({ 'conditions': conditionsToSend });

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: "/Home/GetSearchingToolConditions",
        data: conditionsToSend,
        success: function (data) {
            $("#conditions-table").html(data.ViewString);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}