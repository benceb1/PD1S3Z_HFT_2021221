let books = [];
let libraries = [];
let selectedBook = null;
let connection = null;
getdata();
setupSignalR();


function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:26706/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("BookCreated", (user, message) => {
        getdata();
    });

    connection.on("BookDeleted", (user, message) => {
        getdata();
    });

    connection.on("BookUpdated", (user, message) => {
        getdata();
    });

    connection.onclose(async () => {
        await start();
    });
    start();


}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

async function getdata() {
    await fetch('http://localhost:26706/book')
        .then(x => x.json())
        .then(async y => {
            books = y;
            console.log(books)
            await fetch('http://localhost:26706/library')
                .then(x => x.json())
                .then(y => {
                    libraries = y;
                    displaySelect()
                });
            display();
        });

    
}

function displaySelect() {
    var x = document.getElementById("library");
    x.innerHTML = "";
    libraries.forEach(l => {
        
        var option = new Option(l.name, l.id);
        x.appendChild(option);
    })
    var elems = document.querySelectorAll('select');
    var instances = M.FormSelect.init(elems);
}

function display() {
    document.getElementById('resultarea').innerHTML = "";
    books.forEach(t => {
        let libName = libraries.filter(l => l.id === t.libraryId)[0].name
        document.getElementById('resultarea').innerHTML +=
            "<tr><td>" + t.id + "</td><td>"
        + t.title + "</td>"

        + "<td>" + t.author + "</td>"
        + "<td>" + t.genre + "</td>"
        + "<td>" + t.numberOfPages + "</td>"
        + "<td>" + t.publishing + "</td>"
            + "<td>" + libName + "</td>"

        + "<td>" +
        `<button style="margin: 10px;" class="waves-effect waves-light btn" type="button" onclick="remove(${t.id})">Delete</button>` +
        `<button style="margin: 10px;" class="waves-effect waves-light btn" type="button" onclick="select(${t.id})">Select</button>`
            + "</td></tr>";
    });
    displaySelect();

}

const select = (id) => {
    selectedBook = books.filter(l => l.id === id)[0];
    if (selectedBook) {
        document.getElementById('title').value = selectedBook.title;
        document.getElementById('author').value = selectedBook.author;
        document.getElementById('genre').value = selectedBook.genre;
        document.getElementById('publishing').value = selectedBook.publishing;
        document.getElementById('numberOfPages').value = selectedBook.numberOfPages;

        const element = document.getElementById('library');
        element.value = selectedBook.libraryId;
        const { options } = M.FormSelect.getInstance(element);
        M.FormSelect.init(element, options);

        document.getElementById('title').focus();
        document.getElementById('author').focus();
        document.getElementById('genre').focus();
        document.getElementById('publishing').focus();
        document.getElementById('numberOfPages').focus();
        document.getElementById('library').focus();
    }
}

function remove(id) {
    fetch('http://localhost:26706/book/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });
}

const update = () => {
    if (selectedBook) {
        let title = document.getElementById('title').value;
        let author = document.getElementById('author').value;
        let genre = document.getElementById('genre').value;
        let publishing = document.getElementById('publishing').value;
        let numberOfPages = document.getElementById('numberOfPages').value;
        let libraryId = document.getElementById('library').value;

        fetch('http://localhost:26706/book', {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json', },
            body: JSON.stringify(
                { id: selectedBook.id, title, author, genre, publishing: parseInt(publishing), numberOfPages: parseInt(numberOfPages), libraryId: parseInt(libraryId) })
        })
            .then(response => response)
            .then(data => {
                console.log('Success:', data);
                getdata();
                clearForm();
            })
            .catch((error) => { console.error('Error:', error); });
    } else {
        console.log("selectedBook is empty")
    }
}


function create() {
    let title = document.getElementById('title').value;
    let author = document.getElementById('author').value;
    let genre = document.getElementById('genre').value;
    let publishing = document.getElementById('publishing').value;
    let numberOfPages = document.getElementById('numberOfPages').value;
    let libraryId = document.getElementById('library').value;

    fetch('http://localhost:26706/book', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { title, author, genre, publishing: parseInt(publishing), numberOfPages: parseInt(numberOfPages), libraryId: parseInt(libraryId) })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
            clearForm();
        })
        .catch((error) => { console.error('Error:', error); });

}

function clearForm() {
    document.getElementById('title').value = "";
    document.getElementById('author').value = "";
    document.getElementById('genre').value = "";
    document.getElementById('publishing').value = "";
    document.getElementById('numberOfPages').value = "";
}