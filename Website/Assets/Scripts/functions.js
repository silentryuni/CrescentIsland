var Initialize = {
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
    },
    CustomScrollbar: function ($selector) {
        $selector.mCustomScrollbar({
            mouseWheel: {
                enable: true,
                preventDefault: true,
                scrollAmount: 200
            },
            theme: 'minimal-dark'
        });
    }
};

var Global = {
    AntiCSRFHeaders: function() {
        var token = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
        $.ajaxPrefilter(function (options, originalOptions) {
            if (options.type.toUpperCase() == "POST") {
                options.data = $.param($.extend(originalOptions.data, { __RequestVerificationToken: token }));
            }
        });
        // If posting FormData, this is required:
        // options.data.append("__RequestVerificationToken", token);  
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
    },
    ChatBox: function (username) {
        // Reference the auto-generated proxy for the hub.

        var chat = $.connection.chatHub;
        // Create a function that the hub can call back to display messages.
        chat.client.addMessage = function (id, name, message, timestamp, role) {
            // Add the message to the page.
            var buttons = '';
            if ($('.admin-medal').length > 0) {
                buttons = '<span class="admin-buttons"> <a href="#" onclick="return Global.DeleteChatMessage(this);">[D]</a> <a href="#" onclick="return Global.BanChatUser(this);">[B]</a></span>';
            }

            $('#discussion').prepend('<li id="' + id + '"><span class="role' + role + '">' + Global.HtmlEncode(name)
                + ': </span><span class="chat-time">' + timestamp + '</span>' + buttons + '<br /><span class="chat-message">'
                + Global.HtmlEncode(message) + '</span></li>');

            if ($('#discussion li').length > 50) {
                $('#discussion li').last().remove();
            }
        };
        chat.client.removeMessage = function (id) {
            $('#discussion li#' + id).remove();
        };
        chat.client.lockout = function () {
            $.ajax({
                type: "post",
                cache: false,
                dataType: "json",
                url: "/User/Lockout",
                success: function (response) {
                    window.location = response;
                },
                error: function () {
                    console.log('Failed on lockout - Ajax fail');
                }
            });
        };

        // Start the connection.
        $.connection.hub.start().done(function () {
            if (username) {
                $('#sendmessage').click(function () {
                    var textvalue = $('#message').val().trim();
                    if (textvalue) {
                        chat.server.send(username, textvalue);
                    }
                    // Clear text box and reset focus for next comment.
                    $('#message').val('').focus();
                });
            } else {
                $('#sendmessage, #message').remove();
            }
        });

        $('#message').keypress(function (e) {
            if (e.which == 13) {
                $('#sendmessage').click();
                return false;
            }
        });
    },
    DeleteChatMessage: function (elem) {
        if (confirm('Are you sure you want to delete?')) {
            var msgid = $(elem).closest('li').attr('id');

            var chat = $.connection.chatHub;
            chat.server.delete(msgid);
        }

        return false;
    },
    BanChatUser: function (elem) {
        if (confirm('Are you sure you want to ban?')) {
            var msgid = $(elem).closest('li').attr('id');

            var chat = $.connection.chatHub;
            chat.server.ban(msgid);
        }

        return false;
    },
    GetChatMessages: function () {
        $.ajax({
            type: "POST",
            cache: false,
            async: true,
            dataType: "json",
            url: "/Chat/GetMessages",
            success: function (response) {
                $.each(response, function () {
                    $('#discussion').html('');
                    $('#discussion').prepend(response);
                });
            },
            error: function () {
                
            }
        });
    },
    HtmlEncode: function (value) {
        return String(value)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    }
}

var Pages = {
    SetAvatar: function () {
        $('.change-avatar .avatar-selection').click(function () {
            $('.change-avatar .avatar-selection').removeClass('selected');
            $(this).addClass('selected');
            var selectedSrc = $(this).find('img').attr('src');

            $('.selected-avatar').show().find('img').attr('src', selectedSrc);
            $('#SelectedAvatar').val(selectedSrc);
        });
    },
    SetClass: function () {
        // Sets selection initially based on checked radio button
        var $checkedDiv = $('.class-selection').find('input[type=radio]:checked').prev();
        if ($checkedDiv.length > 0) {
            var selectedClass = $checkedDiv.data('class');
            $('.class-selection .class-button').removeClass('selected');
            $($checkedDiv).addClass('selected');
            $('.class-selected > div').removeClass('selected');
            $('.class-selected').find(selectedClass).addClass('selected');
        }


        $('.class-selection').find('.class-button').click(function () {
            $('.class-selection .class-button').removeClass('selected');
            $(this).addClass('selected');
            var selectedClass = $(this).data('class');

            $('.class-selection input').removeAttr('checked');
            $(this).next().attr('checked', 'checked')
            $('.class-selected > div').removeClass('selected');
            $('.class-selected').find(selectedClass).addClass('selected');
        });
    },
    StatsTooltip: function () {
        $('.character-stats-row .character-stats-name').each(function () {
            $(this).tooltipster({
                animation: 'grow',
                content: $(this).find('.character-stats-info').html(),
                contentAsHTML: true,
                delay: 200,
                position: 'top',
                theme: 'tooltipster-crescent'
            });
        });
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
            data: { actionId: actionId },
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
            data: { actionId: actionId },
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
    }
}