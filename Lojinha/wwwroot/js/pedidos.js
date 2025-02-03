const pedidosapiUrl = 'https://localhost:7217/api/Pedido';

document.addEventListener('DOMContentLoaded', () => {
    getPedidos();

    document.getElementById('pedidosForm').addEventListener('submit', addPedido);
    document.getElementById('editPedidosForm').addEventListener('submit', updatePedido);
});

function getPedidos() {
    fetch(pedidosapiUrl)
        .then(response => response.json())
        .then(data => displayPedidos(data))
        .catch(error => console.error('Erro ao obter pedidos:', error));
}

function addPedido(event) {
    event.preventDefault();

    const pedido = {
        idclient: document.getElementById('idclient').value.trim(),
    };

    fetch(pedidosapiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(pedido),
    })
        .then(() => {
            getPedidos();
            document.getElementById('pedidosForm').reset();
        })
        .catch(error => console.error('Erro ao adicionar pedido:', error));
}

function updatePedido(event) {
    event.preventDefault();

    const idPedido = document.getElementById('edit-idpedido').value;
    const pedido = {
        idpedido: parseInt(idPedido, 10),
        idclient: document.getElementById('edit-idclient').value.trim(),
        datapedido: document.getElementById('edit-datapedido').value.trim(),
        valortotal: document.getElementById('edit-valortotal').value.trim(),
    };

    fetch(`${pedidosapiUrl}/${idPedido}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(pedido),
    })
        .then(() => {
            getPedidos();
            closeEditPedidosForm();
        })
        .catch(error => console.error('Erro ao atualizar pedido:', error));
}

function deletePedido(id) {
    fetch(`${pedidosapiUrl}/${id}`, {
        method: 'DELETE',
    })
        .then(() => getPedidos())
        .catch(error => console.error('Erro ao excluir pedido:', error));
}

function displayPedidos(pedidos) {
    const tableBody = document.getElementById('pedidoTable');
    tableBody.innerHTML = '';

    pedidos.forEach(pedido => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${pedido.idPedido}</td>
            <td>${pedido.idClient}</td>
            <td>${pedido.dataPedido}</td>
            <td>${pedido.valorTotal}</td>
            <td>
                <button onclick="showEditPedidosForm(${pedido.idPedido}, '${pedido.idClient}', '${pedido.dataPedido}', '${pedido.valorTotal}')">Editar</button>
                <button onclick="deletePedido(${pedido.idPedido})">Excluir</button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

function showEditPedidosForm(idPedido, idClient, dataPedido, valorTotal) {
    document.getElementById('edit-idpedido').value = idPedido;
    document.getElementById('edit-idclient').value = idClient;
    document.getElementById('edit-datapedido').value = dataPedido;
    document.getElementById('edit-valortotal').value = valorTotal;

    document.getElementById('editPedidosForm').classList.remove('hidden');
}

function closeEditPedidosForm() {
    document.getElementById('editPedidosForm').classList.add('hidden');
}
