$(function () {
    Global.AntiCSRFHeaders();

    Initialize.Grids('.equal-row .equal-col');
    Initialize.HideMenu('.hide-menu a');
    Initialize.CustomScrollbar($('.chatbox').find('.message-box'), 'minimal-dark');
    Initialize.CustomScrollbar($('.character-page').find('.biography'), 'minimal');
    Initialize.Tooltip($('.character-stats-row').find('.character-stats-name'), '.character-stats-info', 'tooltipster-crescent-dark');
    Initialize.Tooltip($('.nav-top-menu').find('a'), '.top-menu-info', 'tooltipster-crescent-dark');
    Initialize.Tooltip($('.account-info-page').find('.biography').find('.sprite-icon'), '.biography-info', 'tooltipster-crescent-dark');

    Global.GetChatMessages();

    Pages.SetAvatar();
    Pages.SetClass();
    Pages.DropdownUnselected();
    Pages.SetSwitchCheckbox();

    // Temp code
    $battleButton = $('.battle a');
    $battleButton.on('click', function (e) {
        e.preventDefault();

        Battle.UpdateHealth('0');
        Battle.UpdateEnergy('0');
    });
});

/*
$('.equal-col .row').resize(function () {
    
});
*/