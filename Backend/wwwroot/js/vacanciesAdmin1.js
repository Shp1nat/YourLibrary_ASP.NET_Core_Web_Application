async function getVacancies() {
    const response = await fetch('https://localhost:7180/api/vacancy/GetVacancies', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('userToken')}`
        },
    });
    if (response.ok) {
        const vacancies = await response.json();
        const tableBody = document.querySelector('#vacanciesTable tbody');

        vacancies.forEach(vacancy => {
            const row = document.createElement('tr');

            let statusColumn = '';
            if (vacancy.statusOfVacancy === 1) {
                statusColumn = '<td>Принято</td>';
            }
            else if (vacancy.statusOfVacancy === -1) {
                statusColumn = '<td>Отказано</td>';
            }
            else {
                statusColumn = '<td>На рассмотрении</td>';
            }
            row.innerHTML = `
                <td>${vacancy.uidVacancy}</td>
                <td>${vacancy.user.login}</td>
                <td>${vacancy.user.email}</td>
                <td>${vacancy.text}</td>
                ${statusColumn}
                <td><input type="checkbox" /></td>
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving orders');
    }
}

async function acceptVacancies() {
    const checkboxes = document.querySelectorAll('#vacanciesTable tbody input[type="checkbox"]');
    const selectedOrders = [];
    if (checkboxes.length === 0) {
        alert('Выберите хотя бы один заказ');
        return;
    }
    checkboxes.forEach((checkbox, index) => {
        if (checkbox.checked) {
            const row = checkbox.closest('tr');
            const guid = row.querySelector('td:nth-child(1)').textContent;
            selectedOrders.push(guid);
        }
    });

    selectedOrders.forEach(async (uid) => {

        const response = await fetch(`https://localhost:7180/api/vacancy/AcceptVacancy?uIdVacancy=${uid}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
        })
        if (response.ok) {

        }
        else {
            throw new Error('Что-то пошло не так');
        }

    });
    location.reload(true);
}
async function rejectVacancies() {
    const checkboxes = document.querySelectorAll('#vacanciesTable tbody input[type="checkbox"]');
    const selectedOrders = [];
    if (checkboxes.length === 0) {
        alert('Выберите хотя бы один заказ');
        return;
    }
    checkboxes.forEach((checkbox, index) => {
        if (checkbox.checked) {
            const row = checkbox.closest('tr');
            const guid = row.querySelector('td:nth-child(1)').textContent;
            selectedOrders.push(guid);
        }
    });

    selectedOrders.forEach(async (uid) => {
        try {
            const response = await fetch(`https://localhost:7180/api/vacancy/RejectVacancy?uIdVacancy=${uid}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            })

        } catch (error) {
            console.error();
        }
    });
    location.reload(true);
}