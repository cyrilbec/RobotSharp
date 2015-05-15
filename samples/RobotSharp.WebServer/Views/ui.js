'use strict';

var ui = (function() {

    var speed = 65;

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

    return {
        init: function() {
            handleMovements();
        }
    };

})();