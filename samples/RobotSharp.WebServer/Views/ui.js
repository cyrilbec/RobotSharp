'use strict';

var ui = (function() {

    var speed = 65;
    var cameraStep = 10;
    var pan = 0, tilt = 0;

    function handleMovements() {
        $('.move.forward').on('click', function(e) {
            e.preventDefault();
            robot.forward(speed);
        });

        $('.move.reverse').on('click', function (e) {
            e.preventDefault();
            robot.reverse(speed);
        });

        $('.move.left').on('click', function (e) {
            e.preventDefault();
            robot.spinLeft(speed);
        });

        $('.move.right').on('click', function (e) {
            e.preventDefault();
            robot.spinRight(speed);
        });

        $('.move.stop').on('click', function (e) {
            e.preventDefault();
            robot.stop();
        });
    }

    function handleCamera() {
        $('.camera.up').on('click', function (e) {
            e.preventDefault();
            tilt += cameraStep;
            robot.cameraChangeTiltPosition(tilt);
        });

        $('.camera.down').on('click', function (e) {
            e.preventDefault();
            tilt -= cameraStep;
            robot.cameraChangeTiltPosition(tilt);
        });

        $('.camera.left').on('click', function (e) {
            e.preventDefault();
            pan -= cameraStep;
            robot.cameraChangePanPosition(pan);
        });

        $('.camera.right').on('click', function (e) {
            e.preventDefault();
            pan += cameraStep;
            robot.cameraChangePanPosition(pan);
        });
    }

    return {
        init: function() {
            handleMovements();
            handleCamera();
        }
    };

})();