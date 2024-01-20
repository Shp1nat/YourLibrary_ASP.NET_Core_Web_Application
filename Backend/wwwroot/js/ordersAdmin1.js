async function getAllOrders() {
    const response = await fetch('https://localhost:7180/api/order/GetOrders', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('userToken')}`
        },
    });
    if (response.ok) {
        const orders = await response.json();
        const tableBody = document.querySelector('#ordersTable tbody');

        orders.forEach(order => {
            const row = document.createElement('tr');

            let statusColumn = '';
            let checkboxColumn = '';
            if (order.isBack === false) {
                statusColumn = '<td>Не возвращено</td>';
                checkboxColumn = '<td><input type="checkbox" /></td>';
            } else {
                statusColumn = '<td>Возвращено</td>';
                checkboxColumn = '<td></td>';
            }

            const authors = order.example.book.authors.join(', ');
            const types = order.example.book.typesBk.join(', ');
            const genres = order.example.book.genres.join(', ');
            row.innerHTML = `
                <td>${order.uidOrder}</td>
                <td>${order.example.uidExample}</td>
                <td>${order.user.login}</td>
                <td>${order.example.book.shifr}</td>
                <td>${order.example.book.nameOfBook}</td>
                <td>${authors}</td>
                <td>${genres}</td>
                <td>${types}</td>
                <td>${order.example.publisher.nameOfPublisher}</td>
                <td>${order.example.yearOfCreation}</td>
                ${statusColumn}
                ${checkboxColumn}
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving orders');
    }
}

async function closeOrders() {
    const checkboxes = document.querySelectorAll('#ordersTable tbody input[type="checkbox"]');
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
            const response = await fetch(`https://localhost:7180/api/Order/CloseOrder?uIdOrder=${uid}`, {
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
        } catch (error) {
            console.error();
        }
    });
    location.reload(true);
}
