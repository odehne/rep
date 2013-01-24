var Model = function () {
    this.serviceUrl = "http://localhost/MediaManager2010/",
    this.movies = [],
    this.friends = [],
    this.genres = [],
    this.selectedMovie = null,
    this.drawMovies = function() {
        $("#ModelContainer").innerHTML = "";
        for (var i = 0; i < this.movies.length; i++) {
                $("#ModelContainer").append("<div id='mb" + i + "' class='round-corner-div' onMouseDown='dragstart(this)'></div>");
                this.placeObjectOnCanvas($("#mb" + i));
                $("#mb" + i).css("background-image", "url(" + this.movies[i].CoverUrlMediaManager + ")");
        };
    },
    this.drawCommands = function() {
        //Genres Button
        $("#ModelContainer").append("<div id='cmd_Genres' class='round-corner-div' onMouseDown='dragstart(this)'><br/><br/><br/>Genres</div>");
        this.placeObjectOnCanvas($("#cmd_Genres"));
        //Friends Button
        $("#ModelContainer").append("<div id='cmd_Friends' class='round-corner-div' onMouseDown='dragstart(this)'><br/><br/><br/>Freunde</div>");
        this.placeObjectOnCanvas($("#cmd_Friends"));
        //Actors Button
        $("#ModelContainer").append("<div id='cmd_Actors' class='round-corner-div' onMouseDown='dragstart(this)'><br/><br/><br/>Cast</div>");
        this.placeObjectOnCanvas($("#cmd_Actors"));
        //Actors Button
        $("#ModelContainer").append("<div id='cmd_Movies' class='round-corner-div' onMouseDown='dragstart(this)'><br/><br/><br/>Filme</div>");
        this.placeObjectOnCanvas($("#cmd_Movies"));
        //Refresh Button
        $("#ModelContainer").append("<div id='cmd_Refresh' class='round-corner-div' onMouseDown='dragstart(this)'><br/><br/>Neu laden</div>");
        this.placeObjectOnCanvas($("#cmd_Refresh"));
        //Latest Button
        $("#ModelContainer").append("<div id='cmd_Latest' class='round-corner-div' onMouseDown='dragstart(this)'><br/><br/>Neu heiten</div>");
        this.placeObjectOnCanvas($("#cmd_Latest"));
        //Add Button
        $("#ModelContainer").append("<div id='cmd_AddMovies' class='round-corner-div' onMouseDown='dragstart(this)'><br/><br/>Add movies</div>");
        this.placeObjectOnCanvas($("#cmd_AddMovies"));
    },
    this.placeObjectOnCanvas = function(o) {
        o.css("left", getRandomNumber(200, 500));
        o.css("top", getRandomNumber(100, 400));
        var degree = getRandomNumber(1, 270);
        o.css({ WebkitTransform: 'rotate(' + degree + 'deg)' });
        o.css({ '-moz-transform': 'rotate(' + degree + 'deg)' });
        o.css({ '-ms-transform': 'rotate(' + degree + 'deg)' });
    },
    this.loadFriends = function() {
        //load initial movies to play with
        $.getJSON(this.serviceUrl + "friends", function(data) {
            //$("#systemProgress").hide();
            m.friends = data;
        });
    },
    this.loadActors = function(letter, ctrl) {
        $('#' + ctrl).empty();
        $.getJSON(this.serviceUrl + "actors?letter=" + letter, function(data) {
            for (var i = 0; i < data.length; i++) {
                $('#' + ctrl).append("<option value='" + data[i].ID + "'>" + data[i].Name + "</option>");
            }
            ;
        });
    },
    this.loadMoviesWithLetter = function(letter, ctrl) {
        $('#' + ctrl).empty();
        $.getJSON(this.serviceUrl + "items?letter=" + letter, function(data) {
            for (var i = 0; i < data.length; i++) {
                $('#' + ctrl).append("<option value='" + data[i].id + "'>" + data[i].title + "</option>");
            }
            ;
        });
    },
    this.loadGenres = function() {
        //load initial movies to play with
        $.getJSON(this.serviceUrl + "genres", function(data) {
            //$("#systemProgress").hide();
            m.genres = data;
        });
    },
    this.loadSomeMovies = function() {
        this.cleanUp();
        //load initial movies to play with
        $.getJSON(this.serviceUrl + "items?" + getRandomNumber(1,5000) , function(data) {
            //$("#systemProgress").hide();
            m.movies = data;
            if (m.movies.length > 0) {
                m.drawMovies();
            }
        });
    },
    this.loadLatestMovies = function () {
        this.cleanUp();
        //load initial movies to play with
        $.getJSON(this.serviceUrl + "items?latest=true" + getRandomNumber(1, 5000), function (data) {
            //$("#systemProgress").hide();
            m.movies = data;
            if (m.movies.length > 0) {
                m.drawMovies();
            }
        });
    },
    this.cleanUp = function () {
        if (m.movies.length > 0) {
            for (var i = 0; i < this.movies.length; i++) {
                $('#mb' + i).remove();
            }
            m.movies = [];
        }
    },
    this.loadMoviesByGenre = function(genreId) {
        this.cleanUp();
        $.getJSON(this.serviceUrl + "items?genreId=" + genreId, function(data) {
            m.movies = data;
            if (m.movies.length > 0) {
                m.drawMovies();
            }
        });
    },
    this.loadMovieById = function(movieId) {
        this.cleanUp();
        $.getJSON(this.serviceUrl + "items?id=" + movieId, function(data) {
            m.movies.push(data);
            if (m.movies.length > 0) {
                m.drawMovies();
            }
        });
    },
    this.loadMoviesOfaFriend = function(friendId) {
        this.cleanUp();
        $.getJSON(this.serviceUrl + "items?friendId=" + friendId, function(data) {
            m.movies = data;
            if (m.movies.length > 0) {
                m.drawMovies();
            }
        });
    },
    this.loadBorrowed = function() {
        this.cleanUp();
        if ($.cookie('movie-user-id') != null) {
            $.getJSON(this.serviceUrl + "items?borrowedById=" + $.cookie('movie-user-id'), function(data) {
                m.movies = data;
                if (m.movies.length > 0) {
                    m.drawMovies();
                }
            });
        } else {
            alert("Du musst angemeldet sein, um diese Ansicht nutzen zu können.");
        }
    },
    this.loadLent = function() {
        this.cleanUp();
        if ($.cookie('movie-user-id') != null) {
            $.getJSON(this.serviceUrl + "items?lentById=" + $.cookie('movie-user-id'), function (data) {
                m.movies = data;
                if (m.movies.length > 0) {
                    m.drawMovies();
                }
            });
        } else {
            alert("Du musst angemeldet sein, um diese Ansicht nutzen zu können.");
        }

    },
    this.loadMoviesByActor = function(actorId) {
        this.cleanUp();
        $.getJSON(this.serviceUrl + "items?actorId=" + actorId, function(data) {
            m.movies = data;
            if (m.movies.length > 0) {
                m.drawMovies();
            }
        });
    },
    this.showDetails = function(ctrlId) {
        if (ctrlId != undefined && ctrlId.length > 2) {
            var index = ctrlId.substring(2);
            this.selectedMovie = this.movies[index];
            dlg.popupMovieDetails(this.movies[index]);
        }
        ;
    },
    this.showLentTo = function () {
        if (!$.cookie('movie-user-name')) {
            dlg.showLogin($('#details'), $('#detailsContainer'));
        } else {
            if (m.selectedMovie.OwnerID == $.cookie('movie-user-id')) {
                dlg.popupLentToSelector($('#details'), $('#detailsContainer'));
            } else {
                alert("Es scheint, als gehörte dir dieser Film nicht.");
            }
        };
    },
    this.borrowMovie = function () {
        if (!$.cookie('movie-user-name')) {
                dlg.showLogin($('#details'), $('#detailsContainer'));
        } else {
            $.getJSON(this.serviceUrl + "items?borrowTo=" + $.cookie('movie-user-name') + "&id=" + m.selectedMovie.ID, function(data) {
                alert(data);
            });
        };
    },
    this.returnMovie = function() {
        if (!$.cookie('movie-user-id')) {
            dlg.showLogin($('#details'), $('#detailsContainer'));
        } else {
            if ($.cookie('movie-user-id') != m.selectedMovie.OwnerID) {
                alert("Der Film scheint nicht dir zu geören.");
            } else {
                $.getJSON(this.serviceUrl + "items?returnMovie=" + m.selectedMovie.ID, function (data) {
                    if (data == "OK") {
                        m.selectedMovie.OwnerName = "";
                    } else {
                        alert(data);
                    }
                });
            }
        };
    },
    this.addMovie= function () {
        if (!$.cookie('movie-user-id')) {
            dlg.showLogin($('#details'), $('#detailsContainer'));
        } else {
            $("#addMovieErrorMessage").hide();
            $("#resultTable").hide();
            $.getJSON(this.serviceUrl + "items?addByEAN=" + $("#txtEan").val() + "&friendId=" + $.cookie('movie-user-id'), function (data) {
                if (data != null) {
                    if (data.ErrMessage != "OK") {
                        $("#addMovieErrorMessage").show();
                        $("#addMovieErrorMessage").html(data.ErrMessage);
                    } else {
                        $("#resultTable").append("<h2>" + data.ResultMessage + "</h2>");
                        $("#largeThumb").append("<img src='" + data.CoverUrl + "&size=large' alt='" + data.Title + "' height='470' width='330' />");
                        $("#resultTable").show();
                   //     $("#largeThumb").show();
                    }
                }
            });
        };
    },
    this.showGenreSelector = function () {
        dlg.popupGenreSelector();
    },
    this.showFriendsSelector = function() {
        dlg.popupFriendsSelector();
    },
    this.showLoggedInUser = function() {
        if ($.cookie("movie-user-name") != null && $.cookie("movie-user-name")!="") {
            $("#loggedInUser").html("<a href='#' onmouseup='dlg.logout();'>" + $.cookie("movie-user-name") + " abmelden</a>");
        } else {
            $("#loggedInUser").empty();
        };
    };
};

function getRandomNumber(start, end) {
    return Math.floor((Math.random() * end) + start);
}

var m = new Model;


function Init() {
    dragDropInit();
    touchInit();
    // if user clicked on button, the overlay layer or the dialogbox, close the dialog  
    $('a.btn-ok, #dialog-overlay, #dialog-box').click(function () {
        $('#dialog-overlay, #dialog-box').remove();
        return false;
    });

    //show loading animation while ajax calls
    $('#loadingOverlay')
        .hide()  
        .ajaxStart(function () {
            $(this).css("z-index", 6001);
            $(this).show();
        })
        .ajaxStop(function () {
            $(this).hide();
        });

    m.loadFriends();
    m.loadGenres();
    m.loadSomeMovies();
    m.drawCommands();
    m.showLoggedInUser();

    //Welcome Text anzeigen, wenn kein Cookie gestezt ist
    if ($.cookie('movie-welcome') != 'hidden') {
        dlg.popupWelcome();
    }
}

