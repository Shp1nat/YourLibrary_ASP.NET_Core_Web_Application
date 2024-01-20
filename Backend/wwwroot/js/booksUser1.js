async function getAllExamples() {
    const response = await fetch('https://localhost:7180/api/example/GetExamples', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('userToken')}`
        },
    });
    if (response.ok) {
        const examples = await response.json();
        const tableBody = document.querySelector('#examplesTable tbody');

        examples.forEach(example => {
            const row = document.createElement('tr');

            let checkboxColumn = '';
            let statusColumn = '';
            if (example.isTaken === true) {
                checkboxColumn = '<td></td>';
                statusColumn = '<td>В аренде</td>';
            } else {
                checkboxColumn = '<td><input type="checkbox" /></td>';
                statusColumn = '<td>Доступно</td>';
            }


            const authors = example.book.authors.join(', ');
            const types = example.book.typesBk.join(', ');
            const genres = example.book.genres.join(', ');
            row.innerHTML = `
                <td>${example.uidExample}</td>
                <td>${example.book.shifr}</td>
                <td>${example.book.nameOfBook}</td>
                <td>${authors}</td>
                <td>${genres}</td>
                <td>${types}</td>
                <td>${example.publisher.nameOfPublisher}</td>
                <td>${example.yearOfCreation}</td>
                ${statusColumn}
                ${checkboxColumn}
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving examples');
    }
}

function createOrder() {
    const checkboxes = document.querySelectorAll('#examplesTable tbody input[type="checkbox"]');
    const selectedExamples = [];
    if (checkboxes.length === 0) {
        alert('Выберите хотя бы один экземпляр');
        return;
    }
    checkboxes.forEach((checkbox, index) => {
        if (checkbox.checked) {
            const row = checkbox.closest('tr');
            const guid = row.querySelector('td:nth-child(1)').textContent;
            selectedExamples.push(guid);
        }
    });

    selectedExamples.forEach(async (uid) => {
        try {
            const response = await fetch(`https://localhost:7180/api/Order/CreateOrder?uIdExample=${uid}`, {
                method: 'POST',
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