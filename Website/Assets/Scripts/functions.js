var Initialize = {
    Grids: function (selector) {
        // Initializes Grids plugin
        $(selector).responsiveEqualHeightGrid();
    },
    HideMenu: function (selector) {
        // Initializes the hide menu feature
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
    CustomScrollbar: function ($selector, theme) {
        // Initializes mCustomScrollbar plugin
        $selector.mCustomScrollbar({
            mouseWheel: {
                enable: true,
                preventDefault: true,
                scrollAmount: 200
            },
            theme: theme
        });
    },
    Tooltip: function ($selector, infoElem, theme) {
        // Add tooltips on stat names, content is gotten from inner div with character-stats-info class
        $selector.each(function () {
            $(this).tooltipster({
                animation: 'grow',
                content: $(this).find(infoElem).html(),
                contentAsHTML: true,
                delay: 200,
                position: 'top',
                theme: theme
            });
        });
    },
};

var Global = {
    AntiCSRFHeaders: function () {
        // Adds anti-forgery token to each ajax post request
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
        // Reloads header partial
        $('header').load('/Page/HeaderPartial');
    },
    AddAnimation: function ($elem, value) {
        // Animates value change on a certain element
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
            // Add admin buttons if admin
            var buttons = '';
            if ($('.admin-medal').length > 0) {
                buttons = '<span class="admin-buttons"> <a href="#" onclick="return Global.DeleteChatMessage(this);">[D]</a> <a href="#" onclick="return Global.BanChatUser(this);">[B]</a></span>';
            }

            // Add the message to the page.
            $('#discussion').prepend('<li id="' + id + '"><a href="/Character/' + Global.HtmlEncode(name) + '"><span class="role' + role + '">' + Global.HtmlEncode(name)
                + '</a>: </span><span class="chat-time">' + timestamp + '</span>' + buttons + '<br /><span class="chat-message">'
                + Global.HtmlEncode(message) + '</span></li>');

            // Removes oldest message if more than 50 messages are present
            if ($('#discussion li').length > 50) {
                $('#discussion li').last().remove();
            }
        };
        // Triggered when admin removes a message
        chat.client.removeMessage = function (id) {
            $('#discussion li#' + id).remove();
        };
        // Triggered when admin bans an user
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
            // Trigger send message when send button is clicked
            $('#sendmessage').click(function () {
                var textvalue = $('#message').val().trim();
                if (textvalue) {
                    chat.server.send(username, textvalue);
                }
                // Clear text box and reset focus for next comment.
                $('#message').val('').focus();
            });
        });

        // Trigger send message when enter is pressed on textbox
        $('#message').keypress(function (e) {
            if (e.which == 13) {
                $('#sendmessage').click();
                return false;
            }
        });
    },
    DeleteChatMessage: function (elem) {
        // Deletes selected message
        if (confirm('Are you sure you want to delete?')) {
            var msgid = $(elem).closest('li').attr('id');

            var chat = $.connection.chatHub;
            chat.server.delete(msgid);
        }

        return false;
    },
    BanChatUser: function (elem) {
        // Bans user of selected message
        if (confirm('Are you sure you want to ban?')) {
            var msgid = $(elem).closest('li').attr('id');

            var chat = $.connection.chatHub;
            chat.server.ban(msgid);
        }

        return false;
    },
    GetChatMessages: function () {
        // Requests latest chat messages from database
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
        // Html encodes string
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
        // Sets selected class to clicked avatar and adds avatar to selected image source
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
        var modelValue = $('.class-selection').find('input.model-value').val();
        var $checkedDiv = $('.enum' + modelValue);
        if ($checkedDiv.length > 0) {
            var selectedClass = $checkedDiv.data('class');

            $('.class-selection .class-button').removeClass('selected');
            $($checkedDiv).addClass('selected');
            $('.class-selected > div').removeClass('selected');
            $('.class-selected').find(selectedClass).addClass('selected');
        }

        // Sets selection every time any radio button is clicked
        $('.class-selection').find('.class-button').click(function () {
            $('.class-selection .class-button').removeClass('selected');
            $(this).addClass('selected');
            var selectedClass = $(this).data('class');
            var modelValue = $(this).data('value');

            $('.class-selection').find('input.model-value').val(modelValue);
            $('.class-selected > div').removeClass('selected');
            $('.class-selected').find(selectedClass).addClass('selected');
        });
    },
    DropdownUnselected: function () {
        // Initially check if selected option is default
        $('select').each(function () {
            if ($(this)[0].selectedIndex == 0) {
                $(this).addClass('first-option');
            }
            else {
                $(this).removeClass('first-option');
            }
        });

        // Checks every time option is changed
        $('select').change(function () {
            if ($(this)[0].selectedIndex == 0) {
                $(this).addClass('first-option');
            }
            else {
                $(this).removeClass('first-option');
            }
        });
    },
    SetSwitchCheckbox: function () {
        $('.switch-checkbox').find('label').click(function () {
            $(this).next().val(!$(this).prev()[0].checked);
        });
    }
}

var Battle = {
    UpdateHealth: function (actionId) {
        // Updates health depending on which action triggers it
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
        // Updates energy depending on which action triggers it
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