'use strict';

var camera = (function () {

    // private
    var options = {
        debug: true,
        host: 'localhost',
        port: 80,
        frameUpdate: function (data) { console.log('frame received from camera'); }
    };

    var webSocket;

    function log(str) {
        if (options.debug) console.log('[camera]' + str);
    }

    function dispose() {
        webSocket.close();
    }

    var pingBytes = new Uint8Array(3);
    pingBytes[0] = 1;

    function initWebSocket() {

        if (webSocket) dispose();

        var url = 'ws://' + options.host + ':' + options.port + '/camera';
        log('connecting to ' + url);

        webSocket = new WebSocket(url);

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
        webSocket.onmessage = function(evt) {
            log('frame received');
            if (options.frameUpdate) options.frameUpdate(evt.data);
        };
    }

    function ping() {
        if (webSocket && webSocket.readyState === webSocket.OPEN) {
            webSocket.send(pingBytes.buffer);
        }
        else {
            log('cannot ping');
            initWebSocket();
        }
    }

    function setup(host, port, frameUpdate) {
        options.host = host;
        options.port = port;
        options.frameUpdate = frameUpdate;
        initWebSocket();

        // try to ping every 2 seconds
        setInterval(ping, 2000);

        // reconnect every 15 seconds
        setInterval(initWebSocket, 15000);
    }

    // public
    var self = {
        setup: setup
    };

    return self;
})();