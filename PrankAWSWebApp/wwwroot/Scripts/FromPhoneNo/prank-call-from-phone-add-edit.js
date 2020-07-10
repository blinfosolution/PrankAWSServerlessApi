
function changeCountrycode(_this) {
    var selectedCountryCode = $('option:selected', _this).attr('data-country');

       // $(_this).children("option:selected").attr('data-country');
    $('#CountryCode').val(selectedCountryCode);
}