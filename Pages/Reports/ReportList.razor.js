export function embedReport(containerId, reportId, embedUrl, token) {

    var reportContainer = document.getElementById(containerId);

    var models = window["powerbi-client"].models
    var config = {
        type: "report",
        id: reportId,
        embedUrl: embedUrl,
        accessToken: token,
        permissions: models.Permissions.All,
        tokenType: models.TokenType.Embed,
        viewMode: models.ViewMode.View,
        settings: {
            panes: {
                filters: { expanded: false, visible: false },
                pageNavigation: { visible: true }
            }
        }
    }
    var report = powerbi.embed(reportContainer, config);

    var heightBuffer = 32;

    var newHeight = $(window).height() - ($("header").height() + heightBuffer);
    $("#" + containerId).height(newHeight);
    $(window).resize(() => {
        var newHeight = $(window).height() - ($("header").height() + heightBuffer);
        $("#" + containerId).height(newHeight);
    });
}