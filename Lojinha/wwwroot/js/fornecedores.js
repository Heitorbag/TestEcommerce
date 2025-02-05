const fornecedoresApiUrl = 'https://localhost:7217/api/Fornecedores';

document.addEventListener('DOMContentLoaded', () => {
    GetFornecedores();

    document.getElementById('fornecedoresForm').addEventListener('submit', addFornecedor);
    document.getElementById('editFornecedoresForm').addEventListener('submit', updateFornecedor);
});

function GetFornecedores() {
    fetch(fornecedoresApiUrl)
        .then(response => response.json())
        .then(data => displayFornecedor(data))
        .catch(error => console.error('Erro ao obter Fornecedor:', error));
}

function addFornecedor(event) {
    event.preventDefault();

    const fornecedores = {
        nomeFornecedor: document.getElementById('nomeFornecedor').value.trim(),
        cnpj: document.getElementById('cnpj').value.trim(),
        endereco: document.getElementById('enderecoForn').value.trim(),
        telefone: document.getElementById('telefoneForn').value.trim(),
        email: document.getElementById('emailForn').value.trim(),
    };

    fetch(fornecedoresApiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(fornecedores),
    })
        .then(() => {
            GetFornecedores();
            document.getElementById('fornecedoresForm').reset();
        })
        .catch(error => console.error('Erro ao adicionar Fornecedor:', error));
}

function updateFornecedor(event) {
    event.preventDefault();

    const id = document.getElementById('edit-idFornecedor').value;
    const fornecedores = {
        idFornecedor: parseInt(id, 10),
        nomeFornecedor: document.getElementById('edit-nomeFornecedor').value.trim(),
        cnpj: document.getElementById('edit-cnpj').value.trim(),
        endereco: document.getElementById('edit-enderecoForn').value.trim(),
        telefone: document.getElementById('edit-telefoneForn').value.trim(),
        email: document.getElementById('edit-emailForn').value.trim(),
    };
    fetch(`${fornecedoresApiUrl}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(fornecedores),
    })
        .then(() => {
            GetFornecedores();
            closeEditFornecedoresForm();
        })
        .catch(error => console.error('Erro ao atualizar o Fornecedor:', error));
}

function deleteFornecedor(id) {
    fetch(`${fornecedoresApiUrl}/${id}`, {
        method: 'DELETE',
    })
        .then(() => GetFornecedores())
        .catch(error => console.error('Erro ao excluir Fornecedor:', error));
}

function displayFornecedor(fornecedores) {
    const tableBody = document.getElementById('fornecedorTable');
    tableBody.innerHTML = '';

    fornecedores.forEach(fornecedor => {
        const row = document.createElement('tr');
        row.innerHTML = `
        <td>${fornecedor.idFornecedor}</td>
        <td>${fornecedor.nomeFornecedor}</td>
        <td>${fornecedor.cnpj}</td>
        <td>${fornecedor.endereco}</td>
        <td>${fornecedor.telefone}</td>
        <td>${fornecedor.email}</td>
        <td>
            <button onclick="showEditFornecedoresForm(${fornecedor.idFornecedor}, '${fornecedor.nomeFornecedor}', '${fornecedor.cnpj}', '${fornecedor.endereco}', '${fornecedor.telefone}', '${fornecedor.email}')">Editar</button>
            <button onclick="deleteFornecedor(${fornecedor.idFornecedor})">Excluir</button>
        </td>
    `;
        tableBody.appendChild(row);
    });
}

function showEditFornecedoresForm(idFornecedor, nomeFornecedor, cnpj, endereco, telefone, email) {
    document.getElementById('edit-idFornecedor').value = idFornecedor;
    document.getElementById('edit-nomeFornecedor').value = nomeFornecedor;
    document.getElementById('edit-cnpj').value = cnpj;
    document.getElementById('edit-enderecoForn').value = endereco;
    document.getElementById('edit-telefoneForn').value = telefone;
    document.getElementById('edit-emailForn').value = email;

    document.getElementById('editFornecedoresForm').classList.remove('hidden');
}

function closeEditFornecedoresForm() {
    document.getElementById('editFornecedoresForm').classList.add('hidden');
}