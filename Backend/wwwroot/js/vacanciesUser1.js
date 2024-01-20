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
                <td>${vacancy.text}</td>
                ${statusColumn}
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving orders');
    }
}

async function createVacancy() {
    var vacancyText = document.getElementById("vacancyText");
    var text = vacancyText.value;
    vacancyText.value = "";
    const response = await fetch(`https://localhost:7180/api/vacancy/CreateVacancy?text=${text}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`
        },
    })
    location.reload(true);
}