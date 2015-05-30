$(function () {
    Initialize.Grids('.equal-row .equal-col');
    Initialize.HideMenu('.hide-menu a');

    Manage.SetAvatar();

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