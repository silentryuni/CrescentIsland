$(function () {
    Initialize.Grids('.equal-row .equal-col');
    Initialize.HideMenu('.hide-menu a');

    $battleButton = $('.battle a');
    $battleButton.on('click', function (e) {
        e.preventDefault();

        Battle.UpdateHealth('0');
        Battle.UpdateEnergy('0');
    });

    $('.nav-character a').click(function (e) {
        e.preventDefault();

        Battle.UpdateUser();
    });
});