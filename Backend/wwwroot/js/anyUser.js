async function login() {
    var email = document.getElementById('email').value;
    var password = document.getElementById('password').value;

    var registerData = {
        email: email,
        password: password
    }

    var response = await fetch('https://localhost:7180/api/User/Login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(registerData)
    });

    if (response.ok) {
        var data = await response.json();

        if (data.token) {
            localStorage.setItem('userToken', data.token);

            var decodedToken = JSON.parse(atob(data.token.split('.')[1]));
            var userRole = decodedToken.role;

            if (userRole === 'admin') {
                window.location.href = 'mainAdmin.html';
            } else if (userRole === 'user') {
                window.location.href = 'mainUser.html';
            } else {
                alert('Во время авторизации произошла ошибка');
            }
        } else {
            alert('Авторизация не удалась');
        }
    } else {
        alert('Неправильный email или пароль');
    }
}


async function getUserData() {
    var decodedToken = JSON.parse(atob(localStorage.getItem('userToken').split('.')[1]));
    const uid = decodedToken.nameid;

    try {
        const response = await fetch(`https://localhost:7180/api/User/GetData?uIdUser=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
        });

        if (response.ok) {
            const info = await response.json();
            document.getElementById("emailData").textContent = info.email;
            document.getElementById('nameData').textContent = info.name;
            document.getElementById('surnameData').textContent = info.Surname;
            document.getElementById('otchestvoData').textContent = info.Otchestvo;
            document.getElementById("loginData").textContent = info.login;
            document.getElementById("addressData").textContent = info.Address;
            document.getElementById('phoneNumberData').textContent = info.phoneNumber;
            document.getElementById('ageData').textContent = info.Age;
        } else {
            throw new Error('Something went wrong');
        }
    } catch (error) {
        console.error(error);
    }
}
function RegisterUser() {
    const login = document.getElementById('login').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    fetch('https://localhost:7180/api/User/Register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ login, email, password })
    })
        .then(response => {
            if (response.ok) {
                alert('Пользователь зарегистрирован');
                window.location.href = '../html/homePage.html'; // Перенаправление на страницу входа
            } else {
                alert('Пользователь с таким login или email уже существует');
            }
        })
        .catch(error => {
            ;
        });
}
async function updateUser() {
    const email = document.getElementById('emailIn').value;
    const name = document.getElementById('nameIn').value;
    const surname = document.getElementById('surnameIn').value;
    const otchestvo = document.getElementById('otchestvoIn').value;
    const login = document.getElementById('loginIn').value;
    const password = document.getElementById('passwordIn').value;
    const address = document.getElementById('addressIn').value;
    const phoneNumber = document.getElementById('phoneNubmerIn').value;
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
    alert("hello");
    try {
        const response = await fetch(`https://localhost:7180/api/User/EditData?uIdUser=${uid}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
            body: JSON.stringify(userUpdate)
        });

        if (response.ok) {
            const data = await response.text();
            console.log(data);
            alert('Data updated');
            location.reload(true);
        } else {
            throw new Error('Failed to update data');
        }
    } catch (error) {
        console.error(error);
    }
}


