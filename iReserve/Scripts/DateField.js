
$(function () {
    $(".pastDates").datepicker({ maxDate: 0 });
    $(".futureDates").datepicker({ minDate: +1, maxDate: +60 });
    $(".ui-datepicker").css("font-size", "50%");
});