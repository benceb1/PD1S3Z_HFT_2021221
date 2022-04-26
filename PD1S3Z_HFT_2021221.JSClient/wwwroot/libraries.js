let libraries = [];
let selectedLib = null;
let connection = null;
getdata();
setupSignalR();


function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:26706/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("LibraryCreated", (user, message) => {
        getdata();
    });

    connection.on("LibraryDeleted", (user, message) => {
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
    await fetch('http://localhost:26706/library')
        .then(x => x.json())
        .then(y => {
            libraries = y;
            console.log(libraries)
            display();
        });
}

function display() {
    document.getElementById('resultarea').innerHTML = "";
    libraries.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
            `<tr><td>` + t.id + "</td><td>"
            + t.name + "</td><td>" +
        `<button style="margin: 10px;" class="waves-effect waves-light btn" type="button" onclick="remove(${t.id})">Delete</button>` +
        `<button style="margin: 10px;" class="waves-effect waves-light btn" type="button" onclick="select(${t.id})">Select</button>`

            + "</td></tr>";
    });
}

const select = (id) => {
    selectedLib = libraries.filter(l => l.id === id)[0];
    if (selectedLib) {
        document.getElementById('name').value = selectedLib.name;
        document.getElementById('bookCapacity').value = selectedLib.bookCapacity;
        document.getElementById('name').focus();
        document.getElementById('bookCapacity').focus();
    } 
}

function remove(id) {
    fetch('http://localhost:26706/library/' + id, {
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
    let name = document.getElementById('name').value;
    let bookCapacity = document.getElementById("bookCapacity").value;
    fetch('http://localhost:26706/library', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { name, bookCapacity: parseInt(bookCapacity) })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
            clearForm()
        })
        .catch((error) => { console.error('Error:', error); });

}

function clearForm() {
    document.getElementById('name').value = "";
    document.getElementById('bookCapacity').value = "";
}