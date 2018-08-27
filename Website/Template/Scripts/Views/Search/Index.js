$(document).ready(function () {

    $("#searchIcon").unbind("click");
    $("#searchIcon").on("click", function (e) {
        e.preventDefault();
        ShowSpinner();
        RefreshSearch();
    });
    $(".filterCheckbox_js").unbind("change");
    $(".filterCheckbox_js").on("change", function (e) {
        e.preventDefault();
        var type = $(this).data("type");
        var panel = $("#SearchPanel_" + type);
        var checked = $(this).is(':checked');
        var panelExist = $(panel).length > 0 ? true : false;


        if (panelExist && checked) {
            $(panel).fadeIn();
            setTimeout(function () { SetResultCount(); }, 510);
        }
        else if (panelExist && !checked) {
            $(panel).fadeOut();
            setTimeout(function () { SetResultCount(); }, 510);
        }
        else {
            ShowSpinner();
            RefreshSearch();
        }
    });



    SetEnterKeyForSearchPage();
    RefreshSearch();
});



function SetEnterKeyForSearchPage() {
    $(window).keydown(function (event) {
        if ((event.keyCode == 13)) {
            var hasFocus = $('#searchTbx').is(':focus');
            if (hasFocus) {
                ShowSpinner();
                RefreshSearch();
                return false;
            }
        }
    });
}

function RefreshSearch() {

    var _pattern = $("#searchTbx").val();
    var _showUsers = $("#ShowUsers").is(":checked");
    var _showPages = $("#ShowPages").is(":checked");

    var SearchFilter = { Pattern: _pattern, ShowUsers: _showUsers, ShowPages: _showPages };

    $.ajax({
        url: "/Search/_IndexSearchResult",
        type: "GET",
        data: SearchFilter,
        success: function (data) {
            if (data == null || data == Constants.PartialViewResults.UnknownError) {
                notificationKO(Constants.ErrorMessages.UnknownError);
            }
            else {
                $("#searchResultsDiv").fadeOut(500, function () {
                    $("#searchResultsDiv").html(data).fadeIn(500);
                });
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });

}


function SetResultCount() {
    var count = 0;



    $(".SearchTypePanel").each(function (index, value) {
        var type = $(this).data("type");
        var Panel = $("#SearchPanel_" + type);

        if ($(Panel).css('display') != 'none')
            count = count + parseInt($(Panel).find(".HiddenSearchResultsCount").val());
    });

    var textResult = '';
    if (parseInt(count) > 1) {
        textResult = count + " [[[results]]]";
    } else {
        textResult = count + " [[[result]]]";
    }
    $("#searchResultCount").html(textResult);

    if (count == 0) {
        $("#no-resultsFoundDiv").fadeIn();
        $("#spinner").fadeOut();
        HideSpinner();
    }
    else {
        $("#no-resultsFoundDiv").hide();
    }
}


function SetSearchResultsDiv(Type) {

    var Panel = $("#SearchPanel_" + Type);

    var textResult = '';
    var count = $(Panel).find(".HiddenSearchResultsCount").val();

    if (parseInt(count) > Constants.SearchParameters.MaxDisplayedItems) {
        $(Panel).find(".HiddenDisplayedSearchItemCount").val(Constants.SearchParameters.MaxDisplayedItems);
    }
    else {
        $(Panel).find(".HiddenDisplayedSearchItemCount").val(parseInt(count));
    }



    if ($(Panel).find(".viewMoreSearchResultsLink").length > 0) {
        $(Panel).find(".viewMoreSearchResultsLink").unbind("click");
        $(Panel).find(".viewMoreSearchResultsLink").on("click", function (e) {
            e.preventDefault();
            var HiddenDisplayedSearchItemCount = parseInt($(Panel).find(".HiddenDisplayedSearchItemCount").val());
            var MaxDisplayedItems = Constants.SearchParameters.MaxDisplayedItems;
            var HiddenSearchResultsCount = parseInt($(Panel).find(".HiddenSearchResultsCount").val());
            var MaxId = MaxDisplayedItems + HiddenDisplayedSearchItemCount;
            if (MaxId > HiddenSearchResultsCount) {
                MaxId = HiddenSearchResultsCount;
                $(this).hide();
            }

            var i;
            for (i = HiddenDisplayedSearchItemCount + 1; i <= MaxId; i++) {
                var searchItemToSHow = $(Panel).find(".divWrapSearchItem[data-id='" + i + "']");
                if ($(searchItemToSHow).length > 0) {
                    $(searchItemToSHow).removeClass('divWrapSearchItemNotVisible').addClass('divWrapSearchItemVisible');
                    $(searchItemToSHow).fadeIn(500);

                    var nameSearchItemLabel = $(searchItemToSHow).find('.nameSearchItem');
                    $(nameSearchItemLabel).addClass('nameSearchItemVisible')
                }
            }

            $(Panel).find(".HiddenDisplayedSearchItemCount").val(MaxId);
            SetRedirectUrl();
            SetDisplayName();
        });
    }

    SetRedirectUrl();
    SetDisplayName();
    SetFollowBtn(null);
    SetFollowCompanyBtns();
    $("#spinner").fadeOut();
    HideSpinner();
    SetResultCount();
}



function SetFollowCompanyBtns() {
    $(".followedCompanyBtn_js").unbind("click");
    $(".followedCompanyBtn_js").on("click", function (e) {
        e.stopPropagation();
        e.preventDefault();
        var CompanyId = $(this).data("companyid");
        if (CompanyId > 0) {
            $.ajax({
                url: "/Profile/ToggleCompanyFollow",
                type: "POST",
                data: { CompanyId: CompanyId },
                success: function (data) {
                    if (data == null || !data.Result) {

                        if (data.Error != "") {
                            notificationKO(data.Error);
                        }
                        else {
                            notificationKO();
                        }
                        HideSpinner();
                    }
                    else {

                        var BtnTab = $(".followedCompanyBtn_js[data-companyid='" + CompanyId + "']");

                        if (BtnTab.length > 0) {
                            var Btn = $(BtnTab).first();
                            if (data.CompanyFollowed) {
                                $(Btn).html('[[[Following]]]').addClass('FFgreenButton').removeClass('FForangeButton');
                            }
                            else {
                                $(Btn).html('[[[Follow]]]').addClass('FForangeButton').removeClass('FFgreenButton');;
                            }
                        }

                    }
                },
                error: function (xhr, error) {
                    notificationKO(Constants.ErrorMessages.UnknownError);
                }
            });
        }
        else {
            notificationKO(Constants.ErrorMessages.UnknownError);
        }
    });
}

function SetRedirectUrl() {
    $(".divWrapSearchItemVisible").unbind("click");
    $(".divWrapSearchItemVisible").on("click", function (e) {
        e.preventDefault();
        ShowSpinner();
        SetSearchResultItem(this);
        var urlToGo = $(this).data("redirecturl");
        if (urlToGo != null) {
            window.location.href = urlToGo;
        }
    });
}


function SetDisplayName() {
    $('.nameSearchItemVisible').each(function (index, value) {
        var Name = $(this).html();
        if (Name.indexOf('style="color') < 0) {
            var NameLower = Name.toLowerCase();
            Name = replaceAll(Name, '/', '');
            var delimiter = "*|*|*|?@<<?";
            var searchMask = $('#searchTbx').val();
            var regEx = new RegExp(searchMask, "ig");
            var replaceMask = delimiter + searchMask + delimiter;
            var resultRegex = Name.replace(regEx, replaceMask);
            var tabRegex = resultRegex.split(delimiter);

            if (tabRegex.length > 1) {
                var result = '';
                var letterIndex = 0;
                tabRegex.forEach(function (element) {
                    if (element.toLowerCase() == searchMask.toLowerCase()) {
                        for (var i = 0; i < element.length; i++) {
                            var letter = Name.charAt(letterIndex);
                            result = result + '<span style="color:rgba(45,191,183, 1);" class="nameSearchItem">' + letter + '</span>';
                            letterIndex++;
                        }
                    }
                    else {
                        result = result + element;
                        letterIndex = letterIndex + element.length;
                    }
                });

            }
            else {
                result = Name;
            }

            if (!HasValue(result))
                result = Name;
            $(this).html(result);
        }
    });
}
