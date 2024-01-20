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

async function getAllBooks() {
    const response = await fetch('https://localhost:7180/api/book/GetBooks', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('userToken')}`
        },
    });
    if (response.ok) {
        const books = await response.json();
        const tableBody = document.querySelector('#booksTable tbody');

        books.forEach(book => {
            const row = document.createElement('tr');

            const authors = book.authors.join(', ');
            const types = book.typesBk.join(', ');
            const genres = book.genres.join(', ');
            row.innerHTML = `
                <td>${book.uidBook}</td>
                <td>${book.shifr}</td>
                <td>${book.nameOfBook}</td>
                <td>${authors}</td>
                <td>${genres}</td>
                <td>${types}</td>
                <td><input type="checkbox" /></td>
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving examples');
    }
}

async function getAllTypesBk() {
    const response = await fetch('https://localhost:7180/api/typeBk/GetTypesBk', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('userToken')}`
        },
    });
    if (response.ok) {
        const typesBk = await response.json();
        const tableBody = document.querySelector('#typesBkTable tbody');

        typesBk.forEach(typeBk => {
            const row = document.createElement('tr');

            row.innerHTML = `
                <td>${typeBk.uidTypeBk}</td>
                <td>${typeBk.nameOfTypeBk}</td>
                <td><input type="checkbox" /></td>
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving examples');
    }
}

async function getAllGenres() {
    const response = await fetch('https://localhost:7180/api/genre/GetGenres', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('userToken')}`
        },
    });
    if (response.ok) {
        const genres = await response.json();
        const tableBody = document.querySelector('#genresTable tbody');

        genres.forEach(genre => {
            const row = document.createElement('tr');

            row.innerHTML = `
                <td>${genre.uidGenre}</td>
                <td>${genre.nameOfGenre}</td>
                <td><input type="checkbox" /></td>
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving examples');
    }
}

async function getAllAuthors() {
    const response = await fetch('https://localhost:7180/api/author/GetAuthors', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('userToken')}`
        },
    });
    if (response.ok) {
        const authors = await response.json();
        const tableBody = document.querySelector('#authorsTable tbody');

        authors.forEach(author => {
            const row = document.createElement('tr');

            row.innerHTML = `
                <td>${author.uidAuthor}</td>
                <td>${author.nameOfAuthor}</td>
                <td><input type="checkbox" /></td>
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving examples');
    }
}

async function getAllPublishers() {
    const response = await fetch('https://localhost:7180/api/publisher/GetPublishers', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('userToken')}`
        },
    });
    if (response.ok) {
        const publishers = await response.json();
        const tableBody = document.querySelector('#publishersTable tbody');

        publishers.forEach(publisher => {
            const row = document.createElement('tr');

            row.innerHTML = `
                <td>${publisher.uidPublisher}</td>
                <td>${publisher.nameOfPublisher}</td>
                <td><input type="checkbox" /></td>
            `;
            tableBody.appendChild(row);
        });
    } else {
        console.log('An error occurred while retrieving examples');
    }
}

async function getAll() {
    getAllExamples();
    getAllBooks();
    getAllAuthors();
    getAllTypesBk();
    getAllGenres();
    getAllPublishers();
}

async function createOrder() {
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

async function addAuthor() {

    const nameOfAuthor = document.getElementById('authorForAdding').value;
    if (!nameOfAuthor) {
        alert('Поле не должно быть пустым');
        return;
    }
    await fetch(`https://localhost:7180/api/Author/CreateAuthor?nameOfAuthor=${nameOfAuthor}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`,
        },
    });
    location.reload(true);
}


async function deleteAuthors() {
    const checkboxes = document.querySelectorAll('#authorsTable tbody input[type="checkbox"]');
    const selectedExamples = [];
    if (checkboxes.length === 0) {
        alert('Выберите хотя бы одного автора');
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
            const response = await fetch(`https://localhost:7180/api/Author/DeleteAuthor?uIdAuthor=${uid}`, {
                method: 'DELETE',
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

async function addGenre() {

    const nameOfGenre = document.getElementById('genreForAdding').value;
    if (!nameOfGenre) {
        alert('Поле не должно быть пустым');
        return;
    }
    await fetch(`https://localhost:7180/api/Genre/CreateGenre?nameOfGenre=${nameOfGenre}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`,
        },
    });
    location.reload(true);
}


async function deleteGenres() {
    const checkboxes = document.querySelectorAll('#genresTable tbody input[type="checkbox"]');
    const selectedExamples = [];
    if (checkboxes.length === 0) {
        alert('Выберите хотя бы одного автора');
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
            const response = await fetch(`https://localhost:7180/api/Genre/DeleteGenre?uIdGenre=${uid}`, {
                method: 'DELETE',
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

async function addTypeBk() {

    const nameOfTypeBk = document.getElementById('typeBkForAdding').value;
    if (!nameOfTypeBk) {
        alert('Поле не должно быть пустым');
        return;
    }
    await fetch(`https://localhost:7180/api/TypeBk/CreateTypeBk?nameOfTypeBk=${nameOfTypeBk}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`,
        },
    });
    location.reload(true);
}

async function deleteTypesBk() {
    const checkboxes = document.querySelectorAll('#typesBkTable tbody input[type="checkbox"]');
    const selectedExamples = [];
    if (checkboxes.length === 0) {
        alert('Выберите хотя бы одного автора');
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
            const response = await fetch(`https://localhost:7180/api/TypeBk/DeleteTypeBk?uIdTypeBk=${uid}`, {
                method: 'DELETE',
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

async function addPublisher() {

    const nameOfPublisher = document.getElementById('publisherForAdding').value;
    if (!nameOfPublisher) {
        alert('Поле не должно быть пустым');
        return;
    }
    await fetch(`https://localhost:7180/api/Publisher/CreatePublisher?nameOfPublisher=${nameOfPublisher}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`,
        },
    });
    location.reload(true);
}

async function deletePublishers() {
    const checkboxes = document.querySelectorAll('#publishersTable tbody input[type="checkbox"]');
    const selectedExamples = [];
    if (checkboxes.length === 0) {
        alert('Выберите хотя бы одного автора');
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
            const response = await fetch(`https://localhost:7180/api/Publisher/DeletePublisher?uIdPublisher=${uid}`, {
                method: 'DELETE',
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

async function addBook() {
    const shifr = document.getElementById('shifrForAdding').value;
    const nameOfBook = document.getElementById('nameOfBookForAdding').value;
    if (!shifr || !nameOfBook) {
        alert('Поля не должны быть пустыми');
        return;
    }

    const authors = Array.from(document.querySelectorAll('#authorsTable tr')).reduce((authorsArr, row) => {
        const checkbox = row.querySelector('input[type="checkbox"]');
        const name = row.querySelector('td:nth-child(2)').textContent;

        if (checkbox.checked) {
            authorsArr.push(name);
        }

        return authorsArr;
    }, []);

    // Получение выбранных типов книг
    const types = Array.from(document.querySelectorAll('#typesBkTable tr')).reduce((typesArr, row) => {
        const checkbox = row.querySelector('input[type="checkbox"]');
        const name = row.querySelector('td:nth-child(2)').textContent;

        if (checkbox.checked) {
            typesArr.push(name);
        }

        return typesArr;
    }, []);

    // Получение выбранных жанров
    const genres = Array.from(document.querySelectorAll('#GenresTable tr')).reduce((genresArr, row) => {
        const checkbox = row.querySelector('input[type="checkbox"]');
        const name = row.querySelector('td:nth-child(2)').textContent;

        if (checkbox.checked) {
            genresArr.push(name);
        }

        return genresArr;
    }, []);

    // Создание объекта BookUpdate
    const bookUpdate = {
        shifr: shifr,
        nameOfBook: nameOfBook,
        authorsOfBook: authors,
        typesBkOfBook: types,
        genresOfBook: genres
    };

    await fetch(`https://localhost:7180/api/Book/CreateBook?bookUpdate=${bookUpdate}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${localStorage.getItem('userToken')}`,
        }
    });
}

async function deleteBooks() {
    const checkboxes = document.querySelectorAll('#booksTable tbody input[type="checkbox"]');
    const selectedExamples = [];
    if (checkboxes.length === 0) {
        alert('Выберите хотя бы одну книгу');
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
            const response = await fetch(`https://localhost:7180/api/Book/DeleteBook?uIdBook=${uid}`, {
                method: 'DELETE',
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