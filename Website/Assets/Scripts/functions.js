﻿var Initialize = {
    Grids: function (selector) {
        $(selector).responsiveEqualHeightGrid();
    },

    HideMenu: function (selector) {
        var menuhidden = false;

        $(selector).click(function (e) {
            e.preventDefault();

            if (menuhidden) {
                $('#main #hiddenSidebar').hide().removeClass('rotate');
                $('#main #sidebar').show(200, function () { $(this).removeClass('rotate') });
                menuhidden = false;
            }
            else {
                $('#main #sidebar').hide().addClass('rotate');
                $('#main #hiddenSidebar').show(300).addClass('rotate');
                menuhidden = true;
            }
            $('#main').toggleClass('no-menu');
        });
    }
};

var Global = {
    AddAntiForgeryToken: function(data) {
        data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
        return data;
    },
    RefreshHeader: function () {
        $('header').load('/Page/HeaderPartial');
    },
    AddAnimation: function ($elem, value) {
        var offsetTop = $elem.offset().top;
        var offsetLeft = $elem.offset().left;
        var divWidth = $elem.outerWidth();
        var offsetRight = $(window).width() - (offsetLeft + divWidth);

        var $updateDiv = $('<div />').addClass('update').css({ top: offsetTop, right: offsetRight });
        if (value > 0) {
            $updateDiv.addClass('positive').html('+' + value);
        }
        else {
            $updateDiv.html(value);
        }
        $updateDiv.animate({ top: offsetTop + 25 }, 500, "linear", function () {
            $(this).animate({ opacity: 0 }, 150, function () {
                $(this).remove();
            });
        })
        $('#global-animations').prepend($updateDiv);
    }
}

var Battle = {
    UpdateHealth: function (actionId) {
        $.ajax({
            type: "post",
            cache: false,
            async: true,
            dataType: "json",
            url: "/Battle/UpdateHealth",
            data: Global.AddAntiForgeryToken({ actionId: actionId }),
            success: function (response) {
                if (response.Success) {
                    $.when(Global.RefreshHeader()).then(function () {
                        var $curHealth = $('#nav-top').find('.health').find('.cur-health');
                        Global.AddAnimation($curHealth, response.CurHealthChange);
                    });
                }
                else {
                    console.log("Failed on UpdateHealth - Success = false");
                }
            },
            error: function () {
                console.log("Failed on UpdateHealth - Ajax failed");
            }
        });
    },
    UpdateEnergy: function (actionId) {
        $.ajax({
            type: "post",
            cache: false,
            async: true,
            dataType: "json",
            url: "/Battle/UpdateEnergy",
            data: Global.AddAntiForgeryToken({ actionId: actionId }),
            success: function (response) {
                if (response.Success) {
                    $.when(Global.RefreshHeader()).then(function () {
                        var $curEnergy = $('#nav-top').find('.energy').find('.cur-energy');
                        Global.AddAnimation($curEnergy, response.CurEnergyChange);
                    });
                }
                else {
                    console.log("Failed on UpdateEnergy - Success = false");
                }
            },
            error: function ()
            {
                console.log("Failed on UpdateEnergy - Ajax failed");
            }
        });
    },
    UpdateUser: function () {
        $.ajax({
            type: "post",
            cache: false,
            async: true,
            dataType: "json",
            url: "/Battle/UpdateUser",
            success: function (response) {
                if (response.Success) {
                    // Do something
                    Global.RefreshHeader();
                }
                else {
                    console.log("Failed on UpdateUser - Success = false");
                }
            },
            error: function () {
                console.log("Failed on UpdateUser - Ajax failed");
            }
        });
    }
}