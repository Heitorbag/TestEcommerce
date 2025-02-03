const produtosApiUrl = 'https://localhost:7217/api/Produtos';

document.addEventListener('DOMContentLoaded', () => {
    getProdutos();

    document.getElementById('produtosForm').addEventListener('submit', addProduto);
    document.getElementById('editProdutosForm').addEventListener('submit', updateProduto);
});

function getProdutos() {
    fetch(produtosApiUrl)
        .then(response => response.json())
        .then(data => displayProdutos(data))
        .catch(error => console.error('Erro ao obter produtos:', error));
}

function addProduto(event) {
    event.preventDefault();

    const produto = {
        nome: document.getElementById('nomeProduto').value.trim(),
        valor: document.getElementById('valor').value.trim(),
        estoque: document.getElementById('estoque').value.trim(),
    };

    fetch(produtosApiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(produto),
    })
        .then(() => {
            getProdutos();
            document.getElementById('produtosForm').reset();
        })
        .catch(error => console.error('Erro ao adicionar produto:', error));
}

function updateProduto(event) {
    event.preventDefault();

    const idProduto = document.getElementById('edit-idproduto').value;
    const produto = {
        idProduto: parseInt(idProduto, 10),
        nome: document.getElementById('edit-nomeProduto').value.trim(),
        valor: document.getElementById('edit-valor').value.trim(),
        estoque: document.getElementById('edit-estoque').value.trim(),
    };
    fetch(`${produtosApiUrl}/${idProduto}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(produto),
    })
        .then(() => {
            getProdutos();
            closeEditProdutosForm();
        })
        .catch(error => console.error('Erro ao atualizar produto:', error));
}

function deleteProduto(id) {
    fetch(`${produtosApiUrl}/${id}`, {
        method: 'DELETE',
    })
        .then(() => getProdutos())
        .catch(error => console.error('Erro ao excluir produto:', error));
}

function displayProdutos(produtos) {
    const tableBody = document.getElementById('produtosTable');
    tableBody.innerHTML = '';

    produtos.forEach(produto => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${produto.idProduto}</td>
            <td>${produto.nome}</td>
            <td>${produto.valor}</td>
            <td>${produto.estoque}</td>
            <td>
                <button onclick="showEditProdutosForm(${produto.idProduto},'${produto.nome}', '${produto.valor}', '${produto.estoque}')">Editar</button>
                <button onclick="deleteProduto(${produto.idProduto})">Excluir</button>
            </td>
        `;
        tableBody.appendChild(row);
    })
}

function showEditProdutosForm(idProduto, nome, valor) {
    document.getElementById('edit-idproduto').value = idProduto;
    document.getElementById('edit-nomeProduto').value = nome;
    document.getElementById('edit-valor').value = valor;
    document.getElementById('edit-estoque').value = estoque;

    document.getElementById('editProdutosForm').classList.remove('hidden');
}

function closeEditProdutosForm() {
    document.getElementById('editProdutosForm').classList.add('hidden');
}