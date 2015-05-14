'use strict';

var robot = (function () {
    var operations = { move: 0, cameraMove: 1 };
    var cameraServos = { pan: 0, tilt: 1 };

    var webSocket;
    var connectionOpened = false;

    function setup() {
        webSocket = new WebSocket('ws://localhost:81/pi2golite');
        webSocket.binaryType = 'arraybuffer';

        webSocket.onopen = function (event) {
            connectionOpened = true;
        };
    }

    function dispose() {
        if (connectionOpened)
            webSocket.close();
    }

    function move(leftSpeed, rightSpeed) {
        if (!connectionOpened) return;

        var bytes = new Uint8Array(3);
        bytes[0] = operations.move; // operation type
        bytes[1] = leftSpeed; // left motor speed
        bytes[2] = rightSpeed; // right motor speed
        webSocket.send(bytes.buffer);
    }

    function cameraMove(servo, degrees) {
        var bytes = new Uint8Array(3);
        bytes[0] = operations.cameraMove; // operation type
        bytes[1] = servo; // pan or tilt
        bytes[1] = degrees; // position in degrees
        webSocket.send(bytes.buffer);
    }

    return {
        setup: setup,
        forward: function(speed) {
            move(speed, speed);
        },
        move: move,
        reverse: function(speed) {
            speed *= -1;
            move(speed, speed);
        },
        spinLeft: function(speed) {
            move(speed, speed * -1);
        },
        spinRight: function(speed) {
            move(speed * -1, speed);
        },
        stop: function() {
            move(0, 0);
        },
        cameraChangePanPosition: function(degrees) {
            cameraMove(cameraServos.pan, degrees);
        },
        cameraChangeTiltPosition: function(degrees) {
            cameraMove(cameraServos.tilt, degrees);
        }
    };

})();
