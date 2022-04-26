
let borrowers = [];
let selectedBorrower = null;
let connection = null;
getdata();
setupSignalR();


function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:26706/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("BorrowerCreated", (user, message) => {
        getdata();
    });

    connection.on("BorrowerDeleted", (user, message) => {
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
    await fetch('http://localhost:26706/borrower')
        .then(x => x.json())
        .then(y => {
            borrowers = y;
            console.log(borrowers)
            display();
        });
}

function display() {
    document.getElementById('resultarea').innerHTML = "";
    borrowers.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
            "<tr><td>" + t.id + "</td><td>"
            + t.name + "</td><td>" +
        `<button style="margin: 10px;" class="waves-effect waves-light btn" type="button" onclick="remove(${t.id})">Delete</button>` +
        `<button style="margin: 10px;" class="waves-effect waves-light btn" type="button" onclick="select(${t.id})">Select</button>`
            + "</td></tr>";
    });
}

const select = (id) => {
    selectedBorrower = borrowers.filter(l => l.id === id)[0];

    if (selectedBorrower) {

    }
}

function remove(id) {
    fetch('http://localhost:26706/borrower/' + id, {
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
    let Name = document.getElementById('name').value;
    let Age = document.getElementById('age').value;
    let MembershipLevel = document.getElementById('membershipLevel').value;
    let NumberOfBooksRead = document.getElementById('numberOfBooksRead').value;
    let NumberOfLateLendings = document.getElementById('numberOfLateLendings').value;
    let StartOfMembership = document.getElementById('startOfMembership').value;
    
    fetch('http://localhost:26706/borrower', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { Name, Age: parseInt(Age), MembershipLevel: parseInt(MembershipLevel), NumberOfBooksRead: parseInt(NumberOfBooksRead), NumberOfLateLendings: parseInt(NumberOfLateLendings), StartOfMembership })
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
    document.getElementById('name').value = "";
    document.getElementById('age').value = "";
    document.getElementById('membershipLevel').value = "";
    document.getElementById('numberOfBooksRead').value = "";
    document.getElementById('numberOfLateLendings').value = "";
    document.getElementById('startOfMembership').value = "";
}