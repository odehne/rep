 var DialogManager = function() {
    //TODO: Sollte man mal schick machen ...
    this.placeDialogCookie = function(dlgName) {
        if (!$.cookie(dlgName)) {
            $.cookie(dlgName, 'launched', 1, 365);
        };
    },
    this.popupWelcome = function() {
        this.popup("Templates/welcome.html");
    },
    this.popupMovieSelector = function() {
        this.popup("Templates/movieSearch.html");
    },
    this.popupAddMovies = function() {
        this.popup("Templates/addMovie.html", null, null, "#txtEan");
    },
    this.popupActorSelector = function() {
        this.popup("Templates/actorsDetails.html");
    },
    this.popupGenreSelector = function() {
        this.popup("Templates/genresDetails.html", m.genres, "#genreList");
    },
    this.popupFriendsSelector = function() {
        this.popup("Templates/friendsDetails.html", m.friends, "#friendsList");
    },
    this.popupMovieDetails = function(movie) {
        this.popup("Templates/movieDetails.html", movie, "#dialog-message");
    },
    this.popupForgotMyPassword = function(ctrlToHide, parent) {
        ctrlToHide.hide();
        parent.append("<div id='forgotMyPassword' />");
        $("#forgotMyPassword").load("Templates/forgotMyPassword.html", function () {
            $("#ferrorMessage").hide();
            $("#sendEmail").bind("mouseup", function () {
                $.getJSON(m.serviceUrl + "passwordreset?cmd=request&email=" + $("#txtForgotEmail").val(), function (data) {
                    if (data == "OK") {
                        alert("Bitte schau in deinen Email Posteingang, dort sollte sich eine neue Email befinden ;-)");
                    } else {
                        $("#ferrorMessage").innerHTML=data;
                        $("#ferrorMessage").show();
                    }
                });
            });
        });
    },
    this.popupRegister = function(ctrlToHide, parent) {
        ctrlToHide.hide();
        parent.append("<div id='register' />");
        $("#register").load("Templates/register.html", function() {
            $("#registerErrorMessage").hide();
        });
    },
    this.logout = function() {
        $.cookie("movie-user-name", null, -1);
        $.cookie("movie-user-email", null, -1);
        $.cookie("movie-user-id", null, -1);
        m.showLoggedInUser();
    },
    this.registerNewUser = function() {
        $("#registerErrorMessage").hide();
        $.post(m.serviceUrl + "friends", {
            username: $("#txtRegisterUsername").val(),
            password: $("#txtRegisterPassword").val(),
            email: $("#txtRegisterEmail").val()
            }, function (data) {
                if (data != "OK") {
                    $("#registerErrorMessage").innerHTML=data;
                    $("#registerErrorMessage").show();
                } else {
                    alert("Willkommen, du bist jetzt registriert und kannst dich anmelden.");
                    dlg.showLogin($("#register"), $('#detailsContainer'));
                }
        });
    },
    this.popupLentToSelector = function (ctrlToHide, parent) {
        ctrlToHide.hide();
        parent.append("<div id='lentToSelector' />");
        $("#lentToSelector").load("Templates/lentToSelector.html", function () {
            $("#lentToTemplate").tmpl(m.friends).appendTo($("#friendsList"));
        });
    },
    this.lentTo = function (friendId) {
        $.getJSON(m.serviceUrl + "items?id=" + m.selectedMovie.ID + "&lentTo=" + friendId, function (data) {
            if (data == "OK") {
                alert("Der Film wurde als ausgeliehen vermerkt.");
            } else {
                $("#ferrorMessage").innerHTML = data;
                $("#ferrorMessage").show();
            }
        });
    },
    this.showLogin = function(ctrlToHide, parent) {
        ctrlToHide.hide();
        if (document.getElementById("login") != undefined) {
            $("#login").show();
            return;
        };
        parent.append("<div id='login' />");
        $("#login").load("Templates/login.html", function () {
            $("#errorMessage").hide();
            $("#txtUsername").focus();
            $("#registerNew").bind("mouseup", function() {
                dlg.popupRegister($("#login"), parent);
            });
            $("#forgotPassword").bind("mouseup", function() {
                dlg.popupForgotMyPassword($("#login"), parent);
            });
            $("#loginButton").bind("mouseup", function() {
                $("#errorMessage").hide();
                if ($("#txtUsername").val() == "" || $("#txtPassword").val() == "") {
                    $("#errorMessage").show();
                } else {
                    $.getJSON(m.serviceUrl + "friends?username=" + $("#txtUsername").val() + "&password=" + $("#txtPassword").val(), function(data) {
                        if (data != null) {
                            $("#login").remove();
                            ctrlToHide.show();
                            $.cookie("movie-user-name", data.Name, 365);
                            $.cookie("movie-user-email", data.Email, 365);
                            $.cookie("movie-user-id", data.id, 365);
                            m.showLoggedInUser();
                        } else {
                            $("#errorMessage").show();
                        }
                    });
                }
                ;
            });
        }, false);
    },
    this.popup = function(templateUrl, objTemplate, parentCtrlId, focusCtrlName) {
     
        $("body").append("<div id='dialog-overlay'></div>");
        $("body").append("<div id='dialog-box'></div>");

        var maskHeight = $(document).height();
        var maskWidth = $(window).width();

        var dialogTop = 100 //(maskHeight / 2) - ($('#dialog-box').height() / 2);
        var dialogLeft = (maskWidth / 2) - ($('#dialog-box').width() / 2);

        $('#dialog-overlay').css({ height: maskHeight, width: maskWidth }).show();
        $('#dialog-box').css({ top: dialogTop, left: dialogLeft }).show();
        

       $("#dialog-box").load(templateUrl, function () {
                if (parentCtrlId != null) {
                $("#detailsTemplate").tmpl(objTemplate).appendTo(parentCtrlId);
            }
            if (focusCtrlName != null) {
                $(focusCtrlName).focus();
                $(focusCtrlName).val(m.currentSearchEan);
            } 
        });

    };
};

function centerDialog() {
    $(document).keyup(function (e) {
        if (e.keyCode == 13) {
            $('#mask').hide();
            $('.window').hide(); 
        }
    });
    
    $(window).resize(function () {

        var box = $('#boxes .window');

        //Get the screen height and width
        var maskHeight = $(document).height();
        var maskWidth = $(window).width();

        //Set height and width to mask to fill up the whole screen
        $('#mask').css({ 'width': maskWidth, 'height': maskHeight });

        //Get the window height and width
        var winH = $(window).height();
        var winW = $(window).width();

        //Set the popup window to center
        box.css('top', winH / 2 - box.height() / 2);
        box.css('left', winW / 2 - box.width() / 2);

    });
}

var dlg = new DialogManager();