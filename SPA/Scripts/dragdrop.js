// Drag & Drop Stuf
//Das Objekt, das gerade bewegt wird.
var dragObj = null;
var clickTimeout = null;

// Position, an der das Objekt angeklickt wurde.
// relativ zur oberen, rechten Ecke des Objekts
var dragx = 0;
var dragy = 0;

// Mausposition
// relativ zur oberen, rechten Ecke des Fensters
var posx = 0;
var posy = 0;

var posx_init = 0;
var posy_init = 0;

var lastTargetId = null;

function dragDropInit() {
    // Initialisierung der Überwachung der Events
    document.onmousemove = drag;
    document.onmouseup = dragstop;
}

function dragstart(element) {
    //Wird aufgerufen, wenn ein Objekt bewegt werden soll.
    dragObj = element;
    dragx = posx - dragObj.offsetLeft;
    dragy = posy - dragObj.offsetTop;
    posx_init = posx;
    posy_init = posy;
};

function dragstop() {
    var bewegt = false;
    //Wird aufgerufen, wenn ein Objekt nicht mehr bewegt werden soll.
    if ((posx > posx_init + 3) || (posx < posx_init - 3)) {
        bewegt = true;
    };
    if ((posy > posy_init + 3) || (posy < posy_init - 3)) {
        bewegt = true;
    };

    if (!bewegt) {
        spinIt(dragObj);
        selectPopup(dragObj);
    }
    dragObj = null;
}

function spinIt(dragObj) {
    var prefix = dragObj.id.substring(0, 2);

    if ((prefix == "mb") || (prefix == "cm")) {
        $("#" + dragObj.id).animate({ rotate: '360deg' }, 200);
    }
}

function selectPopup(dragObj) {

    setTimeout(function () {

        if (dragObj.id.substring(0, 2) == "mb") {
            m.showDetails(dragObj.id);
        } else {
            switch (dragObj.id) {
                case "cmd_Refresh":
                    m.loadSomeMovies();
                    break;
                case "cmd_Latest":
                    m.loadLatestMovies();
                    break;
                case "cmd_Genres":
                    m.showGenreSelector();
                    break;
                case "cmd_Movies":
                    dlg.popupMovieSelector();
                    break;
                case "cmd_Friends":
                    m.showFriendsSelector();
                    break;
                case "cmd_Actors":
                    dlg.popupActorSelector();
                    break;
                case "cmd_AddMovies":
                    dlg.popupAddMovies();
                    break;
                default:
                    //              alert("not supported " + dragObj.id);
                    break;
            }
        }
    }, 200);
}

function drag(ereignis) {
    //Wird aufgerufen, wenn die Maus bewegt wird und bewegt bei Bedarf das Objekt.
    if (clickTimeout != null) {
        clearTimeout(clickTimeout);
        clickTimeout = null;
    };
    posx = document.all ? window.event.clientX : ereignis.pageX;
    posy = document.all ? window.event.clientY : ereignis.pageY;

    if (dragObj != null) {
        dragObj.style.left = (posx - dragx) + "px";
        dragObj.style.top = (posy - dragy) + "px";
    }
}

function touchHandler(event) {
    // Das TouchEvent darf nicht gehandelt werden, wenn es sich um eine TextBox handelt
    // Convention: Textbox Ids bekommen ein txt Prefix
    var ctrlId = event.target.id;
    if (ctrlId.substr(0, 3) != "txt") {
        var touches = event.changedTouches,
             first = touches[0],
             type = "";

        switch (event.type) {
            case "touchstart":
                moved_touch = false;
                type = "mousedown";
                break;
            case "touchmove":
                moved_touch = true;
                type = "mousemove"; break;
            case "touchend":
                type = "mouseup";
                break;
            default: return;
        }
        var simulatedEvent = document.createEvent("MouseEvent");
        simulatedEvent.initMouseEvent(type, true, true, window, 1,
								  first.screenX, first.screenY,
								  first.clientX, first.clientY, false,
								  false, false, false, 0/*left*/, null);

        posx = document.all ? window.event.clientX : event.pageX;
        posy = document.all ? window.event.clientY : event.pageY;

        if (type == "mouseup") {
            clickTimeout = setTimeout(function () {
                if (!moved_touch) {
                    alert("mouseup");
                    if (lastTargetId != event.target.id) {
                        spinIt(event.target);
                        selectPopup(event.target);
                        lastTargetId = event.target.id;
                        var t = setTimeout(function() {
                            lastTargetId = null;
                        }, 100);
                    }
                };
            }, 100);
        }

        first.target.dispatchEvent(simulatedEvent);
        event.preventDefault();
    }
}

function touchInit() {
    document.addEventListener("touchstart", touchHandler, true);
    document.addEventListener("touchmove", touchHandler, true);
    document.addEventListener("touchend", touchHandler, true);
    document.addEventListener("touchcancel", touchHandler, true);
}