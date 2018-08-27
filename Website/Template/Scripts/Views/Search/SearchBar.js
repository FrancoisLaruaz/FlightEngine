var GoToSearchPage = false;

$(document).ready(function () {
    $(".searchbar").fadeIn(500);
    SetSearchBarOnKeyPressed();
    SetSearchBarAutoComplete();
    SetEnterKeyForSearchBar();
    SetSearchResultItems();
    $(".btn-search").click();
});


function SetSearchResultItems() {

    $(".searchItemSuggestion_js").unbind("click");
    $(".searchItemSuggestion_js").on("click", function (e) {
        ShowSpinner();
        e.preventDefault();
        SetSearchResultItem(this);

    });
}

function SetSearchResultItem(element) {
    var SearchId = $(element).data("searchid");
    var UrlToLog = $(element).data("redirecturl");


    if (SearchId > 0 && HasValue(UrlToLog)) {

        $.ajax({
            url: "/Search/SearchItemClicked",
            type: "POST",
            data: { SearchId: SearchId, Url: UrlToLog },
            success: function (data) {
            },
            error: function (xhr, error) {

            }
        });
    }
}


function SetSearchBarOnKeyPressed() {
    $(".btn-extended").unbind("change");
    $(".btn-extended").on("change", function (e) {
        var pattern = $(this).val();
        $('.HiddenSearchTerm_js').val(pattern);
        $(".btn-extended").val(pattern);
    });
}

function SearchIconOnClick(e) {

    var searchBar = $(e).parent();
    $(searchBar).toggleClass('clicked');
    var stage = $(e).parent().parent().parent();
    $(stage).toggleClass('faded');
    var btnextended = $(searchBar).find('.btn-extended');
    if ($(searchBar).hasClass('clicked')) {
        $(btnextended).focus();
    }
}


function SetEnterKeyForSearchBar() {
    $(window).keydown(function (event) {
        if ((event.keyCode == 13)) {
            var hasFocus = $('.btn-extended').is(':focus');
            if (hasFocus && GoToSearchPage) {
                GoToSearch();
                return false;
            }
        }
    });
}

function SetSearchBarAutoComplete() {
    var $searchBars = $('.btn-extended');


    $searchBars.each(function (index, value) {
        $(this).autocomplete({
            minLength: 1,
            source: function (request, response) {

                $('.HiddenSearchTerm_js').val(request.term);
                var clearUrl = "/Search/GetSearchAutocomplete/" + cleanUrlTerm(request.term);
            
                $.ajax({
                    url: clearUrl,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        GoToSearchPage = true;
                        response(data);
                        SetSearchResultItems();
                    }
                })
            },
            select: function (event, ui) {
                var selectedObj = ui.item;
                if (selectedObj.SearchItemType != Constants.SearchItemType.SearchAll && HasValue(selectedObj.Url)) {
                    if (window.location.href != null && window.location.href.split('#')[0].toLowerCase() != GetHomePageUrl().toLowerCase() + selectedObj.Url.split('#')[0].toLowerCase()) {
                        window.location.href = encodeURI(GetHomePageUrl() + selectedObj.Url.trim());
                        ShowSpinner();
                    }
                    else {
                        HideSpinner();
                    }
                    GoToSearchPage = false;
                    return false;
                }
                else {
                    GoToSearchPage = true;
                }

            },
            focus: function (event, ui) {
                var selectedObj = ui.item;


                $(".li_search").find("a").removeClass("focusAutocomplete");
                $(".li_search").find("span").removeClass("focusAutocomplete");
                $(".searchBarSuggestionLinkFocus").removeClass("searchBarSuggestionLinkFocus").addClass("searchBarSuggestionLink");
                if (selectedObj != null && typeof selectedObj !== 'undefined') {
                    var li = $("#li_search_" + ui.item.Id);
                    if ($(li).length > 0) {
                        $(li).find("a").addClass("focusAutocomplete");
                        $(li).find("span").addClass("focusAutocomplete");
                        $(li).find(".searchBarSuggestionLink").removeClass("searchBarSuggestionLink").addClass("searchBarSuggestionLinkFocus");
                    }
                }
                return false; // Prevent the widget from inserting the value.
            }
        });

        $(this).data("ui-autocomplete")._renderItem = function (ul, item) {



            if (item.SearchItemType == Constants.SearchItemType.SearchAll) {
                var $li = $('<li class="searchBarSuggestion li_search" id="li_search_' + item.Id + '">');
                $li.attr('data-value', item.Name);
                $li.append('<span class="searchBarLink searchBarSuggestionLink" onclick="GoToSearch()"  >' + item.Name + '<i class="fa fa-search iconSearchBar"></i></span>');
            }
            else {
                var $li = $('<li class="li_search" id="li_search_' + item.Id + '">');
                $li.attr('data-value', item.Name);
                var ImageSrc = "'" + item.ImageSrc + "'";

                var imageClass = "searchImage";
                if (item.SearchItemType == Constants.SearchItemType.User) {
                    imageClass = "searchBarImageRounded";
                }

                var divImage = '<div style="background-image:url(' + ImageSrc + ')" class="' + imageClass + '"></div>'

                if (HasValue(item.Description)) {
                    $li.append('<a class="searchBarLink searchItemSuggestion_js" title="' + item.Description + '" data-searchid="' + item.SearchId + '"  data-redirecturl="' + item.Url + '" href="' + item.Url + '">');
                }
                else {
                    $li.append('<a class="searchBarLink searchItemSuggestion_js"  data-searchid="' + item.SearchId + '" data-redirecturl="' + item.Url + '" href="' + item.Url + '">');
                }

                $li.find('a').append(divImage).append(DisplayName(item.Name) + ' <span class="searchElement">' + item.SearchItemType + '</span>');
            }

            return $li.data('ui-autocomplete-item', item).addClass('ui-menu-item').appendTo(ul);
        };

    });



}

function GoToSearch() {
    var term = cleanUrlTerm($('.HiddenSearchTerm_js').first().val());
    ShowSpinner();
    window.location.href = encodeURI(GetHomePageUrl() + '/SearchItems/' + term.trim());

}


function cleanUrlTerm(url) {
    var term = ".";
    var replacement = "";
    var result = url.trim().replace(new RegExp(escapeRegExp(term), 'g'), replacement);
    return result;
}


function DisplayName(Name) {
    var result = '';
    if (Name.indexOf('style="color') < 0) {
        Name = replaceAll(Name, '/', '');
        var NameLower = Name.toLowerCase();
        var delimiter = "*|*|*|?@<<?";
        var searchMask = $('#HiddenSearchTerm').val();
        var regEx = new RegExp(searchMask, "ig");
        var replaceMask = delimiter + searchMask + delimiter;
        var resultRegex = Name.replace(regEx, replaceMask);
        var tabRegex = resultRegex.split(delimiter);

        if (tabRegex.length > 1) {
            var letterIndex = 0;
            tabRegex.forEach(function (element) {
                if (element.toLowerCase() == searchMask.toLowerCase()) {
                    for (var i = 0; i < element.length; i++) {
                        var letter = Name.charAt(letterIndex);
                        result = result + '<span style="color:rgba(45,191,183, 1); vertical-align:top;">' + letter + '</span>';
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
    }
    if (!HasValue(result))
        result = Name;

    return '<span class="searchBarName">' + result + '</span>';


}
