
var _devData = $.connection.devHub;
$(document).ready(function () {
    $.connection.hub.logging = false;
    $.connection.hub.start({ transport: ['webSockets', 'longPolling'] });
    $.connection.hub.start().done(function () {
        console.log("connection started!");
        console.log("Connected, transport = " + $.connection.hub.transport.name);
    });
    $.connection.hub.connectionSlow(function () {
        console.log('We are currently experiencing difficulties with the connection.');
    });
    $.connection.hub.error(function (error) {
        console.log('SignalR error: ' + error);
    });
    _devData.client.stopClient = function () {
        console.log('Client connection stoped');
        $.connection.hub.stop();
    };
});
function update(data) {
    if (data.length > 0) {
        var dData = "";
        for (var i = 0; i < data.length; i++) {
            dData += '<div style="width: 100%; border: 1px solid #CCC; float:left;">';
            dData += '<div style="width: 20%; border-right: 1px solid #CCC; float:left;">' + data[i].CampaignName + '</div>';
            dData += '<div style="width: 20%; border-right: 1px solid #CCC; float:left;">' + data[i].Date + '</div>';
            dData += '<div style="width: 10%; border-right: 1px solid #CCC; float:left;">' + (data[i].Clicks == null ? 0 : data[i].Clicks) + '</div>';
            dData += '<div style="width: 10%; border-right: 1px solid #CCC; float:left;">' + (data[i].Conversions == null ? 0 : data[i].Conversions) + '</div>';
            dData += '<div style="width: 10%; border-right: 1px solid #CCC; float:left;">' + (data[i].Impressions == null ? 0 : data[i].Impressions) + '</div>';
            dData += '<div style="width: 20%;float:left;">' + data[i].AffiliateName + '</div>';
            dData += '</div>';
        }
        $('.row').html(dData);
    }
}
_devData.client.updateData = function (data) {
    update(data);
    $('#divLoading').css('display', 'none');
};