async function getUserData() {
    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;
    const response = await fetch(`https://localhost:7180/api/User/GetData?uIdUser=${uid}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`
        },
    });
    const info = await response.json();
    document.getElementById("emailData").textContent = info.email;
    document.getElementById('nameData').textContent = info.name;
    document.getElementById('surnameData').textContent = info.surname;
    document.getElementById('otchestvoData').textContent = info.otchestvo;
    document.getElementById("loginData").textContent = info.login;
    document.getElementById("addressData").textContent = info.address;
    document.getElementById('phoneNumberData').textContent = info.phoneNumber;
    document.getElementById('ageData').textContent = info.age;
}

async function updateUser() {

    const email = document.getElementById('emailIn').value;
    const name = document.getElementById('nameIn').value;
    const surname = document.getElementById('surnameIn').value;
    const otchestvo = document.getElementById('otchestvoIn').value;
    const login = document.getElementById('loginIn').value;
    const password = document.getElementById('passwordIn').value;
    const address = document.getElementById('addressIn').value;
    const phoneNumber = document.getElementById('phoneNumberIn').value;
    const age = document.getElementById('ageIn').value;

    if (!email || !login || !password) {
        alert('Please fill in the required fields');
        return;
    }
    const userUpdate = {
        email: email,
        name: name,
        surname: surname,
        otchestvo: otchestvo,
        login: login,
        password: password,
        address: address,
        phoneNumber: phoneNumber,
        age: age
    };
    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;
    await fetch(`https://localhost:7180/api/User/EditData?uIdUser=${uid}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`,
        },
        body: JSON.stringify(userUpdate)
    });
    location.reload(true);
}