'use strict';

var host = '192.168.0.6';
var port = 81;

ui.init();
robot.setup(host, port);

var cameraFrameUpdate = function(data) {
    $('#camera').attr('src', 'data:image/png;base64,' + data);
};
camera.setup(host, port, cameraFrameUpdate);