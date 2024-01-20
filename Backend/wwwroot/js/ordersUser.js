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
            if (order.isBack === false) {
                statusColumn = '<td>Не возвращено</td>';
            } else {
                statusColumn = '<td>Возвращено</td>';
            }

            const authors = order.example.book.authors.join(', ');
            const types = order.example.book.typesBk.join(', ');
            const genres = order.example.book.genres.join(', ');
            row.innerHTML = `
                <td>${order.uidOrder}</td>
                <td>${order.example.uidExample}</td>
                <td>${order.example.book.shifr}</td>
                <td>${order.example.book.nameOfBook}</td>
                <td>${authors}</td>
                <td>${genres}</td>
                <td>${types}</td>
                <td>${order.example.publisher.nameOfPublisher}</td>
                <td>${order.example.yearOfCreation}</td>
                ${statusColumn}
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving orders');
    }
}