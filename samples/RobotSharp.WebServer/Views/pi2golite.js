'use strict';

var robot = (function () {

    // private
    var options = {
        debug: true,
        host: 'localhost',
        port: 80
    };
    var operations = { move: 0, cameraMove: 1, ping: 2 };
    var cameraServos = { pan: 0, tilt: 1 };

    var webSocket;

    function log(str) {
        if (options.debug) console.log(str);
    }

    var pingBytes = new Uint8Array(3);
    pingBytes[0] = operations.ping;

    function ping() {
        if (webSocket && webSocket.readyState === webSocket.OPEN) {
            log('ping');
            webSocket.send(pingBytes.buffer);
        }
        else {
            log('cannot ping');
        }
    }

    function initWebSocket() {

        var url = 'ws://' + options.host + ':' + options.port + '/pi2golite';
        log('connecting to ' + url);

        webSocket = new WebSocket(url);
        webSocket.binaryType = 'arraybuffer';

        webSocket.onopen = function () {
            log('web socket opened');
        };
        webSocket.onerror = function () {
            log('web socket error');
        };
        webSocket.onclose = function () {
            log('web socket closed');

            // trying de reconnect
            setTimeout(initWebSocket, 5000);
        };
    }

    function setup(host, port) {
        options.host = host;
        options.port = port;
        initWebSocket();

        // try to ping every 2 seconds
        setInterval(ping, 3000);
    }

    function dispose() {
        webSocket.close();
    }

    function move(leftSpeed, rightSpeed) {
        log('move left speed=' + leftSpeed + ' / right speed=' + rightSpeed);

        var bytes = new Uint8Array(3);
        bytes[0] = operations.move; // operation type
        bytes[1] = leftSpeed; // left motor speed
        bytes[2] = rightSpeed; // right motor speed
        webSocket.send(bytes.buffer);
    }

    function cameraMove(servo, degrees) {
        log('camera move servo=' + servo + ' / degrees=' + degrees);

        var bytes = new Uint8Array(3);
        bytes[0] = operations.cameraMove; // operation type
        bytes[1] = servo; // pan or tilt
        bytes[1] = degrees; // position in degrees
        webSocket.send(bytes.buffer);
    }

    // public
    var self = {
        setup: setup,
        forward: function (speed) {
            move(speed, speed);
        },
        move: move,
        reverse: function (speed) {
            speed *= -1;
            move(speed, speed);
        },
        spinLeft: function (speed) {
            move(speed * -1, speed);
        },
        spinRight: function (speed) {
            move(speed, speed * -1);
        },
        stop: function () {
            move(0, 0);
        },
        cameraChangePanPosition: function (degrees) {
            cameraMove(cameraServos.pan, degrees);
        },
        cameraChangeTiltPosition: function (degrees) {
            cameraMove(cameraServos.tilt, degrees);
        }
    };

    return self;
})();