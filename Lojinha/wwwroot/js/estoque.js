const estoqueApiUrl = 'https://localhost:7217/api/Estoque';

document.addEventListener('DOMContentLoaded', () => {
    getEstoque();

    document.getElementById('estoqueForm').addEventListener('submit', addEstoque);
    document.getElementById('editEstoqueForm').addEventListener('submit', updateEstoque);
});

function getEstoque() {
    fetch(estoqueApiUrl)
        .then(response => response.json())
        .then(data => displayEstoque(data))
        .catch(error => console.error('Erro ao obter estoque:', error));
}

function addEstoque(event) {
    event.preventDefault();

    const estoque = {
        nome: document.getElementById('nomeEstoque').value.trim(),
        quantidade: document.getElementById('quantidade').value.trim(),
    };

    fetch(estoqueApiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(estoque),
    })
        .then(() => {
            getEstoque();
            document.getElementById('estoqueForm').reset();
        })
        .catch(error => console.error('Erro ao adicionar item ao estoque:', error));
}

function updateEstoque(event) {
    event.preventDefault();

    const id = document.getElementById('edit-idEstoque').value;
    const estoque = {
        id: parseInt(id, 10),
        nome: document.getElementById('edit-nomeEstoque').value.trim(),
        idproduto: document.getElementById('edit-idProdutoEstoque').value.trim(),
        quantidade: document.getElementById('edit-quantidade').value.trim(),
    };
    fetch(`${estoqueApiUrl}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(estoque),
    })
        .then(() => {
            getEstoque();
            closeEditEstoqueForm();
        })
        .catch(error => console.error('Erro ao atualizar o estoque:', error));
}

function deleteEstoque(id) {
    fetch(`${estoqueApiUrl}/${id}`, {
        method: 'DELETE',
    })
        .then(() => getEstoque())
        .catch(error => console.error('Erro ao excluir item do estoque:', error));
}

function displayEstoque(estoque) {
    const tableBody = document.getElementById('estoqueTable');
    tableBody.innerHTML = '';

    estoque.forEach(estoques => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${estoques.id}</td>
            <td>${estoques.nome}</td>
            <td>${estoques.idProduto}</td>
            <td>${estoques.quantidade}</td>
            <td>
                <button onclick="showEditEstoqueForm(${estoques.id}, '${estoques.nome}', '${estoques.idProduto}', '${estoques.quantidade}')">Editar</button>
                <button onclick="deleteEstoque(${estoques.id})">Excluir</button>
            </td>
        `;
        tableBody.appendChild(row);
    })
}

function showEditEstoqueForm(id, nome, idProduto, quantidade) {
    document.getElementById('edit-idEstoque').value = id;
    document.getElementById('edit-nomeEstoque').value = nome;
    document.getElementById('edit-idProdutoEstoque').value = idProduto;
    document.getElementById('edit-quantidade').value = quantidade;

    document.getElementById('editEstoqueForm').classList.remove('hidden');
}

function closeEditEstoqueForm() {
    document.getElementById('editEstoqueForm').classList.add('hidden');
}