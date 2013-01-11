var Movie = function (title) {
        this.id = 0;
        this.title = title;
        this.actor1 = "";
        this.actor2 = "";
        this.actor3 = "";
        this.director = "";
        this.description = "";
        this.genre = "";
        this.imageUrl = "";
};

    // Box class, can draw a Table box
var Box = function(parent) {
    this.index = parent.id,
    this.height = 200,
    this.width = 120,
    this.top = 0,
    this.left = 0;

    this.collapse = function() {
        var triple = {
            x: this.height,
            y: this.top + (this.height / 4),
            ident: 2
        };
        return triple;
    };

    this.expand = function() {
        var triple = {
            x: this.left + this.width,
            y: this.top + (this.height / 4),
            ident: -2
        }
        return triple;
    };

    this.render = function() {
        var i,
            modelContainer = document.getElementById("ModelContainer");

        if (modelContainer) {
            document.createDocumentFragment();
            // div
            d = document.createElement('div');
            d.setAttribute('class', 'round-corner-div');
            d.setAttribute('onmousedown', 'dragstart(this)');

            // table
            c = document.createElement('table');
            c.setAttribute('id', parent.name);
            c.setAttribute('border', '0');
            c.setAttribute('width', this.width);
            c.setAttribute('height', this.height);

            th2 = document.createElement("th");
            th2.setAttribute('colspan', '2');
            th2.appendChild(document.createTextNode(parent.name));
            c.appendChild(th2);

            for (i = 0; i < parent.columns.length; i++) {
                tr = document.createElement('tr');
                if (i % 2 != 0) {
                    tr.setAttribute('class', 'odd');
                }
                td1 = document.createElement('td');
                if (parent.columns[i].isPrimaryKey === true) {
                    td1.appendChild(document.createTextNode("$"));
                }
                ;
                tr.appendChild(td1)
                td2 = document.createElement('td');
                td2.appendChild(document.createTextNode(parent.columns[i].name));
                tr.appendChild(td2);
                c.appendChild(tr);
            }
            ;

            d.appendChild(c);
            modelContainer.appendChild(d);
        }
    };
};
