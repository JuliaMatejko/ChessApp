function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.id);
}

function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("text");
    ev.target.className = document.getElementById(data).className;
    document.getElementById(data).className = "";
}

function flipTheBoard() {
    $(".board").toggleClass("flipped");
    if ($(".board").hasClass("flipped")) {
        $(".player-one-area").addClass("order-0");
        $(".player-one-avatar-area").addClass("order-0");
        $(".player-one-clock").addClass("order-1");
        $(".player-two-area").addClass("order-1");
        $(".player-two-avatar-area").addClass("order-1");
        $(".player-two-clock").addClass("order-0");

        $(".player-one-area-sm").addClass("order-0");
        $(".board").addClass("order-1");
        $(".player-two-area-sm").addClass("order-2");
        $(".buttons-and-moves-area").addClass("order-3");

        $(".coords").html('<text x="0.75" y="3.5" font-size="2.8" class="coord-light">1</text><text x="0.75" y="15.75" font-size="2.8" class="coord-dark">2</text><text x="0.75" y="28.25" font-size="2.8" class="coord-light">3</text><text x="0.75" y="40.75" font-size="2.8" class="coord-dark">4</text><text x="0.75" y="53.25" font-size="2.8" class="coord-light">5</text><text x="0.75" y="65.75" font-size="2.8" class="coord-dark">6</text><text x="0.75" y="78.25" font-size="2.8" class="coord-light">7</text><text x="0.75" y="90.75" font-size="2.8" class="coord-dark">8</text><text x="10" y="99" font-size="2.8" class="coord-dark">h</text><text x="22.5" y="99" font-size="2.8" class="coord-light">g</text><text x="35" y="99" font-size="2.8" class="coord-dark">f</text><text x="47.5" y="99" font-size="2.8" class="coord-light">e</text><text x="60" y="99" font-size="2.8" class="coord-dark">d</text><text x="72.5" y="99" font-size="2.8" class="coord-light">c</text><text x="85" y="99" font-size="2.8" class="coord-dark">b</text><text x="97.5" y="99" font-size="2.8" class="coord-light">a</text>');
    }
    else {
        $(".player-one-area").removeClass("order-0");
        $(".player-one-avatar-area").removeClass("order-0");
        $(".player-one-clock").removeClass("order-1");
        $(".player-two-area").removeClass("order-1");
        $(".player-two-avatar-area").removeClass("order-1");
        $(".player-two-clock").removeClass("order-0");


        $(".player-one-area-sm").removeClass("order-0");
        $(".board").removeClass("order-1");
        $(".player-two-area-sm").removeClass("order-2");
        $(".buttons-and-moves-area").removeClass("order-3");

        $(".coords").html('<text x="0.75" y="3.5" font-size="2.8" class="coord-light">8</text><text x="0.75" y="15.75" font-size="2.8" class="coord-dark">7</text><text x="0.75" y="28.25" font-size="2.8" class="coord-light">6</text><text x="0.75" y="40.75" font-size="2.8" class="coord-dark">5</text><text x="0.75" y="53.25" font-size="2.8" class="coord-light">4</text><text x="0.75" y="65.75" font-size="2.8" class="coord-dark">3</text><text x="0.75" y="78.25" font-size="2.8" class="coord-light">2</text><text x="0.75" y="90.75" font-size="2.8" class="coord-dark">1</text><text x="10" y="99" font-size="2.8" class="coord-dark">a</text><text x="22.5" y="99" font-size="2.8" class="coord-light">b</text><text x="35" y="99" font-size="2.8" class="coord-dark">c</text><text x="47.5" y="99" font-size="2.8" class="coord-light">d</text><text x="60" y="99" font-size="2.8" class="coord-dark">e</text><text x="72.5" y="99" font-size="2.8" class="coord-light">f</text><text x="85" y="99" font-size="2.8" class="coord-dark">g</text><text x="97.5" y="99" font-size="2.8" class="coord-light">h</text>');
    }
}