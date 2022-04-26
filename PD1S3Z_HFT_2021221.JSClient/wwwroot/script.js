let books = [];
let libraries = [];
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
        .then(y => {
            books = y;
            console.log(books)
            display();
        });

    await fetch('http://localhost:26706/library')
        .then(x => x.json())
        .then(y => {
            libraries = y;
            console.log("asd")
            displaySelect();
        });
}

function displaySelect() {
    libraries.forEach(l => {
        var x = document.getElementById("library");
        var option = new Option(l.name, l.id);
        console.log(option.value);
        x.appendChild(option);
    })
    var elems = document.querySelectorAll('select');
    var instances = M.FormSelect.init(elems);
}

function display() {
    document.getElementById('resultarea').innerHTML = "";
    books.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
            "<tr><td>" + t.id + "</td><td>"
            + t.title + "</td><td>" +
        `<button class="waves-effect waves-light btn" type="button" onclick="remove(${t.id})">Delete</button>`
            + "</td></tr>";
    });
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


function create() {
    let title = document.getElementById('title').value;
    let author = document.getElementById('author').value;
    let genre = document.getElementById('genre').value;
    let publishing = document.getElementById('publishing').value;
    let numberOfPages = document.getElementById('numberOfPages').value;
    let libraryId = document.getElementById('library').value;

    console.log({ title, author, genre, publishing: parseInt(publishing), numberOfPages: parseInt(numberOfPages), libraryId: parseInt(libraryId) })

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