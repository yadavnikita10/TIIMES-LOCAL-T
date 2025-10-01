function CommonajaxCallForTimeSheet(arrayOfTimesheet) {
    debugger;
    $.ajax({
        url: '../VisitReport/CheckCondition',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ matchingData: arrayOfTimesheet }),
        async: false,
        success: function (response) {
            debugger;
            if (!!response && response.success != "") {
                if (response.success && response.success.includes('Greater Than 24')) {
                    alert(response.success);
                    $("#setvalue").val('true');
                    return false;
                }
            }
            return true;
        },
        error: function (xhr, status, error) {
            console.error('Error sending data: ', error);
            return false;
        }
    });
}
function ValidateTimeSheet(totalStartTime, totalEndTime, totalTravelTime) {
    debugger;

    if (totalStartTime > 0 || totalEndTime > 0 || totalTravelTime > 0) {
        let allowlessthan24 = parseFloat(totalStartTime + totalEndTime + totalTravelTime);

        if (allowlessthan24 > 24) {
            alert("greater than 24");
            return false;
        }
    }
    else {
        alert("Values must be greater than 0!");
        return false;
    }
    return true;
}