const itemsPedidosApiUrl = 'https://localhost:7217/api/ItemsPedidos';

document.addEventListener('DOMContentLoaded', () => {
    getItemsPedidos();

    document.getElementById('itemsPedidosForm').addEventListener('submit', addItemsPedido);
    document.getElementById('editItemsPedidosForm').addEventListener('submit', updateItemsPedido);
});

function getItemsPedidos() {
    fetch(itemsPedidosApiUrl)
        .then(response => response.json())
        .then(data => displayItemsPedidos(data))
        .catch(error => console.error('Erro ao obter items pedidos:', error));
}

function addItemsPedido(event) {
    event.preventDefault();

    const itemPedido = {
        valor: document.getElementById('itemsValor').value.trim(),
        idpedido: document.getElementById('itemsIdPedido').value.trim(),
        idproduto: document.getElementById('itemsIdProduto').value.trim(),
    };

    fetch(itemsPedidosApiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(itemPedido),
    })
        .then(() => {
            getItemsPedidos();
            document.getElementById('itemsPedidosForm').reset();
        })
        .catch(error => console.error('Erro ao adicionar item pedido:', error));
}

function updateItemsPedido(event) {
    event.preventDefault();

    const id = document.getElementById('edit-idItemsPedido').value;
    const itemPedido = {
        id: parseInt(id, 10),
        valor: document.getElementById('edit-itemsValor').value.trim(),
        idpedido: document.getElementById('edit-itemsIdPedido').value.trim(),
        idproduto: document.getElementById('edit-itemsIdProduto').value.trim(),
        quantidade: document.getElementById('edit-itemsQuantidade').value.trim(),
    };

    fetch(`${itemsPedidosApiUrl}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(itemPedido),
    })
        .then(() => {
            getItemsPedidos();
            closeEditItemsPedidosForm();
        })
        .catch(error => console.error('Erro ao atualizar item pedido:', error));
}

function deleteItemsPedido(id) {
    fetch(`${itemsPedidosApiUrl}/${id}`, {
        method: 'DELETE',
    })
        .then(() => getItemsPedidos())
        .catch(error => console.error('Erro ao excluir item pedido:', error));
}

function displayItemsPedidos(itemPedidos) {
    const tableBody = document.getElementById('itemsPedidoTable');
    tableBody.innerHTML = '';

    itemPedidos.forEach(itemPedido => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${itemPedido.id}</td>
            <td>${itemPedido.valor}</td>
            <td>${itemPedido.idPedido}</td>
            <td>${itemPedido.idProduto}</td>
            <td>${itemPedido.quantidade}</td>
            <td>
                <button onclick="showEditItemsPedidosForm(${itemPedido.id}, '${itemPedido.valor}', '${itemPedido.idPedido}', '${itemPedido.idProduto}', '${itemPedido.quantidade}')">Editar</button>
                <button onclick="deleteItemsPedido(${itemPedido.id})">Excluir</button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

function showEditItemsPedidosForm(id, valor, idPedido, idProduto, quantidade) {
    document.getElementById('edit-idItemsPedido').value = id;
    document.getElementById('edit-itemsValor').value = valor;
    document.getElementById('edit-itemsIdPedido').value = idPedido;
    document.getElementById('edit-itemsIdProduto').value = idProduto;
    document.getElementById('edit-itemsQuantidade').value = quantidade;

    document.getElementById('editItemsPedidosForm').classList.remove('hidden');
}

function closeEditItemsPedidosForm() {
    document.getElementById('editItemsPedidosForm').classList.add('hidden');
}